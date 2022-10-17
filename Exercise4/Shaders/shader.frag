#version 330 core
out vec4 FragColor;

uniform vec4 customColor;
uniform vec2 resolution;

int cicrle(vec2 location) {
    float radius = 0.5;
    float distance = location.x * location.x + location.y * location.y;
    if (distance < radius * radius) {
        return 1;
    }
    return 0;
}

void fractal(vec2 value, int depth)
{
    if (depth == 0) {
        return;
    }

    if (depth == 3) {
        FragColor = vec4(1, 0, 0, 1);
    }
    vec2 newValue = {1, };
}

void main(void)
{
    float scale = resolution.x < resolution.y ? resolution.x : resolution.y;
    vec2 location = (gl_FragCoord.xy - resolution.xy * 0.5) / scale;
    FragColor = customColor;
    vec2 start = {1, 0};
    fractal(start, 5);
}