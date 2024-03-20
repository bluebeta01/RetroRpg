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
    private static RenderContext _renderContext = default!;
    private static Camera _camera = new();
    private static Model _testDae = default!;
    private static Model _plane = default!;
    private static Model _sphere = default!;
    private static Vector3 _sphereLocation = new();
    
    public static void Main(string[] args)
    {
        _window = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);
        _window.UpdateFrame += OnUpdateFrame;
        _window.RenderFrame += OnRenderFrame;
        _window.MouseMove += OnMouseMove;
        _window.Load += OnLoad;
        _window.Resize += OnResize;
        _renderContext = new(_window);
        _window.Run();
    }

    private static void OnResize(ResizeEventArgs obj)
    {
        _renderContext.Resize(obj.Width, obj.Height);
        _camera.Resize(_window.ClientSize.X, _window.ClientSize.Y);
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
        _renderContext.Initialize(_window.Size.X, _window.Size.Y);
        Game.Initialize();
        _basicShader = BasicShader.LoadShader();
        //_testDae = (Model)Game.AssetManager.GetAsset(Asset.AssetType.MODEL, "C:/game/assets/cube.dae");
        _plane = (Model)Game.AssetManager.GetAsset(Asset.AssetType.MODEL, "C:/game/assets/plane.dae");
        _sphere = (Model)Game.AssetManager.GetAsset(Asset.AssetType.MODEL, "C:/game/assets/sphere.dae");
    }

    private static void OnRenderFrame(FrameEventArgs args)
    {
        _renderContext.ClearBuffer();
        _renderContext.BindShader(_basicShader);
        _renderContext.UseCamera(_camera);
        //_renderContext.RenderModel(_testDae, Matrix4.CreateRotationX(_rotX) * Matrix4.CreateRotationY(_rotY));
        _renderContext.RenderModel(_sphere, Matrix4.CreateTranslation(_sphereLocation));
        _renderContext.RenderModel(_plane, Matrix4.Identity);
        _renderContext.Present();
        

        if (_window.MouseState.IsButtonPressed(MouseButton.Button1) || true)
        {
            var ray = _camera.GetRay(_window.MouseState.X, _window.MouseState.Y, (float)_window.ClientSize.X,
                (float)_window.ClientSize.Y);
            var yDir = ray.Y;
            var dist = _camera.Position.Y / yDir;
            _sphereLocation = ray * dist - _camera.Position;
        }

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