using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace Exercise8_Shader
{
    public class Scene
    {
        public static Scene Active => scenes[activeIndex];

        private static int activeIndex;
        public static int ActiveIndex
        {
            get => activeIndex;
            set => activeIndex = (value < 0 ? value + scenes.Length : value) % scenes.Length;
        }

        public Sphere[] spheres;
        public Plane[] planes;
        public Material[] materials;
        public Light[] lights;
        public Vector3 backgroundColor;
        public Action<Scene, float> UpdateCallback;

        private static Scene[] scenes = new Scene[]
        {
            // diffuse, reflective and refractive spheres and a mirror
            new Scene()
            {
                spheres = new Sphere[]
                {
                    new Sphere() { position=new(3, 1.3f, 2), radius=0.8f, materialId=3 },
                    new Sphere() { position=new(0, 2, 2), radius=1f, materialId=5 },
                    new Sphere() { position=new(-3, 2, 2), radius=1.2f, materialId=4 },
                    //new Sphere() { position=new(1, 1, 15), radius=0.8f },
                    //new Sphere() { position=new(-2, 2, -5), radius=2f, material=4 },
                },
                planes = new Plane[]
                {
                    //floor
                    new Plane()
                    {
                        position=new(0, 0, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //ceiling
                    new Plane()
                    {
                        position=new(0, 5, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //white wall
                    new Plane()
                    {
                        position=new(0, 0, 5),
                        normal=new(0, 0, -1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //back white wall
                    new Plane()
                    {
                        position=new(0, 0, -5),
                        normal=new(0, 0, 1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //green wall
                    new Plane()
                    {
                        position=new(5, 0, 0),
                        normal=new(-1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=1,
                    },
                    //red wall
                    new Plane()
                    {
                        position=new(-5, 0, 0),
                        normal=new(1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=2,
                    },
                    //mirror
                    new Plane()
                    {
                        position=new(3, 1.5f, 4),
                        normal=new(0, 0, -1),
                        height=new(0, 3, 0),
                        width=new(2, 0, 0),
                        materialId=6,
                    },
                },
                materials = new Material[]
                {
                    //white ceiling and floor
                    new Material() { color=new(1, 1, 1),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                    //green right wall
                    new Material() { color=new(0, 1, 0),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                    //red left wall
                    new Material() { color=new(1, 0, 0),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },

                    new Material() { color=new(0.2f, 0.3f, 1),
                        ambient=0.1f, diffuse=1f, specular=1, shininess=128, reflection = 0.3f },
                    new Material() { color=new(0.7f), ambient = 0.1f,
                        diffuse=0.5f, specular=0.5f, shininess=32,
                        refraction=0.8f, refractiveIndex=1.03f },
                    new Material() { color=new(1, 0.4f, 1f),
                        ambient=0.1f, diffuse=1, specular=0.3f, shininess=4 },
                    new Material() { color=new(0.7f, 0.7f, 0.7f), reflection=1 },
                },
                lights = new Light[] {
                    new Light { color = new Vector3(0.7f) }
                },
                backgroundColor = new Vector3(0.5f),
                UpdateCallback = (scene, time) =>
                {
                    scene.lights[0].position = new Vector3(MathF.Sin(time / 2), 2f, MathF.Cos(time / 2));
                    scene.lights[0].position *= 2;
                }
            },

            // three reflective spheres
            new Scene()
            {
                spheres = new Sphere[]
                {
                    new Sphere() { position=new(2.5f, 1.3f, 3), radius=1, materialId=1 },
                    new Sphere() { position=new(0, 1.3f, 3), radius=1f, materialId=2 },
                    new Sphere() { position=new(-2.5f, 1.3f, 3), radius=1, materialId=3 },
                },
                planes = new Plane[]
                {
                    //floor
                    new Plane()
                    {
                        position=new(0, 0, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 1000),
                        width=new(1000, 0, 0),
                        materialId=0
                    },
                },
                materials = new Material[]
                {
                    new Material() { color=new(0.5f),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32, reflection=0.8f },
                    new Material() { color=new(1, 0, 0),
                        ambient=0.1f, diffuse=1, reflection=1f },
                    new Material() { color=new(0, 1, 0),
                        ambient=0.1f, diffuse=1, reflection=1f },
                    new Material() { color=new(0, 0, 1),
                        ambient=0.1f, diffuse=1, reflection=1f },
                },
                lights = new Light[] {
                    new Light { color = new Vector3(0.6f), position = Vector3.UnitY * 5 }
                },
                backgroundColor = new Vector3(0),
            },

            // one sphere three lights
            new Scene()
            {
                spheres = new Sphere[]
                {
                    new Sphere() { position=new(0, 1.2f, 0), radius=1f, materialId=1 },
                },
                planes = new Plane[]
                {
                    //floor
                    new Plane()
                    {
                        position=new(0, 0, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 1000),
                        width=new(1000, 0, 0),
                        materialId=0
                    },
                },
                materials = new Material[]
                {
                    new Material() { color=new(0.5f),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32, reflection=0.8f },
                    new Material() { color=new(1, 1, 1),
                        ambient=0.1f, diffuse=1, specular=1, shininess = 64 },
                },
                lights = new Light[] {
                    new Light { color = new Vector3(1, 0, 0) },
                    new Light { color = new Vector3(0, 1, 0) },
                    new Light { color = new Vector3(0, 0, 1) }
                },
                backgroundColor = new Vector3(0),
                UpdateCallback = (scene, time) =>
                {
                    time /= 2;
                    scene.lights[0].position = new Vector3(MathF.Sin(time), 2f, MathF.Cos(time)) * 3;
                    time += 2 * MathF.PI / 3;
                    scene.lights[1].position = new Vector3(MathF.Sin(time), 2f, MathF.Cos(time)) * 3;
                    time += 2 * MathF.PI / 3;
                    scene.lights[2].position = new Vector3(MathF.Sin(time), 2f, MathF.Cos(time)) * 3;
                }
            },

            // mirror room
            new Scene()
            {
                spheres = new Sphere[]
                {
                    new Sphere() { position=new(0, 1.2f, 0), radius=1f, materialId=1 },
                },
                planes = new Plane[]
                {
                    //floor
                    new Plane()
                    {
                        position=new(0, 0, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //ceiling
                    new Plane()
                    {
                        position=new(0, 5, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //front wall
                    new Plane()
                    {
                        position=new(0, 0, 5),
                        normal=new(0, 0, -1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //white wall
                    new Plane()
                    {
                        position=new(0, 0, -5),
                        normal=new(0, 0, 1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //right wall
                    new Plane()
                    {
                        position=new(5, 0, 0),
                        normal=new(-1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=0,
                    },
                    //left wall
                    new Plane()
                    {
                        position=new(-5, 0, 0),
                        normal=new(1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=0,
                    },
                },
                materials = new Material[]
                {
                    new Material() { color=new(0), reflection=1 },
                    new Material() { color=new(1, 1, 1),
                        ambient=0.1f, diffuse=1, specular=1, shininess = 64 },
                },
                lights = new Light[]
                {
                    new Light
                    {
                        color = new Vector3(2.0f, 1.6f, 1.0f) / 2,
                        position = Vector3.UnitY * 4,
                    },
                },
                backgroundColor = new Vector3(0),
            },

            // diamond sphere
            new Scene()
            {
                spheres = new Sphere[]
                {
                    new Sphere() { position=new(0, 1.2f, 0), radius=1f, materialId=1 },
                    new Sphere() { position=new(2, 1.2f, 0), radius=0.6f, materialId=2 },
                    new Sphere() { position=new(-2, 1.2f, 0), radius=0.6f, materialId=3 },
                },
                planes = new Plane[]
                {
                    //floor
                    new Plane()
                    {
                        position=new(0, 0, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //ceiling
                    new Plane()
                    {
                        position=new(0, 5, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //front wall
                    new Plane()
                    {
                        position=new(0, 0, 5),
                        normal=new(0, 0, -1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //white wall
                    new Plane()
                    {
                        position=new(0, 0, -5),
                        normal=new(0, 0, 1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //right wall
                    new Plane()
                    {
                        position=new(5, 0, 0),
                        normal=new(-1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=0,
                    },
                    //left wall
                    new Plane()
                    {
                        position=new(-5, 0, 0),
                        normal=new(1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=0,
                    },
                },
                materials = new Material[]
                {
                    new Material() { color=new(1, 1, 1),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                    new Material() { color=new(1, 1, 1),
                        //ambient=0.1f, diffuse=1, specular=1, shininess = 64,
                        reflection = 0.7f, refraction = 0.7f, refractiveIndex = 2.4f },
                    new Material() { color=new(1, 1, 0),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                    new Material() { color=new(0, 1, 1),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                },
                lights = new Light[]
                {
                    new Light
                    {
                        color = new Vector3(2.0f, 1.6f, 1.0f) / 3,
                        position = Vector3.UnitY * 4,
                    },
                },
                backgroundColor = new Vector3(0),
                UpdateCallback = (scene, time) =>
                {
                    scene.lights[0].position = new Vector3(MathF.Sin(time / 2), 2f, MathF.Cos(time / 2));
                    scene.lights[0].position *= 2;
                }
            },

            // reflective planes
            new Scene()
            {
                spheres = new Sphere[]
                {
                    new Sphere() { position=new(0, 1.2f, 3.5f), radius=1f, materialId=1 },
                },
                planes = new Plane[]
                {
                    //floor
                    new Plane()
                    {
                        position=new(0, 0, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //ceiling
                    new Plane()
                    {
                        position=new(0, 5, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 20),
                        width=new(20, 0, 0),
                        materialId=0
                    },
                    //front wall
                    new Plane()
                    {
                        position=new(0, 0, 5),
                        normal=new(0, 0, -1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //white wall
                    new Plane()
                    {
                        position=new(0, 0, -5),
                        normal=new(0, 0, 1),
                        height=new(0, 20, 0),
                        width=new(20, 0, 0),
                        materialId=0,
                    },
                    //right wall
                    new Plane()
                    {
                        position=new(5, 0, 0),
                        normal=new(-1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=0,
                    },
                    //left wall
                    new Plane()
                    {
                        position=new(-5, 0, 0),
                        normal=new(1, 0, 0),
                        height=new(0, 20, 0),
                        width=new(0, 0, 20),
                        materialId=0,
                    },
                    // mirrors
                    new Plane()
                    {
                        position=new(0, 1.5f, 0),
                        normal=new(0, 0, -1),
                        height=new(0, 3, 0),
                        width=new(3, 0, 0),
                        notTiled = 1,
                        materialId=2,
                    },
                    new Plane()
                    {
                        position=new(0, 1.5f, 1),
                        normal=new(0, 0, -1),
                        height=new(0, 3, 0),
                        width=new(3, 0, 0),
                        notTiled = 1,
                        materialId=3,
                    },
                    new Plane()
                    {
                        position=new(0, 1.5f, 2),
                        normal=new(0, 0, -1),
                        height=new(0, 3, 0),
                        width=new(3, 0, 0),
                        notTiled = 1,
                        materialId=4,
                    },
                },
                materials = new Material[]
                {
                    new Material() { color=new(1),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                    new Material() { color=new(1),
                        ambient=0.1f, diffuse=1, specular=1, shininess = 64 },
                    new Material() { color=new(1, 0, 0),
                        ambient=0.1f, diffuse=0.5f, specular=0.5f, shininess=32,
                        //reflection = 0.7f,
                        refraction = 1, refractiveIndex = 1.03f },
                    new Material() { color=new(0, 1, 0),
                        ambient=0.1f, diffuse=0.5f, specular=0.5f, shininess=32,
                        //reflection = 0.7f,
                        refraction = 1, refractiveIndex = 1.03f },
                    new Material() { color=new(0, 0, 1),
                        ambient=0.1f, diffuse=0.5f, specular=0.5f, shininess=32,
                        //reflection = 0.7f,
                        refraction = 1, refractiveIndex = 1.03f },
                },
                lights = new Light[]
                {
                    new Light
                    {
                        color = new Vector3(0.5f),
                        position = Vector3.UnitY * 4,
                    },
                },
                backgroundColor = new Vector3(0),
                //UpdateCallback = (scene, time) =>
                //{
                //    scene.lights[0].position = new Vector3(MathF.Sin(time / 2), 2f, MathF.Cos(time / 2));
                //    scene.lights[0].position *= 2;
                //}
            },

            /*
            new Scene()
            {
                spheres = new Sphere[]
                {
                    new Sphere() { position=new(0, 1.5f, 0), radius=3f, materialId=1 },
                    new Sphere() { position=new(5, 1.2f, 0), radius=0.6f, materialId=2 },
                    new Sphere() { position=new(-5, 1.2f, 0), radius=0.6f, materialId=3 },
                },
                planes = new Plane[]
                {
                    //floor
                    new Plane()
                    {
                        position=new(0, 0, 0),
                        normal=new(0, 1, 0),
                        height=new(0, 0, 1000),
                        width=new(1000, 0, 0),
                        materialId=0
                    },
                },
                materials = new Material[]
                {
                    new Material() { color=new(1, 1, 1),
                        ambient=0.1f, diffuse=0.3f, specular=0.5f, shininess=32 },
                    new Material() { color=new(1, 1, 1),
                        //ambient=0.1f, diffuse=1, specular=1, shininess = 64,
                        refraction = 1, refractiveIndex = 0.5f },
                    new Material() { color=new(1, 0, 0.5f),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                    new Material() { color=new(0.5f, 0, 1),
                        ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
                },
                lights = new Light[]
                {
                    new Light
                    {
                        color = new Vector3(2.0f, 1, 1.6f) / 3,
                        position = Vector3.UnitY * 4,
                    },
                },
                backgroundColor = new Vector3(0.5f),
                UpdateCallback = (scene, time) =>
                {
                    scene.lights[0].position = new Vector3(MathF.Sin(time / 2), 2f, MathF.Cos(time / 2)) * 5;
                }
            },
            */
        };
        private Shader shader;

        public void Update(float time)
        {
            UpdateCallback?.Invoke(this,time);
        }

        public void SendToShader(Shader _shader)
        {
            shader = _shader;

            SetUniform("backgroundColor", backgroundColor);

            SetUniform("lightCount", lights.Length);
            for (var i = 0; i < lights.Length; i++)
            {
                SetUniform($"lights[{i}].position", lights[i].position);
                SetUniform($"lights[{i}].color", lights[i].color);
            }

            SetUniform("sphereCount", spheres.Length);
            for (var i = 0; i < spheres.Length; i++)
            {

                SetUniform($"spheres[{i}].position", spheres[i].position);
                SetUniform($"spheres[{i}].radius", spheres[i].radius);
                SetUniform($"spheres[{i}].materialId", spheres[i].materialId);
            }

            SetUniform("planeCount", planes.Length);
            for (var i = 0; i < planes.Length; i++)
            {

                SetUniform($"planes[{i}].position", planes[i].position);
                SetUniform($"planes[{i}].normal", planes[i].normal);
                SetUniform($"planes[{i}].height", planes[i].height);
                SetUniform($"planes[{i}].width", planes[i].width);
                SetUniform($"planes[{i}].notTiled", planes[i].notTiled);
                SetUniform($"planes[{i}].materialId", planes[i].materialId);
            }

            for (var i = 0; i < materials.Length; i++)
            {
                SetUniform($"materials[{i}].color", materials[i].color);
                SetUniform($"materials[{i}].ambient", materials[i].ambient);
                SetUniform($"materials[{i}].diffuse", materials[i].diffuse);
                SetUniform($"materials[{i}].specular", materials[i].specular);
                SetUniform($"materials[{i}].shininess", materials[i].shininess);
                SetUniform($"materials[{i}].reflection", materials[i].reflection);
                SetUniform($"materials[{i}].refraction", materials[i].refraction);
                SetUniform($"materials[{i}].refractiveIndex", materials[i].refractiveIndex);
            }
        }

        private void SetUniform(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(shader.Handle, name), value);
        }

        private void SetUniform(string name, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(shader.Handle, name), value);
        }

        private void SetUniform(string name, Vector3 value)
        {
            GL.Uniform3(GL.GetUniformLocation(shader.Handle, name), value);
        }
    }

    public struct Sphere
    {
        public Vector3 position;
        public float radius;
        public int materialId;
    }

    public struct Plane
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector3 height;
        public Vector3 width;
        public int notTiled;
        public int materialId;
    }

    public struct Material
    {
        public Vector3 color;
        public float ambient;
        public float diffuse;
        public float specular;
        public float shininess;
        public float reflection;
        public float refraction;
        public float refractiveIndex;
    }

    public struct Light
    {
        public Vector3 position;
        public Vector3 color;
    }
}
