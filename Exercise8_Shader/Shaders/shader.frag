#version 330 core
out vec4 FragColor;


struct Sphere {
    vec3 position;
    float radius;
    int materialId;
};

struct Plane {
    vec3 position;
    vec3 normal;
    vec3 height;
    vec3 width;
    bool notTiled;
    int materialId;
};

struct Material {
    vec3 color;
    float ambient;
    float diffuse;
    float specular;
    float shininess;
    float reflection;
    float refraction;
    float refractiveIndex;
};

struct Ray {
    vec3 origin;
    vec3 direction;
};

struct Hit {
    vec3 point;
    float distanceTo;
    vec3 normal;
    vec3 refractionDirection;
    Material material;
};

struct Light {
    vec3 position;
    vec3 color;
};


uniform ivec2 resolution;
uniform vec2 cameraSize;
uniform vec2 lookAngle;
uniform vec3 cameraPosition;
uniform int maxDepth;

uniform vec3 backgroundColor;

uniform int sphereCount;
uniform Sphere[32] spheres;

uniform int planeCount;
uniform Plane[32] planes;

uniform Material[8] materials;

uniform int lightCount;
uniform Light[8] lights;


#define STACK_SIZE 256

vec3[STACK_SIZE] colorStack;
Ray[STACK_SIZE] rayStack;
float[STACK_SIZE] reflectionStack;
float[STACK_SIZE] refractionStack;
bool[STACK_SIZE] skipRay = bool[STACK_SIZE](false);


vec2 rotate(vec2 v, float angle) {
    return vec2(
        v.x * cos(angle) - v.y * sin(angle),
        v.x * sin(angle) + v.y * cos(angle));
}

int ipow(int base, int power) {
    int num = base;
    for (int i = 0; i < power - 1; i++) {
        num *= base;
    }
    return num;
}

Ray getCameraRay() {
    vec2 uv = (gl_FragCoord.xy - resolution.xy / 2) / resolution.xy;
    vec3 direction = vec3(cameraSize * uv, 1);
    direction.yz = rotate(direction.yz, lookAngle.y);
    direction.xz = rotate(direction.xz, -lookAngle.x);
    return Ray(cameraPosition, normalize(direction));
}

bool isSphereHit(Sphere sphere, Ray ray, out Hit hit)
{
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
    hit.distanceTo = thc > tca ? tca + thc : min(tca - thc, tca + thc);
    hit.point = ray.origin + hit.distanceTo * ray.direction;
    hit.normal = normalize(hit.point - sphere.position);

    float cosTheta = min(dot(-ray.direction, hit.normal), 1);
    float sinTheta = sqrt(1 - cosTheta * cosTheta);

    hit.material = materials[sphere.materialId];
    float refractionRatio;
    if (dot(ray.direction, hit.normal) < 0) {
        refractionRatio = 1.0 / hit.material.refractiveIndex;
    } else {
        refractionRatio = hit.material.refractiveIndex;
        hit.normal *= -1;
    }

    if (sinTheta * refractionRatio > 1) {
        hit.refractionDirection = reflect(ray.direction, hit.normal);
    } else {
        hit.refractionDirection = refract(ray.direction, hit.normal, refractionRatio);
    }

    return true;
}

bool isPlaneHit(Plane plane, Ray ray, out Hit hit)
{
    float perpendicular = dot(plane.normal, ray.direction);
    if (perpendicular == 0) {
        return false;
    }
    hit.distanceTo = dot(plane.position - ray.origin, plane.normal) / perpendicular;
    if (hit.distanceTo < 0) {
        return false;
    }
    hit.point = ray.origin + hit.distanceTo * ray.direction;

    vec3 pointOnPlane = hit.point - plane.position;
    float planeWidth = length(plane.width);
    float planeHeight = length(plane.height);
    float planePointY = dot(pointOnPlane, plane.height) / planeHeight;
    float planePointX = dot(pointOnPlane, plane.width) / planeWidth;
    if (planePointY > planeHeight / 2 || planePointY < -planeHeight / 2
        || planePointX > planeWidth / 2 || planePointX < -planeWidth / 2)
    {
        return false;
    }

    hit.material = materials[plane.materialId];
    if (planePointY < 0) {
        planePointY--;
    }
    if (planePointX < 0) {
        planePointX--;
    }
    if (!plane.notTiled && (int(planePointY) + int(planePointX)) % 2 == 0) {
        hit.material.color /= 1.5;
    }

    hit.normal = perpendicular < dot(-plane.normal, ray.direction) ? plane.normal : -plane.normal;
    hit.refractionDirection = ray.direction;

    return true;
}

bool tryGetNearestHit(Ray ray, out Hit hit)
{
    Hit newHit;
    bool isObjectHit = false;

    for (int i = 0; i < sphereCount; i++) {
        if (isSphereHit(spheres[i], ray, newHit)
            && (isObjectHit == false || newHit.distanceTo < hit.distanceTo))
        {
            hit = newHit;
            isObjectHit = true;
        }
    }

    for (int i = 0; i < planeCount; i++) {
        if (isPlaneHit(planes[i], ray, newHit)
            && (isObjectHit == false || newHit.distanceTo < hit.distanceTo))
        {
            hit = newHit;
            isObjectHit = true;
        }
    }

    return isObjectHit;
}

bool getLightObstruction(Ray shadowRay, float distanceToLight) {
    Hit hit;
    float lightObstruction;

    for (int i = 0; i < sphereCount; i++) {
        if (isSphereHit(spheres[i], shadowRay, hit)
            && hit.distanceTo < distanceToLight
            && hit.material.refraction == 0)
        {
            return true;
        }
    }

    for (int i = 0; i < planeCount; i++) {
        if (isPlaneHit(planes[i], shadowRay, hit)
            && hit.distanceTo < distanceToLight
            && hit.material.refraction == 0)
        {
            return true;
        }
    }

    return false;
}

vec3 getDirectLightColor(Ray ray, Light light) {
    Hit hit;

    vec3 lightDirection = normalize(light.position - ray.origin);
    float size = max(dot(ray.direction, lightDirection), 0.0);
    float lightDistance = length(light.position - ray.origin);
    vec3 color = (pow(size, 256.0) + 0.2*pow(size, 2.0)) * light.color;
    Ray sunRay = Ray(ray.origin, lightDirection);
    if (!tryGetNearestHit(sunRay, hit) || hit.distanceTo > lightDistance) {
        return color;
    }
    return vec3(0);
}

vec3 getAllDirectLightColors(Ray ray) {
    vec3 color = vec3(0);
    for (int i = 0; i < lightCount; i++) {
        color += getDirectLightColor(ray, lights[i]);
    }
    return color;
}

vec3 getLighting(Ray ray, Hit hit, Light light) {
    // shadow
    vec3 lightDirection = normalize(hit.point - light.position);
    float distanceToLight = length(light.position - hit.point);

    Ray shadowRay = Ray(hit.point, -lightDirection);
    shadowRay.origin += shadowRay.direction * 0.001;
    float shadowAlignment = dot(hit.normal, shadowRay.direction);
    vec3 ambient = hit.material.ambient * vec3(1);

    // diffuse
    float diffuseIntensity = max(shadowAlignment, 0.0);
    vec3 diffuse = hit.material.diffuse * light.color * diffuseIntensity;

    // specular
    vec3 specular = vec3(0);
    if (hit.material.shininess > 0) {
        vec3 specularDirection = reflect(lightDirection, hit.normal);
        float specularIntensity = pow(max(dot(-ray.direction, specularDirection), 0.0), hit.material.shininess);
        specular = hit.material.specular * specularIntensity * light.color;
    }

    bool isLightObstructed = getLightObstruction(shadowRay, distanceToLight);
    if (shadowAlignment < 0 || isLightObstructed) {
        return ambient;
    }

    return ambient + diffuse + specular;
}

vec3 getAllLighting(Ray ray, Hit hit) {
    vec3 color = vec3(0);
    for (int i = 0; i < lightCount; i++) {
        color += getLighting(ray, hit, lights[i]);
    }
    return color;
}

vec3 castRay(Ray ray, out Ray reflectedRay, out Ray refractedRay,
    out float materialReflection, out float materialRefraction)
{
    Hit hit, newHit;
    materialReflection = materialRefraction = 0;

    // light gizmo
    vec3 directLightColor = getAllDirectLightColors(ray);

    // main hit
    if (!tryGetNearestHit(ray, hit)) {
        materialReflection = materialRefraction = 0;
        reflectedRay = refractedRay = ray;
        return directLightColor + backgroundColor;
    }

    // reflection and refraction
    vec3 reflectDirection = reflect(ray.direction, hit.normal);
    reflectedRay = Ray(hit.point + reflectDirection * 0.001, reflectDirection);
    materialReflection = hit.material.reflection;

    refractedRay = Ray(hit.point + hit.refractionDirection * 0.001, hit.refractionDirection);
    materialRefraction = hit.material.refraction;

    vec3 lighting = getAllLighting(ray, hit);
    return directLightColor + lighting * hit.material.color;
}

vec3 castRecursiveRay(Ray ray) {
    float reflection, refraction;
    Ray reflectedRay, refractedRay;
    int rayCount = ipow(2, maxDepth), continuedRayCount = ipow(2, maxDepth - 1);

    if (maxDepth == 1) {
        return castRay(ray, reflectedRay, refractedRay, reflection, refraction);
    }

    rayStack[1] = ray;
    for (int i = 1; i < rayCount; i++) {
        if (skipRay[i]) {
            colorStack[i] = vec3(0);
        } else {
            colorStack[i] = castRay(rayStack[i], reflectedRay, refractedRay, reflection, refraction);
            reflectionStack[i] = reflection;
            refractionStack[i] = refraction;
        }

        if (i < continuedRayCount) {
            if (reflection == 0 || skipRay[i]) {
                skipRay[i * 2] = true;
            } else {
                rayStack[i * 2] = reflectedRay;
            }

            if (refraction == 0 || skipRay[i]) {
                skipRay[i * 2 + 1] = true;
            } else {
                rayStack[i * 2 + 1] = refractedRay;
            }
        }
    }

    for (int i = rayCount - 1; i > 0; i--) {
        if (skipRay[i]) {
            continue;
        }
        float coeff = i % 2 == 0 ? reflectionStack[i / 2] : refractionStack[i / 2];
        colorStack[i / 2] += colorStack[i] * coeff;
    }
    return colorStack[1];
}

void main() {
    vec3 color = castRecursiveRay(getCameraRay());
    FragColor = vec4(color, 1);
}