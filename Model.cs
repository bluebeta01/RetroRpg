using OpenTK.Graphics.OpenGL;

namespace RetroRpg;

public class Model : Asset
{
    public int Vao { get; private set; } = -1;
    public int VertexCount { get; private set; }
    
    public override Asset LoadFromDisk(string filePath)
    {
        throw new NotImplementedException();
    }

    public static Model LoadTestCube()
    {
        var model = new Model
        {
            Vao = GL.GenVertexArray(),
            VertexCount = 3
        };
        GL.BindVertexArray(model.Vao);
        var vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Cube.Verticies.Length * sizeof(float), Cube.Verticies, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, sizeof(float) * 3);
        GL.EnableVertexAttribArray(1);
        
        return model;
    }
}