using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;

namespace Exercise6
{
    public class Window : GameWindow
    {
        private Shader _shader;
        private PolygonMode currentPolygonMode;

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
            foreach (var shape in Program.Shapes)
            {
                var newVertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, newVertexBufferObject);

                var vertices = shape.GetVertices();
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                    vertices, BufferUsageHint.StreamDraw);

                shape.VertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(shape.VertexArrayObject);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();

            foreach (var polygon in Program.Shapes)
            {
                var vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "customColor");
                GL.Uniform4(vertexColorLocation, polygon.Color.X, polygon.Color.Y, polygon.Color.Z, 1.0f);

                var transformUniformLocation = GL.GetUniformLocation(_shader.Handle, "transform");
                var transform = polygon.GetTransform();
                transform *= Matrix4.CreateScale(1f / Size.X, 1f / Size.Y, 1f / Math.Min(Size.X, Size.Y));

                //transform *= Matrix4.CreatePerspectiveFieldOfView(
                //    MathHelper.DegreesToRadians(45),
                //    (float)Size.X / Size.Y, 0.1f, 100);

                //transform *= Matrix4.LookAt(new Vector3(0, 0, -10), Vector3.One, new Vector3(0, 1, 0));

                GL.UniformMatrix4(transformUniformLocation, true, ref transform);

                GL.BindVertexArray(polygon.VertexArrayObject);
                GL.DrawArrays(PrimitiveType.TriangleFan, 0, polygon.Points.Length);
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
            else if (input.IsKeyPressed(Keys.P))
            {
                currentPolygonMode = currentPolygonMode switch
                {
                    PolygonMode.Fill => PolygonMode.Line,
                    PolygonMode.Line => PolygonMode.Point,
                    _ => PolygonMode.Fill,
                };
                GL.PolygonMode(MaterialFace.FrontAndBack, currentPolygonMode);
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
