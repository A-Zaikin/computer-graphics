#version 330 core
out vec4 FragColor;

uniform ivec2 resolution;
uniform vec2 cameraSize;
uniform vec2 lookAngle;
uniform vec3 cameraPosition;

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
    float refractiveIndex;
};
uniform Material[8] materials;

uniform int maxDepth;

#define BACKGROUND_COLOR vec3(0.5, 0.5, 0.5)
#define LIGHT_GIZMO_COLOR vec3(1)

struct Ray {
    vec3 origin;
    vec3 direction;
};

vec3[128] colorStack;
Ray[128] rayStack;
float[128] reflectionStack;
float[128] refractionStack;

vec2 rotate(vec2 v, float angle) {
    return vec2(
        v.x * cos(angle) - v.y * sin(angle),
        v.x * sin(angle) + v.y * cos(angle));
}

vec3 refractVector(vec3 direction, vec3 normal, float refractionRatio) {
    float cosTheta = min(dot(-direction, normal), 1);
    vec3 outPerpendicular = refractionRatio * (direction + cosTheta * normal);
    float perpLengthSquared = outPerpendicular.x * outPerpendicular.x
        + outPerpendicular.y * outPerpendicular.y
        + outPerpendicular.z * outPerpendicular.z;
    vec3 outParallel = -sqrt(abs(1 - perpLengthSquared)) * normal;
    return outPerpendicular + outParallel;
}

int ipow(int base, int power) {
    int num = base;
    for (int i = 0; i < power - 1; i++) {
        num *= base;
    }
    return num;
}

Ray getFirstRay() {
    vec2 uv = (gl_FragCoord.xy - resolution.xy / 2) / resolution.xy;
    vec3 direction = vec3(cameraSize * uv, 1);
    direction.yz = rotate(direction.yz, lookAngle.y);
    direction.xz = rotate(direction.xz, -lookAngle.x);
    return Ray(cameraPosition, normalize(direction));
}

bool isSphereHit(Sphere sphere, Ray ray, out vec3 hitPoint, out float distanceToOrigin,
    out vec3 normal, out vec3 refraction)
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
    distanceToOrigin = thc > tca ? tca + thc : min(tca - thc, tca + thc);
    hitPoint = ray.origin + distanceToOrigin * ray.direction;
    normal = normalize(hitPoint - sphere.position);

    if (dot(ray.direction, normal) < 0) {
        refraction = refract(ray.direction, normal, 1.0 / materials[sphere.material].refractiveIndex);
    } else {
        refraction = refract(ray.direction, -normal, materials[sphere.material].refractiveIndex);
    }

    return true;
}

bool isPlaneHit(Plane plane, Ray ray, out vec3 hitPoint, out float distanceToOrigin,
    out vec3 normal, out vec3 refraction)
{
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

    refraction = ray.direction;

    return true;
}

bool tryGetNearestHit(Ray ray, out vec3 hitPoint, out float distanceToOrigin, out vec3 normal,
    out Material material, out vec3 refraction)
{
    vec3 newHitPoint, newNormal, newRefraction;
    float newDistanceToOrigin;
    bool isObjectHit = false;

    for (int i = 0; i < sphereCount; i++) {
        if (isSphereHit(spheres[i], ray, newHitPoint, newDistanceToOrigin, newNormal, newRefraction)
            && (isObjectHit == false || newDistanceToOrigin < distanceToOrigin))
        {
            distanceToOrigin = newDistanceToOrigin;
            hitPoint = newHitPoint;
            normal = newNormal;
            material = materials[spheres[i].material];
            refraction = newRefraction;
            isObjectHit = true;
        }
    }

    for (int i = 0; i < planeCount; i++) {
        if (isPlaneHit(planes[i], ray, newHitPoint, newDistanceToOrigin, newNormal, newRefraction)
            && (isObjectHit == false || newDistanceToOrigin < distanceToOrigin))
        {
            distanceToOrigin = newDistanceToOrigin;
            hitPoint = newHitPoint;
            normal = newNormal;
            material = materials[planes[i].material];
            refraction = newRefraction;
            isObjectHit = true;
        }
    }

    return isObjectHit;
}

vec3 castRay(Ray ray, out Ray reflectedRay, out Ray refractedRay,
    out float materialReflection, out float materialRefraction)
{
    vec3 hitPoint, newHitPoint, normal, _, refractionDirection;
    float distanceToOrigin, _f, distanceToObstruction;
    Material _m, material;

    if (isSphereHit(Sphere(lightPosition, 0.2, 0), ray, _, distanceToOrigin, _, _)
        && !(tryGetNearestHit(ray, _, distanceToObstruction, _, _m, _) && distanceToObstruction < distanceToOrigin))
    {
        return LIGHT_GIZMO_COLOR;
    }

    if (!tryGetNearestHit(ray, hitPoint, distanceToOrigin, normal, material, refractionDirection)) {
        materialReflection = materialRefraction = 0;
        reflectedRay = refractedRay = ray;
        return BACKGROUND_COLOR;
    }

    vec3 reflectDirection = reflect(ray.direction, normal);
    reflectedRay = Ray(hitPoint + reflectDirection * 0.001, reflectDirection);
    refractedRay = Ray(hitPoint + refractionDirection * 0.001, refractionDirection);
    materialReflection = material.reflection;
    materialRefraction = material.refraction;

    vec3 lightDirection = normalize(hitPoint - lightPosition);
    float distanceToLight = length(lightPosition - hitPoint);
    vec3 lightColor = vec3(1);

    Ray shadowRay = Ray(hitPoint, -lightDirection);
    shadowRay.origin += shadowRay.direction * 0.001;
    float shadowAlignment = dot(normal, shadowRay.direction);
    vec3 ambient = material.ambient * lightColor;

    if (tryGetNearestHit(shadowRay, _, distanceToObstruction, _, _m, _) && distanceToObstruction < distanceToLight
        || shadowAlignment < 0)
    {
        return ambient * material.color;
    }
    float diffuseIntensity = max(shadowAlignment, 0.0);
    vec3 diffuse = material.diffuse * lightColor * diffuseIntensity;

    vec3 specular = vec3(0);
    if (material.shininess > 0) {
        vec3 specularDirection = reflect(lightDirection, normal);
        float specularIntensity = pow(max(dot(-ray.direction, specularDirection), 0.0), material.shininess);
        specular = material.specular * specularIntensity * lightColor;
    }

    return (ambient + diffuse + specular) * material.color;
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
        if (rayStack[i].direction == vec3(-1)) {
            colorStack[i] = vec3(0);
        } else {
            colorStack[i] = castRay(rayStack[i], reflectedRay, refractedRay, reflection, refraction);
            reflectionStack[i] = reflection;
            refractionStack[i] = refraction;
        }

        if (i < continuedRayCount) {
            if (reflection == 0 || rayStack[i].direction == vec3(-1)) {
                reflectedRay.direction = vec3(-1);
            }
            if (refraction == 0 || rayStack[i].direction == vec3(-1)) {
                refractedRay.direction = vec3(-1);
            }
            rayStack[i * 2] = reflectedRay;
            rayStack[i * 2 + 1] = refractedRay;
        }
    }

    for (int i = rayCount - 1; i > 0; i--) {
        if (rayStack[i].direction == vec3(-1)) {
            continue;
        }
        float coeff = i % 2 == 0 ? reflectionStack[i / 2] : refractionStack[i / 2];
        colorStack[i / 2] += colorStack[i] * coeff;
    }
    return colorStack[1];
}

void main() {
    vec3 color = castRecursiveRay(getFirstRay());
    FragColor = vec4(color, 1);
}