using System.Text.Json;

namespace RetroRpg;

public class Material : Asset
{
    public Texture ColorTexture { get; set; }
    
    public override void LoadFromDisk(string filePath)
    {
        try
        {
            LoadData(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to load material {filePath}. {e.Message}");
            throw;
        }
    }

    private void LoadData(string filePath)
    {
        var jsonData = File.ReadAllText(Game.AssetManager.AssetBasePath + "Materials/" + filePath + ".mat");
        var matDict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData);
        matDict!.TryGetValue("texture", out var textureName);
        if (textureName is null)
            throw new Exception("Failed to find texture in material definition.");
        ColorTexture = (Texture)Game.AssetManager.GetAsset(Asset.AssetType.TEXTURE, textureName);
    }
}