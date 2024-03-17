namespace RetroRpg;

public class AssetManager
{
    public static ColladaParser ColladaParser { get; } = new();
    private Dictionary<string, Asset> _assetStore = new();

    public Asset GetAsset(Asset.AssetType type, string name)
    {
        if (_assetStore.ContainsKey(name))
            return _assetStore[name];

        if (type == Asset.AssetType.MODEL)
        {
            var asset = new Model();
            asset.LoadFromDisk(name);
            _assetStore[name] = asset;
            return asset;
        }

        throw new NotImplementedException();
    }

    public Model GetTestCube()
    {
        if (_assetStore.ContainsKey("_testCube"))
            return (Model)_assetStore["_testCube"];

        var model =  Model.LoadTestCube();
        _assetStore["_testCube"] = model;
        return model;
    }
}