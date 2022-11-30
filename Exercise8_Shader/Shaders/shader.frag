﻿#version 330 core
out vec4 FragColor;

uniform ivec2 resolution;
uniform vec2 cameraSize;

uniform vec3 lightPosition;

uniform int sphereCount;
struct Sphere {
    vec3 position;
    float radius;
    int material;
};
uniform Sphere[32] spheres;

uniform int planeCount;
struct Plane {
    vec3 position;
    vec3 normal;
    vec3 height;
    vec3 width;
    int material;
};
uniform Plane[32] planes;

struct Material {
    vec3 color;
    float ambient;
    float diffuse;
    float specular;
    float shininess;
    float reflection;
    float refraction;
};
uniform Material[8] materials;

#define BACKGROUND_COLOR vec3(0.12, 0.12, 0.12)
#define LIGHT_GIZMO_COLOR vec3(1)
#define MAX_DEPTH 10

vec3[MAX_DEPTH] litColorStack;
float[MAX_DEPTH] reflectionStack;

struct Ray {
    vec3 origin;
    vec3 direction;
};

Ray getFirstRay() {
    vec2 uv = (gl_FragCoord.xy - resolution.xy / 2) / resolution.xy;
    return Ray(vec3(0), normalize(vec3(cameraSize * uv, 1)));
}

bool isSphereHit(Sphere sphere, Ray ray, out vec3 hitPoint, out float distanceToOrigin, out vec3 normal) {
    vec3 l = sphere.position - ray.origin;
    float tca = dot(l, ray.direction);
    if (tca < 0) {
        return false;
    }

    float lLength = length(l);
    float distanceSquared = lLength * lLength - tca * tca;
    float radiusSquared = sphere.radius * sphere.radius;
    if (distanceSquared > radiusSquared) {
        return false;
    }

    float thc = sqrt(radiusSquared - distanceSquared);
    distanceToOrigin = min(tca - thc, tca + thc);
    hitPoint = ray.origin + distanceToOrigin * ray.direction;
    normal = normalize(hitPoint - sphere.position);
    return true;
}

bool isPlaneHit(Plane plane, Ray ray, out vec3 hitPoint, out float distanceToOrigin, out vec3 normal) {
    float perpendicular = dot(plane.normal, ray.direction);
    if (perpendicular == 0) {
        return false;
    }
    distanceToOrigin = dot(plane.position - ray.origin, plane.normal) / perpendicular;
    if (distanceToOrigin < 0) {
        return false;
    }
    hitPoint = ray.origin + distanceToOrigin * ray.direction;
    vec3 pointOnPlane = hitPoint - plane.position;
    float planeHeight = length(plane.height), planeWidth = length(plane.width);
    float planePointY = dot(pointOnPlane, plane.height) / planeHeight;
    float planePointX = dot(pointOnPlane, plane.width) / planeWidth;
    if (planePointY > planeHeight / 2 || planePointY < -planeHeight / 2
        || planePointX > planeWidth / 2 || planePointX < -planeWidth / 2)
    {
        return false;
    }
    normal = perpendicular < dot(-plane.normal, ray.direction) ? plane.normal : -plane.normal;
    return true;
}

bool tryGetNearestHit(Ray ray, out vec3 hitPoint, out float distanceToOrigin, out vec3 normal, out Material material) {
    vec3 newHitPoint, newNormal;
    float newDistanceToOrigin;
    bool isObjectHit = false;

    for (int i = 0; i < sphereCount; i++) {
        if (isSphereHit(spheres[i], ray, newHitPoint, newDistanceToOrigin, newNormal)
            && (isObjectHit == false || newDistanceToOrigin < distanceToOrigin))
        {
            distanceToOrigin = newDistanceToOrigin;
            hitPoint = newHitPoint;
            normal = newNormal;
            material = materials[spheres[i].material];
            isObjectHit = true;
        }
    }

    for (int i = 0; i < planeCount; i++) {
        if (isPlaneHit(planes[i], ray, newHitPoint, newDistanceToOrigin, newNormal)
            && (isObjectHit == false || newDistanceToOrigin < distanceToOrigin))
        {
            distanceToOrigin = newDistanceToOrigin;
            hitPoint = newHitPoint;
            normal = newNormal;
            material = materials[planes[i].material];
            isObjectHit = true;
        }
    }

    return isObjectHit;
}

vec3 castRay(Ray ray, out Ray reflectedRay, out float materialReflection) {
    vec3 hitPoint, newHitPoint, normal, newNormal, _;
    float distanceToOrigin, _f, distanceToObstruction;
    Material material, _m;

    if (isSphereHit(Sphere(lightPosition, 0.2, 0), ray, _, _f, _)
        && !(tryGetNearestHit(ray, _, distanceToObstruction, _, _m) && distanceToObstruction < length(lightPosition)))
    {
        return LIGHT_GIZMO_COLOR;
    }
    if (!tryGetNearestHit(ray, hitPoint, distanceToOrigin, normal, material)) {
        return vec3(0);
    }

    vec3 reflectDirection = reflect(ray.direction, normal);
    reflectedRay = Ray(hitPoint + reflectDirection * 0.001, reflectDirection);
    materialReflection = material.reflection;

    vec3 lightDirection = normalize(hitPoint - lightPosition);
    float distanceToLight = length(lightPosition - hitPoint);
    vec3 lightColor = vec3(1);

    Ray shadowRay = Ray(hitPoint, -lightDirection);
    shadowRay.origin += shadowRay.direction * 0.001;
    float shadowAlignment = dot(normal, shadowRay.direction);
    vec3 ambient = material.ambient * lightColor;

    if (tryGetNearestHit(shadowRay, _, distanceToObstruction, _, _m) && distanceToObstruction < distanceToLight
        || shadowAlignment < 0) {
        return ambient * material.color;
    }
    float diffuseIntensity = max(shadowAlignment, 0.0);
    vec3 diffuse = material.diffuse * lightColor * diffuseIntensity;

    vec3 specularDirection = reflect(lightDirection, normal);
    float specularIntensity = pow(max(dot(-ray.direction, specularDirection), 0.0), material.shininess);
    vec3 specular = material.specular * specularIntensity * lightColor;

    return (ambient + diffuse + specular) * material.color;
}

vec3 castRecursiveRay(Ray ray) {
    float reflection;
    Ray reflectedRay;
    for (int i = 0; i < MAX_DEPTH; i++) {
        litColorStack[i] = castRay(ray, reflectedRay, reflection);
        reflectionStack[i] = reflection;
        ray = reflectedRay;
    }

    for (int i = MAX_DEPTH - 2; i >= 0; i--) {
        litColorStack[i] += litColorStack[i + 1] * reflectionStack[i];
    }
    return litColorStack[0];
}

void main() {
    vec3 color = castRecursiveRay(getFirstRay());
    if (color == vec3(0,0,0)) {
        color = BACKGROUND_COLOR;
    }
    FragColor = vec4(color, 1);
}