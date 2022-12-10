#version 330 core
out vec4 FragColor;

uniform int flatMode;
uniform int polyMode;
uniform vec3 cameraPos;

uniform int generateColors;
uniform vec3 objectColor;
uniform float objectSpecularStrength;
uniform float objectDiffuseStrength;

uniform int isObjectTextured;

uniform sampler2D texture0;

in float zDistance;
in vec3 color;
in vec3 fragPos;
in vec3 vertexNormal;
in vec2 texCoord;

vec3 get_light(vec3 lightPosition, vec3 lightColor, vec3 normal)
{
    float distanceToLight = length(lightPosition - fragPos);
    float lightAttenuation = 1 / (1 + distanceToLight);

    vec3 lightDirection = normalize(lightPosition - fragPos);

    float intensity = max(dot(normal, lightDirection), 0);
    vec3 diffuse = intensity * lightColor * lightAttenuation;
    if (generateColors == 0) {
        diffuse *= objectDiffuseStrength;
    }

    vec3 ambient = 0.1 * vec3(1, 1, 1);

    float specularStrength = generateColors == 1 ? 0.5 : objectSpecularStrength;
    vec3 viewDir = normalize(cameraPos - fragPos);
    vec3 reflectDir = reflect(-lightDirection, normal);
    float specularIntensity = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * specularIntensity * lightColor * lightAttenuation;

    return diffuse + ambient + specular;
}

void main()
{
    vec3 normal = flatMode == 1 && polyMode == 1
        ? normalize(cross(dFdx(fragPos), dFdy(fragPos)))
        : normalize(vertexNormal);

    vec3 lightPosition = vec3(-3, 2, 10);
    vec3 lightColor = vec3(1, 1, 1) * 12;
    vec3 mainLight = get_light(lightPosition, lightColor, normal);

    vec3 finalColor = generateColors == 1 ? normalize(color) : normalize(objectColor);
    if (isObjectTextured == 1) {
        finalColor *= texture(texture0, texCoord).xyz;
    }
    FragColor = vec4(mainLight * finalColor, 1);
    //FragColor = texture(texture0, texCoord).xy;
}