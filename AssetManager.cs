namespace RetroRpg;

public class AssetManager
{
    public ColladaParser ColladaParser { get; } = new();
    public string AssetBasePath { get; } = "./Assets/";
    private Dictionary<string, Asset> _assetStore = new();

    public Asset GetAsset(Asset.AssetType type, string name)
    {
        if (_assetStore.ContainsKey(name))
            return _assetStore[name];

        Console.WriteLine($"Loading {type.ToString()} {name}");
        
        if (type == Asset.AssetType.MODEL)
        {
            var asset = new Model();
            asset.LoadFromDisk(name);
            _assetStore[name] = asset;
            return asset;
        }
        
        if (type == Asset.AssetType.TEXTURE)
        {
            var asset = new Texture();
            asset.LoadFromDisk(name);
            _assetStore[name] = asset;
            return asset;
        }
        
        if (type == Asset.AssetType.MATERIAL)
        {
            var asset = new Material();
            asset.LoadFromDisk(name);
            _assetStore[name] = asset;
            return asset;
        }

        throw new NotImplementedException();
    }
}