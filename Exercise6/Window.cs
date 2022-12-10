using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using System.Linq;
using StbImageSharp;
using System.IO;
using System.Collections.Generic;

namespace Exercise6
{
    public class Window : GameWindow
    {
        private static Vector3 cameraPos = 10 * Vector3.UnitZ;
        private static Matrix4 view = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY);
        private List<int> textureIds = new();

        private float verticalFov = MathHelper.DegreesToRadians(45);
        private Matrix4 projection;

        private Shader _shader;
        private PolygonMode currentPolygonMode;

        private Vector3 globalRotation;
        private float rotationSpeed = 1f;
        private bool manualRotation = true;

        private bool flatLighting = false;
        private float cameraZoomSpeed = 3;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            LoadTexture("metal2.png");
            LoadTexture("paint.png");
            LoadTexture("metal5.png");
            LoadTexture("neon2.png");
            LoadTexture("rubber2.png");
            LoadTexture("pipe.png");

            LoadPolyhedrons();

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();
        }

        private void LoadTexture(string path)
        {
            var handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, handle);

            StbImage.stbi_set_flip_vertically_on_load(1);
            var image = ImageResult.FromStream(File.OpenRead("Textures/" + path), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 0);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            textureIds.Add(handle);
        }

        private void LoadPolyhedrons()
        {
            foreach (var shape in Program.Polyhedrons)
            {
                foreach (var polyhedron in shape)
                {
                    var newVertexBufferObject = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, newVertexBufferObject);

                    GL.BufferData(BufferTarget.ArrayBuffer, polyhedron.BufferData.Length * sizeof(float),
                        polyhedron.BufferData, BufferUsageHint.StaticDraw);

                    polyhedron.VertexArrayObject = GL.GenVertexArray();
                    GL.BindVertexArray(polyhedron.VertexArrayObject);

                    GL.EnableVertexAttribArray(0);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

                    GL.EnableVertexAttribArray(1);
                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false,
                        8 * sizeof(float), 3 * sizeof(float));

                    GL.EnableVertexAttribArray(2);
                    GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false,
                        8 * sizeof(float), 6 * sizeof(float));

                    var elementBufferObject = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, polyhedron.Indices.Length * sizeof(uint),
                        polyhedron.Indices, BufferUsageHint.StaticDraw);
                }
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

            foreach(var polyhedron in Program.Polyhedrons[Program.CurrentIndex])
            {
                if (polyhedron.TextureIndex != -1)
                {
                    GL.BindTexture(TextureTarget.Texture2D, textureIds[polyhedron.TextureIndex]);
                }

                var model = polyhedron.GetTransform();

                if (Program.CurrentIndex == Program.Polyhedrons.Count - 1)
                {
                    GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "generateColors"), 0);
                    GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "objectColor"), polyhedron.Material.Color);
                    GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "objectSpecularStrength"),
                        polyhedron.Material.SpecularStrength);
                    GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "objectDiffuseStrength"),
                        polyhedron.Material.Color.Length);

                    var isObjectTextured = polyhedron.TextureIndex == -1 ? 0 : 1;
                    GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "isObjectTextured"), isObjectTextured);
                }
                else
                {
                    GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "generateColors"), 1);
                    GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "isObjectTextured"), 0);
                }

                GL.UniformMatrix4(GL.GetUniformLocation(_shader.Handle, "model"), true, ref model);
                GL.UniformMatrix4(GL.GetUniformLocation(_shader.Handle, "view"), true, ref view);
                GL.UniformMatrix4(GL.GetUniformLocation(_shader.Handle, "projection"), true, ref projection);

                var worldRotation = polyhedron.GetWorldRotation();
                GL.UniformMatrix4(GL.GetUniformLocation(_shader.Handle, "worldRotation"), true, ref worldRotation);

                var lighting = !polyhedron.IsRound || flatLighting;
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "flatMode"), lighting ? 1 : 0);
                GL.Uniform1(GL.GetUniformLocation(_shader.Handle, "polyMode"),
                    currentPolygonMode == PolygonMode.Line ? 0 : 1);
                GL.Uniform3(GL.GetUniformLocation(_shader.Handle, "cameraPos"), cameraPos);

                GL.BindVertexArray(polyhedron.VertexArrayObject);
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

            if (input.IsKeyPressed(Keys.R))
            {
                currentPolygonMode = currentPolygonMode switch
                {
                    PolygonMode.Line => PolygonMode.Fill,
                    _ => PolygonMode.Line,
                };
                GL.PolygonMode(MaterialFace.FrontAndBack, currentPolygonMode);
            }

            if (input.IsKeyPressed(Keys.F))
            {
                flatLighting = !flatLighting;
            }

            if (input.IsKeyPressed(Keys.Left) || input.IsKeyPressed(Keys.Right))
            {
                //foreach (var shape in Program.Polyhedrons[Program.CurrentIndex])
                //{
                //    shape.Rotation = Vector3.Zero;
                //}
                flatLighting = false;
            }
            if (input.IsKeyPressed(Keys.Right))
            {
                Program.CurrentIndex += 1;
            }
            if (input.IsKeyPressed(Keys.Left))
            {
                Program.CurrentIndex -= 1;
            }

            if (input.IsKeyPressed(Keys.Space))
            {
                manualRotation = !manualRotation;
            }

            if (input.IsKeyDown(Keys.Z))
            {
                cameraPos.Z -= cameraZoomSpeed * (float)e.Time;
                view = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY);
            }
            if (input.IsKeyDown(Keys.X))
            {
                cameraPos.Z += cameraZoomSpeed * (float)e.Time;
                view = Matrix4.LookAt(cameraPos, Vector3.Zero, Vector3.UnitY);
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
                globalRotation.Z = rotationSpeed * (float)e.Time;
            if (input.IsKeyDown(Keys.E))
                globalRotation.Z = -rotationSpeed * (float)e.Time;

            foreach (var shape in Program.Polyhedrons[Program.CurrentIndex])
            {
                if (manualRotation)
                {
                    shape.WorldRotation += globalRotation;
                }
                else
                {
                    //polyhedron.Update();
                    shape.WorldRotation += new Vector3(0.6f, 0.46f, 0.1f) * (float)e.Time;
                }
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
