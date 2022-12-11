using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using Microsoft.VisualBasic;

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
        private float levitationSpeed = 2;
        private Vector2 lookAngle;
        private float mouseSensitivity = 0.0003f;
        private int maxDepth = 4;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            LoadPolygons();
            Scene.Active.Update(0);

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
            GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "maxDepth"), maxDepth);
            GL.Uniform2(GL.GetUniformLocation(_shader.Handle, "lookAngle"), lookAngle);
            GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "cameraPosition"), cameraPosition);

            Scene.Active.SendToShader(_shader);

            GL.BindVertexArray(fullScreenVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyPressed(Keys.Left) || input.IsKeyPressed(Keys.Right))
            {
                SetDefaultCamera();
            }
            if (input.IsKeyPressed(Keys.Right))
            {
                Scene.ActiveIndex++;
            }
            if (input.IsKeyPressed(Keys.Left))
            {
                Scene.ActiveIndex--;
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
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
                var actualMoveSpeed = input.IsKeyDown(Keys.LeftShift) ? moveSpeed * 3 : moveSpeed;
                cameraPosition.Xz += movementDirection * actualMoveSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Keys.Space))
            {
                cameraPosition.Y += levitationSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.LeftControl))
            {
                cameraPosition.Y -= levitationSpeed * (float)e.Time;
            }

            time += (float)e.Time;
            Scene.Active.Update(time);

            if (input.IsKeyPressed(Keys.R) && maxDepth > 1)
            {
                maxDepth -= 1;
            }
            if (input.IsKeyPressed(Keys.T) && maxDepth < 8)
            {
                maxDepth += 1;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        private void SetDefaultCamera()
        {
            cameraPosition = new(0, 1.7f, -3);
            lookAngle = new();
            time = 0;
        }
    }
}
