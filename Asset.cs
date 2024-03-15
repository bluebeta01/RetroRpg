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

    public abstract Asset LoadFromDisk(string filePath);
}