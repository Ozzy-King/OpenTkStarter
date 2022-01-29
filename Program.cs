using System;
using System.IO;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace what
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GameWindowSettings gws = new GameWindowSettings();
            NativeWindowSettings nws = new NativeWindowSettings();

            gws.UpdateFrequency = 60;
            gws.RenderFrequency = 60;
            //gws.IsMultiThreaded = true;


            nws.Size = new Vector2i(800, 600);
            nws.Title = "test0";


            TestGraphic test0 = new TestGraphic(gws, nws);
            test0.Run();
        }
    }


    class TestGraphic : GameWindow
    {

        int VERTEX_BUFFER;
        int VERTEX_ARRAY;
        float[] vertexs = new float[]
        {
            0.0f, 0.5f,
            0.5f, 0.5f,
            0.5f, 0.0f,

            0.0f, 0.5f,
            0.5f, 0.0f,
            0.0f, 0.0f,
        };
        float[] vertexs2 = new float[] {
           -0.5f, 0.0f,
            0.0f, 0.0f,
            0.0f, -0.5f
        };

        int VERT_SHADER;
        int FRAG_SHADER;
        int PROG;

        //for debugging purposes
        void debug(string words = "test test 123")
        {
            Console.WriteLine(words);
        }


        public TestGraphic(GameWindowSettings GWS, NativeWindowSettings NWS) : base(GWS, NWS) { }


        //load frag and vert shaders
        void LoadShaders() {
            string FragShader = File.ReadAllText("./Shader/FragShader.txt");
            string VertShader = File.ReadAllText("./Shader/VertShader.txt");
            FragShader.Trim('\n');
            VertShader.Trim('\n');

            debug(FragShader);
            debug(VertShader);

            GL.ShaderSource(VERT_SHADER, VertShader);
            GL.ShaderSource(FRAG_SHADER, FragShader);

            GL.CompileShader(VERT_SHADER);
            GL.CompileShader(FRAG_SHADER);

            PROG = GL.CreateProgram();
            GL.AttachShader(PROG, VERT_SHADER);
            GL.AttachShader(PROG, FRAG_SHADER);

            GL.LinkProgram(PROG);

            GL.DetachShader(PROG, VERT_SHADER);
            GL.DetachShader(PROG, FRAG_SHADER);
            GL.DeleteShader(VERT_SHADER);
            GL.DeleteShader(FRAG_SHADER);
        }

        //loads vertexs into buffer object
        protected void LoadVertexs(ref float[] vertexs, bool first = false) {

            GL.BindBuffer(BufferTarget.ArrayBuffer, VERTEX_BUFFER);
            GL.BindVertexArray(VERTEX_ARRAY);

            if (first)
            {
                GL.BufferData(BufferTarget.ArrayBuffer, vertexs.Length * sizeof(float), vertexs, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            }
            else {
                IntPtr nun = (IntPtr)0x0;
                GL.BufferSubData<float>(BufferTarget.ArrayBuffer, nun, vertexs.Length * sizeof(float), vertexs);
            }
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        //loads as well as draws vertexs in buffer object
        void DrawVertexs(ref float[] vertexs) {

            LoadVertexs(ref vertexs);

            GL.UseProgram(PROG);
            GL.BindVertexArray(VERTEX_ARRAY);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexs.Length);
            GL.BindVertexArray(0);
        }

        //called once when window first starts
        protected override void OnLoad()
        {
            //seting of clear colour
            GL.ClearColor(new Color4(0f, 0f, 0f, 1f));

            //initilize buffer and array object for later population
            VERTEX_BUFFER = GL.GenBuffer();
            VERTEX_ARRAY = GL.GenVertexArray();

            //inital decluration of data -- allows for a max of 100 vertexs for each object
            float[] empty = new float[100];
            LoadVertexs(ref empty, true);          
            

            //
            //-------------------------shaders----------------------------
            //

            //initlization of vertex and frag shader for later population
            VERT_SHADER = GL.CreateShader(ShaderType.VertexShader);
            FRAG_SHADER = GL.CreateShader(ShaderType.FragmentShader);

            //loads inital shaders for application
            LoadShaders();

            base.OnLoad();
        }

        //call each frame
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawVertexs(ref vertexs);

            DrawVertexs(ref vertexs2);
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        //(assuming) called when fram renders
        protected override void OnUpdateFrame(FrameEventArgs args)
        {


            base.OnUpdateFrame(args);
        }

        //called when window closes
        protected override void OnUnload()
        {
            base.OnUnload();
        }

        //called when window does resize.
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }

    }
}

//comment at the bottom
//another coment
//does this work?
