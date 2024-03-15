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
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    public void RenderModel(Model model, Matrix4 modelMatrix)
    {
        if (_shader is null) return;
        
        GL.BindVertexArray(model.Vao);
        GL.UniformMatrix4(_shader.ModelMatrixLocation, true, ref modelMatrix);
        GL.UniformMatrix4(_shader.ViewMatrixLocation, true, ref _viewMatrix);
        GL.UniformMatrix4(_shader.ProjectionMatrixLocation, true, ref _projectionMatrix);
        GL.DrawArrays(PrimitiveType.Triangles, 0, model.VertexCount);
    }

    public void Present()
    {
        _window.SwapBuffers();
    }
}