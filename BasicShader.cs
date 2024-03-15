using OpenTK.Graphics.OpenGL;

namespace RetroRpg;

public class BasicShader : Shader
{
    private static void CheckShaderResult(int shader, int result)
    {
        if (result == 0)
        {
            string glCompileInfo = GL.GetShaderInfoLog(shader);
            Console.WriteLine(glCompileInfo);
        }
    }

    public static BasicShader LoadShader()
    {
        var vertSrc = File.ReadAllText("Assets/Shaders/basic_vert.glsl");
        var vertShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertShader, vertSrc);
        GL.CompileShader(vertShader);
        GL.GetShader(vertShader, ShaderParameter.CompileStatus, out var compileResult);
        CheckShaderResult(vertShader, compileResult);
        
        var fragSrc = File.ReadAllText("Assets/Shaders/basic_frag.glsl");
        var fragShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragShader, fragSrc);
        GL.CompileShader(fragShader);
        GL.GetShader(fragShader, ShaderParameter.CompileStatus, out compileResult);
        CheckShaderResult(vertShader, compileResult);

        BasicShader shader = new();
        shader.ShaderProgram = GL.CreateProgram();
        GL.AttachShader(shader.ShaderProgram, vertShader);
        GL.AttachShader(shader.ShaderProgram, fragShader);
        GL.LinkProgram(shader.ShaderProgram);
        GL.GetProgram(shader.ShaderProgram, GetProgramParameterName.LinkStatus, out compileResult);
        CheckShaderResult(shader.ShaderProgram, compileResult);
        
        shader.ModelMatrixLocation = GL.GetUniformLocation(shader.ShaderProgram, "model");
        shader.ViewMatrixLocation = GL.GetUniformLocation(shader.ShaderProgram, "view");
        shader.ProjectionMatrixLocation = GL.GetUniformLocation(shader.ShaderProgram, "projection");

        return shader;
    }
}