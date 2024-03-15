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
