﻿#version 400
out vec4 FragColor;
in vec4 vertexColor;
in vec2 texCoord;
uniform sampler2D diffuseTexture;
void main()
{
    FragColor = texture(diffuseTexture, texCoord);
}
