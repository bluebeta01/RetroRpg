﻿#version 400
layout (location = 0) in vec3 vertPos;
layout (location = 1) in vec3 vertNormal;
layout (location = 2) in vec2 vertUv;
out vec4 vertexColor;
out vec2 texCoord;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
void main()
{
    gl_Position = vec4(vertPos, 1.0f) * model * view * projection;
    vertexColor = vec4(1.0f, 1.0f, 1.0f, 1.0f);
    texCoord = vertUv;
}
