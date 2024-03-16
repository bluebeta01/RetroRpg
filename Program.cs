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
        _window.MouseMove += OnMouseMove;
        _window.Load += OnLoad;
        _renderContext = new(_window);
        _window.Run();
    }

    private static void OnMouseMove(MouseMoveEventArgs obj)
    {
        float sensitivity = 0.01f;
        if (_window.MouseState.IsButtonDown(MouseButton.Button2))
        {
            _camera.Rotate(obj.DeltaY * sensitivity, obj.DeltaX * sensitivity);
        }
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

        //_rotY += (float)(args.Time);
        //_rotX += (float)(0.2 * args.Time);
    }

    private static void OnUpdateFrame(FrameEventArgs e)
    {
        if(_window.KeyboardState.IsKeyDown(Keys.Escape))
        {
            _window.Close();
        }
        if(_window.KeyboardState.IsKeyDown(Keys.W))
            _camera.MoveForward();
        if(_window.KeyboardState.IsKeyDown(Keys.S))
            _camera.MoveBack();
        if(_window.KeyboardState.IsKeyDown(Keys.A))
            _camera.MoveLeft();
        if(_window.KeyboardState.IsKeyDown(Keys.D))
            _camera.MoveRight();
        if(_window.KeyboardState.IsKeyDown(Keys.Q))
            _camera.MoveUp();
        if(_window.KeyboardState.IsKeyDown(Keys.E))
            _camera.MoveDown();
    }
}