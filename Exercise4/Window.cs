using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;

namespace Exercise4
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

        private Vector2 currentLocation;
        private Vector2 direction;
        private Vector2 velocity;
        private float acceleration = 5f;
        private float scale = 1;
        private float zoomSpeed = 0.5f;

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

            var translateLocation = GL.GetUniformLocation(_shader.Handle, "translate");
            GL.Uniform2(translateLocation, currentLocation);

            var scaleLocation = GL.GetUniformLocation(_shader.Handle, "scale");
            GL.Uniform1(scaleLocation, scale);

            var resolutionLocation = GL.GetUniformLocation(_shader.Handle, "resolution");
            GL.Uniform2(resolutionLocation, (float)Size.X, (float)Size.Y);

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

            direction = Vector2.Zero;
            if (input.IsKeyDown(Keys.W))
                direction.Y += 1;
            if (input.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (input.IsKeyDown(Keys.S))
                direction.Y -= 1;
            if (input.IsKeyDown(Keys.D))
                direction.X += 1;
            if (direction.X > 0.01 || direction.Y > 0.01)
            {
                direction.Normalize();
            }

            velocity += acceleration * (float)e.Time * direction;
            currentLocation += velocity;

            if (input.IsKeyDown(Keys.K))
                scale *= (1 + zoomSpeed * (float)e.Time);
            if (input.IsKeyDown(Keys.L))
                scale /= (1 + zoomSpeed * (float)e.Time);

            if (scale > 3)
                scale = 1;
            else if (scale < 1)
                scale = 3;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
