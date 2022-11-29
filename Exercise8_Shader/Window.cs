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

        private float horizontalFov = MathF.PI / 2;
        private Vector3 lightPosition;
        private float time;
        private Vector3 cameraPosition = new(0, 1.7f, 0);
        private float moveSpeed = 2;
        private Vector2 lookAngle;
        private Matrix4 LookAt;
        private float mouseSensitivity = 0.0003f;

        private struct Sphere
        {
            public Vector3 position;
            public float radius;
            public int material;
        }
        private Sphere[] spheres = new Sphere[]
        {
            new Sphere() { position=new(-1, 1, 10), radius=0.4f },
            new Sphere() { position=new(1, 1, 15), radius=0.8f },
        };

        private struct Plane
        {
            public Vector3 position;
            public Vector3 normal;
            public Vector3 height;
            public Vector3 width;
            public int material;
        }
        private Plane[] planes = new Plane[]
        {
            new Plane()
            {
                position=new(0, 0, 0),
                normal=new(0, 1, 0),
                height=new(0, 0, 50),
                width=new(50, 0, 0),
                material=2
            },
            new Plane()
            {
                position=new(0, 2, 5),
                normal=new(0, 0, 1),
                height=new(0, 3, 0),
                width=new(3, 0, 0),
                material=1,
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
        };
        private Material[] materials = new Material[]
        {
            new Material() { color=new(1, 0, 0.2f), ambient=0.1f, diffuse=1, specular=0.5f, shininess=32 },
            new Material() { color=new(0.2f, 1f, 0.3f), ambient=0.1f, diffuse=1, specular=0, shininess=8 },
            new Material() { color=new(0.7f, 0.7f, 0.7f), ambient=0.1f, diffuse=1, specular=1, shininess=128 }
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
            GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "lightPosition"),
                ToViewPosition(lightPosition));

            GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "sphereCount"), spheres.Length);
            for (var i = 0; i < spheres.Length; i++)
            {

                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"spheres[{i}].position"),
                    ToViewPosition(spheres[i].position));
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"spheres[{i}].radius"), spheres[i].radius);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"spheres[{i}].material"), spheres[i].material);
            }

            GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "planeCount"), planes.Length);
            for (var i = 0; i < planes.Length; i++)
            {

                var viewOrigin = ToViewPosition(planes[i].position);
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].position"), viewOrigin);
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].normal"),
                    ToRelativeViewPosition(planes[i].normal, planes[i].position, viewOrigin));
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].height"),
                    ToRelativeViewPosition(planes[i].height, planes[i].position, viewOrigin));
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].width"),
                    ToRelativeViewPosition(planes[i].width, planes[i].position, viewOrigin));
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"planes[{i}].material"), planes[i].material);
            }

            for (var i = 0; i < materials.Length; i++)
            {
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].color"), materials[i].color);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].ambient"), materials[i].ambient);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].diffuse"), materials[i].diffuse);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].specular"), materials[i].specular);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].shininess"), materials[i].shininess);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].reflection"), 0);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, $"materials[{i}].refraction"), 0);
            }

            GL.BindVertexArray(fullScreenVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        private Vector3 ToViewPosition(Vector3 worldPosition)
        {
            var viewPosition = (new Vector4(worldPosition, 1) * LookAt).Xyz;
            viewPosition.Xz *= -1;
            return viewPosition;
        }

        private Vector3 ToRelativeViewPosition(Vector3 worldDirection,
            Vector3 worldOrigin, Vector3 viewOrigin)
        {
            var absolutePosition = worldOrigin + worldDirection;
            var viewPosition = (new Vector4(absolutePosition, 1) * LookAt).Xyz;
            viewPosition.Xz *= -1;
            viewPosition -= ToViewPosition(worldOrigin);
            return viewPosition;
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
            var lookTarget = cameraPosition + lookDirection;
            LookAt = Matrix4.LookAt(cameraPosition, lookTarget, Vector3.UnitY);

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
            lightPosition = new Vector3(MathF.Sin(time), 0, MathF.Cos(time));
            lightPosition *= 10;
            lightPosition.Z += 10;
            lightPosition.Y = 1;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
