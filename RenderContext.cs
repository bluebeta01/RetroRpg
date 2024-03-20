using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace RetroRpg;

public class RenderContext(GameWindow window)
{
    private Matrix4 _viewMatrix;
    private Matrix4 _projectionMatrix;
    private Shader? _shader;
    private GameWindow _window = window;
    private Framebuffer _colorpickFramebuffer;
    private ColorPickShader _colorpickShader;

    public void Initialize(int width, int height)
    {
        GL.DepthFunc(DepthFunction.Less);
        GL.Enable(EnableCap.DepthTest);
        _colorpickFramebuffer = new Framebuffer(width, height);
        _colorpickShader = ColorPickShader.LoadShader();
    }

    public void Resize(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
        _colorpickFramebuffer.Resize(width, height);
    }
    
    public void BindShader(Shader shader)
    {
        GL.UseProgram(shader.ShaderProgram);
        _shader = shader;
    }

    public void UseCamera(Camera camera)
    {
        _viewMatrix = camera.ViewMatrix;
        _projectionMatrix = camera.ProjectionMatrix;
    }

    public void ClearBuffer()
    {
        GL.ClearColor(0, 0, 0, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void RenderModel(Model model, Matrix4 modelMatrix)
    {
        if (_shader is null) return;

        GL.BindVertexArray(model.Vao);
        foreach (var mesh in model.Meshes)
        {
            GL.BindTexture(TextureTarget.Texture2D, mesh.Material?.ColorTexture.TextureId ?? 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.Ebo);
            GL.UniformMatrix4(_shader.ModelMatrixLocation, true, ref modelMatrix);
            GL.UniformMatrix4(_shader.ViewMatrixLocation, true, ref _viewMatrix);
            GL.UniformMatrix4(_shader.ProjectionMatrixLocation, true, ref _projectionMatrix);
            GL.DrawElements(PrimitiveType.Triangles, mesh.ElementCount, DrawElementsType.UnsignedInt, 0);
        }
    }

    public void RenderModelColorpick(Model model, Matrix4 modelMatrix, Vector3 color)
    {
        GL.BindVertexArray(model.Vao);
        GL.Uniform3(_colorpickShader.ColorUniformLocation, color);
        foreach (var mesh in model.Meshes)
        {
            GL.BindTexture(TextureTarget.Texture2D, mesh.Material?.ColorTexture.TextureId ?? 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.Ebo);
            GL.UniformMatrix4(_shader.ModelMatrixLocation, true, ref modelMatrix);
            GL.UniformMatrix4(_shader.ViewMatrixLocation, true, ref _viewMatrix);
            GL.UniformMatrix4(_shader.ProjectionMatrixLocation, true, ref _projectionMatrix);
            GL.DrawElements(PrimitiveType.Triangles, mesh.ElementCount, DrawElementsType.UnsignedInt, 0);
        }
    }

    public float GetDepthpickResult(int x, int y)
    {
        float[] pixelData = new float[1];
        GL.ReadPixels(x, y, 1, 1, PixelFormat.DepthComponent, PixelType.Float, pixelData);
        return pixelData[0];
    }

    public Vector3 GetColorpickResult(int x, int y)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _colorpickFramebuffer.GlId);
        byte[] pixelData = new byte[3];
        GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixelData);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        var r = pixelData[0] / 255.0f;
        var g = pixelData[1] / 255.0f;
        var b = pixelData[2] / 255.0f;
        return new Vector3(r, g, b);
    }

    public void BeginColorpickRender()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _colorpickFramebuffer.GlId);
        GL.ClearColor(0, 0, 0, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.UseProgram(_colorpickShader.ShaderProgram);
    }

    public void EndColorpickRender()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Present()
    {
        _window.SwapBuffers();
    }
}