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

            LoadPolyhedrons();

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();
        }

        private void LoadPolyhedrons()
        {
            foreach (var polyhedron in Program.Polyhedrons)
            {
                var newVertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, newVertexBufferObject);

                var vertices = polyhedron.GetVertices();
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                    vertices, BufferUsageHint.StaticDraw);

                polyhedron.VertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(polyhedron.VertexArrayObject);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                var elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, polyhedron.Indices.Length * sizeof(uint),
                    polyhedron.Indices, BufferUsageHint.StaticDraw);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _shader.Use();
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthClamp);
            GL.DepthFunc(DepthFunction.Less);
            GL.DepthMask(true);

            foreach (var polyhedron in Program.Polyhedrons)
            {
                var vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "customColor");
                GL.Uniform4(vertexColorLocation, polyhedron.Color.X, polyhedron.Color.Y, polyhedron.Color.Z, 1.0f);

                var modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
                var model = polyhedron.GetTransform();
                GL.UniformMatrix4(modelLocation, true, ref model);

                var view = Matrix4.LookAt(-10 * Vector3.UnitZ, Vector3.Zero, -Vector3.UnitY);
                var viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
                GL.UniformMatrix4(viewLocation, true, ref view);

                var projection = Matrix4.CreatePerspectiveFieldOfView(
                    MathHelper.DegreesToRadians(45),
                    (float)Size.X / Size.Y, 0.1f, 10000f);
                var projectionLocation = GL.GetUniformLocation(_shader.Handle, "projection");
                GL.UniformMatrix4(projectionLocation, true, ref projection);

                GL.BindVertexArray(polyhedron.VertexArrayObject);
                //GL.DrawArrays(PrimitiveType.Triangles, 0, polyhedron.Points.Length);
                GL.DrawElements(PrimitiveType.Triangles, polyhedron.Indices.Length, DrawElementsType.UnsignedInt, 0);
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
