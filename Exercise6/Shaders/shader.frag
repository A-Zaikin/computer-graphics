#version 330 core
out vec4 FragColor;

uniform vec4 customColor;

void main()
{
    FragColor = vec4(customColor.x, customColor.y, gl_FragCoord.z, 1);
}