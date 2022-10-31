#version 330 core
out vec4 FragColor;

uniform vec4 customColor;

in float zDistance;
in vec3 color;

void main()
{
    //FragColor = customColor * (1 - clamp(zDistance, 0, 20) / 20);
    FragColor = vec4(color, 1);
}