using OpenTK.Mathematics;

namespace RetroRpg;

public class Camera
{
    public Matrix4 ViewMatrix { get; private set; }
    public Matrix4 ProjectionMatrix { get; private set; }
    private Quaternion _rotation = Quaternion.Identity;
    public Vector3 Position { get; private set; }
    private Vector2 _viewport = new Vector2(1920, 1080);

    public Camera()
    {
        Position = new Vector3(0, 0, -10.0f);
        CalculateViewMatrix();
        CalculateProjectionMatrix();
    }

    private void CalculateViewMatrix()
    {
        ViewMatrix = Matrix4.CreateTranslation(Position) * Matrix4.CreateFromQuaternion(_rotation);
    }

    private void CalculateProjectionMatrix()
    {
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), _viewport.X / _viewport.Y, 0.1f, 5000.0f);
    }

    public void Resize(float width, float height)
    {
        _viewport = new Vector2(width, height);
        CalculateProjectionMatrix();
    }

    public void Rotate(float x, float y)
    {
        var newXRot = Quaternion.FromEulerAngles(x, 0.0f, 0.0f) * _rotation;
        var forward = newXRot * new Vector3(0, 1, 0);
        if (forward.Y > 0.1f)
            _rotation = newXRot;
        _rotation = _rotation * Quaternion.FromEulerAngles(0.0f, y, 0.0f);
        CalculateViewMatrix();
    }

    public Vector3 GetRay(float mouseX, float mouseY, float windowX, float windowY)
    {
        var tempMatrix = ProjectionMatrix.Inverted() * ViewMatrix.Inverted();
        var pos = new Vector4(mouseX / (windowX * 0.5f) - 1.0f, (mouseY / windowY - 0.5f) * -2f, 1.0f, 1.0f);
        return (pos * tempMatrix).Xyz.Normalized();
    }

    public void MoveForward()
    {
        float speed = 0.005f;
        Position += _rotation.Inverted() * new Vector3(0, 0, 1) * speed;
        CalculateViewMatrix();
    }
    public void MoveBack()
    {
        float speed = 0.005f;
        Position += _rotation.Inverted() * new Vector3(0, 0, -1) * speed;
        CalculateViewMatrix();
    }
    public void MoveLeft()
    {
        float speed = 0.005f;
        Position += _rotation.Inverted() * new Vector3(1, 0, 0) * speed;
        CalculateViewMatrix();
    }
    public void MoveRight()
    {
        float speed = 0.005f;
        Position += _rotation.Inverted() * new Vector3(-1, 0, 0) * speed;
        CalculateViewMatrix();
    }
    public void MoveUp()
    {
        float speed = 0.005f;
        Position += _rotation.Inverted() * new Vector3(0, 1, 0) * speed;
        CalculateViewMatrix();
    }
    public void MoveDown()
    {
        float speed = 0.005f;
        Position += _rotation.Inverted() * new Vector3(0, -1, 0) * speed;
        CalculateViewMatrix();
    }
}