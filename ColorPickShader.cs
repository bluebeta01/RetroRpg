using OpenTK.Graphics.OpenGL;

namespace RetroRpg;

public class ColorPickShader : Shader
{
    public int ColorUniformLocation { get; set; } = -1;
    
    private static void CheckShaderResult(int shader, int result)
    {
        if (result == 0)
        {
            string glCompileInfo = GL.GetShaderInfoLog(shader);
            Console.WriteLine(glCompileInfo);
        }
    }

    public static ColorPickShader LoadShader()
    {
        var vertSrc = File.ReadAllText("Assets/Shaders/colorpick_vert.glsl");
        var vertShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertShader, vertSrc);
        GL.CompileShader(vertShader);
        GL.GetShader(vertShader, ShaderParameter.CompileStatus, out var compileResult);
        CheckShaderResult(vertShader, compileResult);
        
        var fragSrc = File.ReadAllText("Assets/Shaders/colorpick_frag.glsl");
        var fragShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragShader, fragSrc);
        GL.CompileShader(fragShader);
        GL.GetShader(fragShader, ShaderParameter.CompileStatus, out compileResult);
        CheckShaderResult(vertShader, compileResult);

        ColorPickShader shader = new();
        shader.ShaderProgram = GL.CreateProgram();
        GL.AttachShader(shader.ShaderProgram, vertShader);
        GL.AttachShader(shader.ShaderProgram, fragShader);
        GL.LinkProgram(shader.ShaderProgram);
        GL.GetProgram(shader.ShaderProgram, GetProgramParameterName.LinkStatus, out compileResult);
        CheckShaderResult(shader.ShaderProgram, compileResult);
        
        shader.ModelMatrixLocation = GL.GetUniformLocation(shader.ShaderProgram, "model");
        shader.ViewMatrixLocation = GL.GetUniformLocation(shader.ShaderProgram, "view");
        shader.ProjectionMatrixLocation = GL.GetUniformLocation(shader.ShaderProgram, "projection");
        shader.ColorUniformLocation = GL.GetUniformLocation(shader.ShaderProgram, "colorPickColor");

        return shader;
    }
    
}