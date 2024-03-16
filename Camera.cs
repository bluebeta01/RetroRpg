using OpenTK.Mathematics;

namespace RetroRpg;

public class Camera
{
    public Matrix4 ViewMatrix { get; private set; }
    public Matrix4 ProjectionMatrix { get; private set; }
    private Quaternion _rotation = Quaternion.Identity;
    private Vector3 _position = Vector3.Zero;

    public Camera()
    {
        _position = new Vector3(0, 0, -10.0f);
        CalculateViewMatrix();
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 16.0f / 9.0f, 0.1f, 100.0f);
    }

    private void CalculateViewMatrix()
    {
        ViewMatrix = Matrix4.CreateTranslation(_position) * Matrix4.CreateFromQuaternion(_rotation);
    }

    public void Rotate(float x, float y)
    {
        var newXRot = Quaternion.FromEulerAngles(x, 0.0f, 0.0f) * _rotation;
        var forward = newXRot * new Vector3(0, 1, 0);
        Console.WriteLine(forward.Y);
        if (forward.Y > 0.1f)
            _rotation = newXRot;
        _rotation = _rotation * Quaternion.FromEulerAngles(0.0f, y, 0.0f);
        CalculateViewMatrix();
    }

    public void MoveForward()
    {
        float speed = 0.005f;
        _position += _rotation.Inverted() * new Vector3(0, 0, 1) * speed;
        CalculateViewMatrix();
    }
    public void MoveBack()
    {
        float speed = 0.005f;
        _position += _rotation.Inverted() * new Vector3(0, 0, -1) * speed;
        CalculateViewMatrix();
    }
    public void MoveLeft()
    {
        float speed = 0.005f;
        _position += _rotation.Inverted() * new Vector3(1, 0, 0) * speed;
        CalculateViewMatrix();
    }
    public void MoveRight()
    {
        float speed = 0.005f;
        _position += _rotation.Inverted() * new Vector3(-1, 0, 0) * speed;
        CalculateViewMatrix();
    }
    public void MoveUp()
    {
        float speed = 0.005f;
        _position += _rotation.Inverted() * new Vector3(0, 1, 0) * speed;
        CalculateViewMatrix();
    }
    public void MoveDown()
    {
        float speed = 0.005f;
        _position += _rotation.Inverted() * new Vector3(0, -1, 0) * speed;
        CalculateViewMatrix();
    }
}