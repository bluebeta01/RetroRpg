namespace RetroRpg;

public abstract class Asset
{
    public enum AssetType
    {
        INVALID,
        MODEL,
        TEXTURE
    }
    
    public string Name { get; set; } = "invalid";

    public abstract void LoadFromDisk(string filePath);
}