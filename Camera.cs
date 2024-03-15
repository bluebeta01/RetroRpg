using OpenTK.Mathematics;

namespace RetroRpg;

public class Camera
{
    public Matrix4 ViewMatrix { get; }
    public Matrix4 ProjectionMatrix { get; }

    public Camera()
    {
        ViewMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, -10.0f);
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 16.0f / 9.0f, 0.1f, 100.0f);
    }
}