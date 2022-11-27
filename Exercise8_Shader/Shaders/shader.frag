#version 330 core
out vec4 FragColor;

uniform vec2 resolution;

void main(void) {
    vec2 uv = gl_FragCoord.xy - resolution.xy / 2;
    vec4 backgroundColor = vec4(0.12, 0.12, 0.12, 1);
    FragColor = backgroundColor;
    if (length(uv) < 100)
        FragColor = vec4(0, 0.4, 1, 1);
}