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
        var model = Game.AssetManager.ColladaParser.ParseFile(filePath);
        if (model is not null)
        {
            Vao = model.Vao;
            VertexCount = model.VertexCount;
            Meshes = model.Meshes;
        }
    }
}