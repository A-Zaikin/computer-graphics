#version 330 core
layout (location = 0) in vec3 aPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out float zDistance;
out vec3 color;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    zDistance = gl_Position.z;
    color = vec3(aPosition);
}