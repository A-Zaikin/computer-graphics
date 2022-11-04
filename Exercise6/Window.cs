using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Exercise6
{
    public class Window : GameWindow
    {
        private Matrix4 view = Matrix4.LookAt(-10 * Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
        private float verticalFov = MathHelper.DegreesToRadians(45);
        private Matrix4 projection;

        private Shader _shader;
        private PolygonMode currentPolygonMode;

        private Vector3 globalRotation;
        private float rotationSpeed = 1f;
        private bool manualRotation = true;

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
            foreach(var polyhedron in Program.Polyhedrons)
            {
                var newVertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, newVertexBufferObject);

                GL.BufferData(BufferTarget.ArrayBuffer, polyhedron.Vertices.Length * sizeof(float),
                    polyhedron.Vertices, BufferUsageHint.StaticDraw);

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

            var vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "customColor");
            var modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
            var viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
            var projectionLocation = GL.GetUniformLocation(_shader.Handle, "projection");

            var polyhedron = Program.Polyhedrons[Program.CurrentIndex];
            GL.Uniform4(vertexColorLocation, polyhedron.Color.X, polyhedron.Color.Y, polyhedron.Color.Z, 1.0f);

            var model = polyhedron.GetTransform();

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.BindVertexArray(polyhedron.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, polyhedron.Indices.Length, DrawElementsType.UnsignedInt, 0);

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

            if (input.IsKeyPressed(Keys.P))
            {
                currentPolygonMode = currentPolygonMode switch
                {
                    PolygonMode.Fill => PolygonMode.Line,
                    _ => PolygonMode.Fill,
                };
                GL.PolygonMode(MaterialFace.FrontAndBack, currentPolygonMode);
            }

            if (input.IsKeyPressed(Keys.Right))
            {
                Program.CurrentIndex += 1;
                Program.Polyhedrons[Program.CurrentIndex].Rotation = Vector3.Zero;
            }
            if (input.IsKeyPressed(Keys.Left))
            {
                Program.CurrentIndex -= 1;
                Program.Polyhedrons[Program.CurrentIndex].Rotation = Vector3.Zero;
            }

            if (input.IsKeyPressed(Keys.Space))
            {
                manualRotation = !manualRotation;
            }

            globalRotation = Vector3.Zero;
            if (input.IsKeyDown(Keys.W))
                globalRotation.X = rotationSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.S))
                globalRotation.X = -rotationSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.A))
                globalRotation.Y = -rotationSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.D))
                globalRotation.Y = rotationSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.Q))
                globalRotation.Z = -rotationSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.E))
                globalRotation.Z = rotationSpeed * (float)e.Time;

            var polyhedron = Program.Polyhedrons[Program.CurrentIndex];
            if (manualRotation)
            {
                polyhedron.Rotation += globalRotation;
            }
            else
            {
                //polyhedron.Update();
                polyhedron.Rotation += new Vector3(0.6f, 0.46f, 0.1f) * (float)e.Time;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);

            projection = Matrix4.CreatePerspectiveFieldOfView(
                verticalFov, (float)Size.X / Size.Y, 0.1f, 10000f);
        }
    }
}
