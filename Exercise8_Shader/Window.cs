using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;

namespace Exercise8_Shader
{
    public class Window : GameWindow
    {
        private Shader _shader;
        private float[] fullScreenVertices = new float[]
        {
            -1, -1, 0,
            3, -1, 0,
            -1, 3, 0,
        };
        private int fullScreenVao;

        private float horizontalFov = MathHelper.DegreesToRadians(140f);
        private Vector3 lightPosition;
        private float time;
        private Vector3 cameraPosition = new(0, 1.7f, -3);
        private float moveSpeed = 2;
        private Vector2 lookAngle;
        private float mouseSensitivity = 0.0003f;
        private int maxDepth = 4;

        private struct Sphere
        {
            public Vector3 position;
            public float radius;
            public int materialId;
        }
        private Sphere[] spheres = new Sphere[]
        {
            new Sphere() { position=new(3, 1.3f, 2), radius=0.8f, materialId=3 },
            new Sphere() { position=new(0, 2, 2), radius=1f, materialId=5 },
            new Sphere() { position=new(-3, 2, 2), radius=1.2f, materialId=4 },
            //new Sphere() { position=new(1, 1, 15), radius=0.8f },
            //new Sphere() { position=new(-2, 2, -5), radius=2f, material=4 },
        };

        private struct Plane
        {
            public Vector3 position;
            public Vector3 normal;
            public Vector3 height;
            public Vector3 width;
            public int materialId;
        }
        private Plane[] planes = new Plane[]
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
        };

        private struct Material
        {
            public Vector3 color;
            public float ambient;
            public float diffuse;
            public float specular;
            public float shininess;
            public float reflection;
            public float refraction;
            public float refractiveIndex;
        };
        private Material[] materials = new Material[]
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
            //new Material() { color=new(0.7f, 0.7f, 0.7f),
            //    ambient=0.1f, diffuse=0.5f, specular=0.5f, shininess=4, refraction=0.8f, refractiveIndex=1.03f },
            new Material() { color=new(0.7f), ambient = 0.1f,
                diffuse=0.5f, specular=0.5f, shininess=32,
                refraction=0.8f, refractiveIndex=1.03f },
            new Material() { color=new(1, 0.4f, 1f),
                ambient=0.1f, diffuse=1, specular=0.3f, shininess=4 },
            new Material() { color=new(0.7f, 0.7f, 0.7f), reflection=1 },
        };

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            LoadPolygons();

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "sphereCount"), spheres.Length);
            for (var i = 0; i < spheres.Length; i++)
            {

                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"spheres[{i}].position"), spheres[i].position);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"spheres[{i}].radius"), spheres[i].radius);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"spheres[{i}].materialId"), spheres[i].materialId);
            }

            GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "planeCount"), planes.Length);
            for (var i = 0; i < planes.Length; i++)
            {

                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].position"), planes[i].position);
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].normal"), planes[i].normal);
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].height"), planes[i].height);
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].width"), planes[i].width);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].materialId"), planes[i].materialId);
            }

            for (var i = 0; i < materials.Length; i++)
            {
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].color"), materials[i].color);
                SetUniform($"materials[{i}].ambient", materials[i].ambient);
                SetUniform($"materials[{i}].diffuse", materials[i].diffuse);
                SetUniform($"materials[{i}].specular", materials[i].specular);
                SetUniform($"materials[{i}].shininess", materials[i].shininess);
                SetUniform($"materials[{i}].reflection", materials[i].reflection);
                SetUniform($"materials[{i}].refraction", materials[i].refraction);
                SetUniform($"materials[{i}].refractiveIndex", materials[i].refractiveIndex);
            }
        }

        private void LoadPolygons()
        {
            var newVertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, newVertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, fullScreenVertices.Length * sizeof(float),
                fullScreenVertices, BufferUsageHint.StreamDraw);

            fullScreenVao = GL.GenVertexArray();
            GL.BindVertexArray(fullScreenVao);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            var cameraX = MathF.Tan(horizontalFov / 2);
            var cameraSize = new Vector2(cameraX, cameraX / Size.X * Size.Y);

            GL.Uniform2(GL.GetUniformLocation(_shader.Handle, "resolution"), Size);
            GL.Uniform2(GL.GetUniformLocation(_shader.Handle, "cameraSize"), cameraSize);
            GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "lightPosition"), lightPosition);
            GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "maxDepth"), maxDepth);
            GL.Uniform2(GL.GetUniformLocation(_shader.Handle, "lookAngle"), lookAngle);
            GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "cameraPosition"), cameraPosition);

            GL.BindVertexArray(fullScreenVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        private void SetUniform(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(_shader.Handle, name), value);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            lookAngle += MouseState.Delta * mouseSensitivity;
            lookAngle.Y = MathHelper.Clamp(lookAngle.Y, -MathF.PI / 2 * 0.95f, MathF.PI / 2 * 0.95f);
            var lookDirection = Vector3.UnitZ;
            lookDirection.Yz = lookDirection.Yz.Rotate(lookAngle.Y);
            lookDirection.Xz = lookDirection.Xz.Rotate(-lookAngle.X);

            Vector2 movementDirection = new();
            if (input.IsKeyDown(Keys.W))
            {
                movementDirection += lookDirection.Xz;
            }
            if (input.IsKeyDown(Keys.S))
            {
                movementDirection += -lookDirection.Xz;
            }
            if (input.IsKeyDown(Keys.A))
            {
                movementDirection += lookDirection.Xz.PerpendicularLeft;
            }
            if (input.IsKeyDown(Keys.D))
            {
                movementDirection += lookDirection.Xz.PerpendicularRight;
            }
            var actualMoveSpeed = input.IsKeyDown(Keys.LeftShift) ? moveSpeed * 3 : moveSpeed;
            cameraPosition.Xz += movementDirection * actualMoveSpeed * (float)e.Time;

            time += (float)e.Time;
            lightPosition = new Vector3(MathF.Sin(time/2), 0, MathF.Cos(time/2));
            lightPosition *= 2;
            lightPosition.Y = 4f;

            if (input.IsKeyPressed(Keys.R) && maxDepth > 1)
            {
                maxDepth -= 1;
            }
            if (input.IsKeyPressed(Keys.T) && maxDepth < 7)
            {
                maxDepth += 1;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
