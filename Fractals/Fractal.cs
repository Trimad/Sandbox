using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{
    public class Fractal : Shader
    {
        internal int width, height;
        //internal int[] exposure, canvas;
        internal double aspectRatio;
        internal double[][][] domain;
        internal String name = "Fractal";
        //internal String savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Output");
        internal String savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        internal DateTime t0 = DateTime.Now;
        internal TimeSpan TimeStamp { get => DateTime.Now - t0; }

        public Fractal() { }

        public Fractal Init(double x0, double x1, double y0, double y1)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            this.highestActual = 0;
            this.domain = new double[width][][];
            //The domain is a 3D jagged array that must be initialized
            for (int x = 0; x < width; x++)
            {
                this.domain[x] = new double[height][];
                for (int y = 0; y < height; y++)
                {
                    this.domain[x][y] = new double[2];
                }
            }
            this.exposure = new int[width * height];
            this.distance = new double[width * height];
            this.canvas = new int[width * height];
            this.aspectRatio = (double)width / (double)height;
            //Define the domain with a nested for-loop
            _ = Parallel.For(0, width, x =>
            {
                //double Ax = 0;
                double Bx = width;
                double Cx = x0 * aspectRatio;
                double Dx = x1 * aspectRatio;
                //Map the real plane to the imaginary plane
                double mx = x / (Bx) * (Dx - Cx) + Cx;
                for (int y = 0; y < height; y++)
                {
                    //double Ay = 0;
                    double By = height;
                    double Cy = y0;
                    double Dy = y1;
                    //Map the real plane to the imaginary plane
                    double my = y / (By) * (Dy - Cy) + Cy;
                    domain[x][y][0] = mx;
                    domain[x][y][1] = my;
                }
            });

            return this;
        }

        public Fractal InitPoint(double r0, double i0, double W, double H)
        {

            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            this.highestActual = 0;
            this.domain = new double[width][][];
            //The domain is a 3D jagged array that must be initialized
            for (int x = 0; x < width; x++)
            {
                this.domain[x] = new double[height][];
                for (int y = 0; y < height; y++)
                {
                    this.domain[x][y] = new double[2];
                }
            }
            this.exposure = new int[width * height];
            this.distance = new double[width * height];
            this.canvas = new int[width * height];
            this.aspectRatio = (double)width / (double)height;
            //Map r to x
            double Ax = 0;
            double Bx = width;
            double Cx = (r0 - W) * this.aspectRatio;
            double Dx = (r0 - W) * this.aspectRatio;
            //Map i to y 
            double Ay = 0;
            double By = height;
            double Cy = i0;
            double Dy = i0;
            for (int x = 0; x < width; x++)
            {
                //Map r to x
                double r = ((x - Ax) / (Bx - Ax) * (Dx - Cx) + Cx);
                for (int y = 0; y < height; y++)
                {
                    //Map i to y
                    double i = ((y - Ay) / (By - Ay) * (Dy - Cy) + Cy);
                    domain[x][y][0] = r;
                    domain[x][y][1] = i;
                }
            }

            return this;
        }

        public virtual Fractal Render() { return this; }


        public Fractal TransformExposure()
        {
            _ = Parallel.For(0, exposure.Length, i =>
            {
                double X = exposure[i];
                int Y = (int)Math.Log(X, 255);
                exposure[i] = Y;
            });
            return this;
        }
        public Fractal Draw()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            GCHandle bitsHandle = GCHandle.Alloc(canvas, GCHandleType.Pinned);
            Bitmap image = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, bitsHandle.AddrOfPinnedObject());
            string image_path = Path.Combine(savePath, name + ".png");
            Console.WriteLine(image_path);
            Directory.CreateDirectory(savePath);
            image.Save(image_path, ImageFormat.Png);
            return this;
        }
        public Fractal SaveImage(String s)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            GCHandle bitsHandle = GCHandle.Alloc(canvas, GCHandleType.Pinned);
            Bitmap image = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, bitsHandle.AddrOfPinnedObject());
            string image_path = Path.Combine(savePath, s + ".png");
            Console.WriteLine(image_path);
            image.Save(image_path, ImageFormat.Png);
            return this;
        }
        public Fractal SaveExposure(String name)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            string path = Path.Combine(savePath, name);
            byte[] myFinalBytes = new byte[exposure.Length * 4];
            for (int i = 0; i < exposure.Length; i++)
            {
                byte[] myBytes = BitConverter.GetBytes(exposure[i]);
                Array.Copy(myBytes, 0, myFinalBytes, i * 4, 4);
            }
            File.WriteAllBytes(path, myFinalBytes);
            return this;
        }
        public Fractal LoadExposure(String name)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            string path = Path.Combine(savePath, name);
            byte[] inputElements = File.ReadAllBytes(path);
            exposure = new int[inputElements.Length / 4];
            for (int i = 0; i < inputElements.Length; i += 4)
            {
                exposure[i / 4] = BitConverter.ToInt32(inputElements, i);
            }
            return this;
        }
        public Fractal SaveDistance(String name)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            //int step = sizeof(double);
            string path = Path.Combine(savePath, name);
            byte[] myFinalBytes = new byte[distance.Length * sizeof(double)];
            for (int i = 0; i < distance.Length; i++)
            {
                byte[] myBytes = BitConverter.GetBytes(distance[i]);
                Array.Copy(myBytes, 0, myFinalBytes, i * sizeof(double), sizeof(double));
            }
            File.WriteAllBytes(path, myFinalBytes);
            return this;
        }
        public Fractal LoadDistance(String name)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            string path = Path.Combine(savePath, name);
            byte[] inputElements = File.ReadAllBytes(path);
            distance = new double[inputElements.Length / sizeof(double)];
            for (int i = 0; i < inputElements.Length; i += sizeof(double))
            {

                distance[i / sizeof(double)] = BitConverter.ToDouble(inputElements, i);
            }
            return this;
        }
        public Fractal SaveSettings(String name)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            string path = Path.Combine(savePath, name);
            Settings _data = new Settings()
            {
                Name = this.name,
                AspectRatio = this.aspectRatio,
                Height = this.height,
                Highest = this.highestActual,
                Time = this.TimeStamp,
                Width = this.width
            };

            string json = JsonConvert.SerializeObject(_data);
            File.WriteAllText(path, json);
            return this;
        }
        public Fractal LoadSettings(String name)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            string path = Path.Combine(savePath, name);
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                Settings data = (Settings)serializer.Deserialize(file, typeof(Settings));
                aspectRatio = data.AspectRatio;
                highestActual = data.Highest;
                height = data.Height;
                width = data.Width;
            }
            return this;
        }
    }
}
