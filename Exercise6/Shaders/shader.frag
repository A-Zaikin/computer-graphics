#version 330 core
out vec4 FragColor;

uniform vec3 customColor;
uniform int flatMode;
uniform int polyMode;
uniform vec3 cameraPos;

in float zDistance;
in vec3 color;
in vec3 fragPos;
in vec3 vertexNormal;

void main()
{
    vec3 normal = flatMode == 1 && polyMode == 1
        ? normalize(cross(dFdx(fragPos), dFdy(fragPos)))
        : vertexNormal;

    vec3 light = vec3(3, 2, -10);
    vec3 lightColor = vec3(1, 1, 1);
    vec3 lightDirection = normalize(light - fragPos);
    float intensity = max(dot(normal, lightDirection), 0);
    vec3 diffuse = intensity * lightColor;

    vec3 ambient = 0.1 * vec3(1, 1, 1);

    float specularStrength = 0.5;
    vec3 viewDir = normalize(cameraPos - fragPos);
    vec3 reflectDir = reflect(-lightDirection, normal);
    float specularIntensity = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * specularIntensity * lightColor;

//    if (shadows == 1) {
//        FragColor = vec4(clamp(color, 0, 1), 1) * intensity * 1.5;
//    } else {
//        FragColor = vec4(color, 1);
//    }

    //vec3 objectColor = vec3(0.9, 0.1, 0.2);
    FragColor = vec4((ambient + diffuse + specular) * color, 1);
}