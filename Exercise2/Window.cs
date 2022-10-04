using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;

namespace Exercise2
{
    public class Window : GameWindow
    {
        private Shader _shader;

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
            foreach (var polygon in Program.Polygons)
            {
                var newVertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, newVertexBufferObject);

                var vertices = polygon.GetVertices();
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                    vertices, BufferUsageHint.StreamDraw);

                polygon.VertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(polygon.VertexArrayObject);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            foreach (var polygon in Program.Polygons)
            {
                var vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "customColor");
                GL.Uniform4(vertexColorLocation, polygon.Color.X, polygon.Color.Y, polygon.Color.Z, 1.0f);

                //var transformUniformLocation = GL.GetUniformLocation(_shader.Handle, "transform");
                //var transform = polygon.Scale;
                //GL.UniformMatrix4(transformUniformLocation, true, ref transform);

                GL.BindVertexArray(polygon.VertexArrayObject);
                GL.DrawArrays(PrimitiveType.TriangleFan, 0, polygon.VertexCount);
            }

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
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
