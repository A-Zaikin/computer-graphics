#version 330 core
out vec4 FragColor;

uniform vec4 customColor;
uniform int shadows;

in float zDistance;
in vec3 color;
in vec3 viewPosition;

void main()
{
    //FragColor = customColor * (1 - clamp(zDistance, 0, 20) / 20);
    //FragColor = vec4(color, 1);

    vec3 xTangent = dFdx(viewPosition);
    vec3 yTangent = dFdy(viewPosition);
    vec3 normal = normalize(cross(xTangent, yTangent));
    vec3 light = vec3(-100, 30, -100);
    vec3 lightDirection = normalize(viewPosition - light);
    float intensity = dot(lightDirection, normal);

    if (shadows == 1) {
        FragColor = vec4(clamp(color, 0, 1), 1) * intensity * 1.5;
    } else {
        FragColor = vec4(color, 1);
    }
}