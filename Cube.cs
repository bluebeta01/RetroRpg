namespace RetroRpg;

public class Cube
{
    public static float[] Verticies { get; } =
    [
        0, 0, 0,
        1.0f, 0.0f, 0.0f,

        0, 1.0f, 0,
        0.0f, 0.0f, 1.0f,
        
        1.0f, 1.0f, 0,
        0.0f, 1.0f, 0.0f,

    ];

    public static string VertShaderText { get; } = @"
#version 400
layout (location = 0) in vec3 vertPos;
layout (location = 1) in vec3 vertColor;
out vec4 vertexColor;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
void main()
{
    gl_Position = vec4(vertPos, 1.0f) * model * view * projection;
    vertexColor = vec4(vertColor, 1.0f);
}
";

    public static string FragShaderText { get; } = @"
#version 400
out vec4 FragColor;
in vec4 vertexColor;
void main()
{
    FragColor = vertexColor;
}
";
}