using OpenTK.Graphics.OpenGL;

namespace RetroRpg;

public class Model : Asset
{
    public int Vao { get; private set; } = -1;
    public int Ebo { get; private set; } = -1;
    public int VertexCount { get; private set; }
    public int IndexCount { get; private set; }
    
    public override Asset LoadFromDisk(string filePath)
    {
        throw new NotImplementedException();
    }

    public static Model LoadTestCube()
    {
        var model = new Model
        {
            Vao = GL.GenVertexArray(),
            Ebo = GL.GenBuffer(),
            VertexCount = Cube.Verticies.Length / 3,
            IndexCount = Cube.Elements.Length
        };
        GL.BindVertexArray(model.Vao);
        var vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Cube.Verticies.Length * sizeof(float), Cube.Verticies, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
        GL.EnableVertexAttribArray(0);
        
        var colorVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, colorVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Cube.Colors.Length * sizeof(float), Cube.Colors, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
        GL.EnableVertexAttribArray(1);

        model.Ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, model.Ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Cube.Elements.Length * sizeof(int), Cube.Elements, BufferUsageHint.StaticDraw);
        
        return model;
    }
}