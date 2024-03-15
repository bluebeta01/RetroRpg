using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RetroRpg;

public static class Program
{
    private static GameWindow _window = default!;
    private static float _rotY;
    private static Shader _basicShader = default!;
    private static Model _testCubeModel = default!;
    private static RenderContext _renderContext = default!;
    private static Camera _camera = new();
    
    public static void Main(string[] args)
    {
        _window = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);
        _window.UpdateFrame += OnUpdateFrame;
        _window.RenderFrame += OnRenderFrame;
        _window.Load += OnLoad;
        _renderContext = new(_window);
        _window.Run();
    }

    private static void OnLoad()
    {
        _basicShader = BasicShader.LoadShader();
        _testCubeModel = Model.LoadTestCube();
    }

    private static void OnRenderFrame(FrameEventArgs args)
    {
        _renderContext.ClearBuffer();
        _renderContext.BindShader(_basicShader);
        _renderContext.UseCamera(_camera);
        _renderContext.RenderModel(_testCubeModel, Matrix4.Identity);
        _renderContext.Present();
    }

    private static void OnUpdateFrame(FrameEventArgs e)
    {
        if(_window.KeyboardState.IsKeyDown(Keys.Escape))
        {
            _window.Close();
        }
    }
}