#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 worldRotation;

out float zDistance;
out vec3 color;
out vec3 fragPos;
out vec3 vertexNormal;

void main()
{
    vec4 model_pos = vec4(aPosition, 1.0) * model * worldRotation;
    gl_Position = model_pos * view * projection;
    //color = (aPosition + vec3(1, 1, 1)) / 2;
    vec3 colorPos = aPosition;
    colorPos.z *= -1;
    color = normalize(normalize(colorPos) + vec3(1,1,1));
    fragPos = model_pos.xyz;
    vertexNormal = (vec4(aNormal, 1.0) * transpose(inverse(model * worldRotation))).xyz;
}