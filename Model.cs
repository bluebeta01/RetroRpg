using System.Xml;
using OpenTK.Graphics.OpenGL;

namespace RetroRpg;

public class Model : Asset
{
    public int Vao { get; set; } = -1;
    public int VertexCount { get; set; }
    public Mesh[] Meshes { get; set; } = Array.Empty<Mesh>();

    public override void LoadFromDisk(string filePath)
    {
        var model = AssetManager.ColladaParser.ParseFile(filePath);
        if (model is not null)
        {
            Vao = model.Vao;
            VertexCount = model.VertexCount;
            Meshes = model.Meshes;
        }
    }

    public static Model LoadTestCube()
    {
        var model = new Model
        {
            Vao = GL.GenVertexArray(),
            VertexCount = Cube.Verticies.Length / 3,
            Meshes = 
            [
                new Mesh()
                {
                    Ebo = GL.GenBuffer(),
                    ElementCount = Cube.Elements.Length
                }
            ]
        };
        var mesh = model.Meshes[0];
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

        mesh.Ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.Ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Cube.Elements.Length * sizeof(int), Cube.Elements, BufferUsageHint.StaticDraw);
        
        return model;
    }
}