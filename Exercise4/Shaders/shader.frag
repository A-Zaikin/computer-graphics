#version 330 core
out vec4 FragColor;

uniform vec2 resolution;
uniform vec2 translate;
uniform float scale;

#define MAX_DEPTH 7
#define PIXELS_PER_UNIT 2187

int carpet_fractal(vec2 uv) {
    uv = abs(uv - int(uv));

    int depth = 0;
    float square_size = 1.0 / 3;
    float cell_x = int(uv.x);
    float cell_y = int(uv.y);

    while (depth < MAX_DEPTH) {
        if (uv.x > cell_x + square_size
            && uv.x < cell_x + 2 * square_size
            && uv.y > cell_y + square_size
            && uv.y < cell_y + 2 * square_size) {
            return 0;
        }
        depth++;
        cell_x += square_size * int((uv.x - cell_x) / square_size);
        cell_y += square_size * int((uv.y - cell_y) / square_size);
        square_size /= 3;
    }
    return 1;
}

void main(void) {
    vec2 uv = (gl_FragCoord.xy + translate) / PIXELS_PER_UNIT;
    uv /= scale;

    if (carpet_fractal(uv) == 1) {
        FragColor = vec4(0.184, 0.729, 0.729, 1);
    } else {
        FragColor = vec4(0, 0, 0, 0);
    }
}