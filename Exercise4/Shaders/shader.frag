#version 330 core
out vec4 FragColor;

uniform vec2 resolution;
uniform vec2 translate;
uniform float scale;
uniform int mode;
uniform float rotation;

#define MAX_DEPTH 7
#define PIXELS_PER_UNIT_CARPET 2187
#define PIXELS_PER_UNIT 400
#define PI 3.1415926538

#define MODE_CARPET 0
#define MODE_TRIANGLE 1
#define MODE_CROSS 2

vec2 reflect_point(vec2 point, float line) {
    float b = -line;
    return vec2(
        (point.x*(1 - b*b) - 2*b*point.y)/(1 + b*b),
        (point.y*(b*b - 1) - 2*b*point.x)/(1 + b*b));
}

vec2 rotate(vec2 point, float angle) {
    return vec2(
        point.x*cos(angle) - point.y*sin(angle),
        point.x*sin(angle) + point.y*cos(angle));
}

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

float triangle_DE(vec2 uv) {
    float r = 1;
    vec2 a1 = vec2(0, rotation / PI * 4);
    vec2 a2 = vec2(cos(7*PI/6)*r, sin(7*PI/6)*r);
    vec2 a3 = vec2(cos(-PI/6)*r, sin(-PI/6)*r);

    vec2 a13 = (a1 + a3) / 2;
    float line1 = (a13.y - a2.y)/(a13.x - a2.x);
    vec2 a12 = (a1 + a2) / 2;
    float line2 = (a3.y - a12.y)/(a3.x - a12.x);

	int n = 0;
    float shape_scale = 2.0;
    while (n < 28) {
        if (uv.y < line1*uv.x) {
            uv = reflect_point(uv, line1);
        }
        if (uv.y < line2*uv.x) {
            uv = reflect_point(uv, line2);
        }

        uv = shape_scale*uv - a1*(shape_scale - 1.0);
        n++;
    }
    return length(uv) * pow(shape_scale, float(-n));
}

float cross_DE(vec2 uv) {
    vec2 a = vec2(1, 1);

	int n = 0;
    float shape_scale = 2;
    while (n < 28) {
        uv = rotate(uv, rotation);
        uv = abs(uv);
        uv = shape_scale*uv - a*(shape_scale-1.0);
        n++;
    }
    return length(uv) * pow(shape_scale, float(-n));
}

void main(void) {
    vec2 uv;
    if (mode == MODE_CARPET) {
        uv = gl_FragCoord.xy - resolution.xy/2 + translate;
        uv /= PIXELS_PER_UNIT_CARPET;
        uv /= scale;
    } else {
        uv = gl_FragCoord.xy - resolution.xy/2;
        uv /= scale;
        uv += translate;
        uv /= PIXELS_PER_UNIT;
    }

    if (mode == MODE_CARPET) {
        FragColor = carpet_fractal(uv) == 1
            ? vec4(0.184, 0.729, 0.729, 1)
            : vec4(0, 0, 0, 0);
    } else if (mode == MODE_TRIANGLE) {
        FragColor = triangle_DE(uv) < 1.0 / PIXELS_PER_UNIT / scale
            ? vec4(0.8, 0, 0.3, 1)
            : vec4(0, 0, 0, 0);
    } else if (mode == MODE_CROSS) {
        FragColor = cross_DE(uv) < 1.0 / PIXELS_PER_UNIT / scale
            ? vec4(0.2, 1, 0.5, 1)
            : vec4(0, 0, 0, 0);
    }
}