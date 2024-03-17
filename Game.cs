namespace RetroRpg;

public static class Game
{
    public static AssetManager AssetManager { get; private set; }

    public static void Initialize()
    {
        AssetManager = new AssetManager();
    }
}