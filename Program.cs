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
    private static float _rotX;
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
        _renderContext.Initialize();
        _basicShader = BasicShader.LoadShader();
        _testCubeModel = Model.LoadTestCube();
    }

    private static void OnRenderFrame(FrameEventArgs args)
    {
        _renderContext.ClearBuffer();
        _renderContext.BindShader(_basicShader);
        _renderContext.UseCamera(_camera);
        _renderContext.RenderModel(_testCubeModel, Matrix4.CreateRotationX(_rotX) * Matrix4.CreateRotationY(_rotY));
        _renderContext.Present();

        _rotY += (float)(args.Time);
        _rotX += (float)(0.2 * args.Time);
    }

    private static void OnUpdateFrame(FrameEventArgs e)
    {
        if(_window.KeyboardState.IsKeyDown(Keys.Escape))
        {
            _window.Close();
        }
    }
}