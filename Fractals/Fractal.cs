using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{
    public class Fractal
    {
        internal int width, height, highestExposureTarget, highest;
        internal int[] exposure, canvas;
        internal double aspectRatio;
        internal double[] distance;



        //Span<double> numbers = stackalloc double[10];

        internal double[][][] domain;
        internal String name = "Fractal";
        internal String savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Output");
        internal DateTime t0 = DateTime.Now;
        internal TimeSpan TimeStamp { get => DateTime.Now - t0; }

        //public Fractal SmoothStep()
        //{
        //    double[] normalized = Auxiliary.NormalizeArray(exposure, 0, 255);
        //    _ = Parallel.For(0, normalized.Length, i =>
        //    {
        //        double t1 = 0;
        //        double t2 = 255;
        //        double numerator = (normalized[i] - t1);
        //        double denominator = (t2 - t1);
        //        double k = Math.Max(0.0, Math.Min(1.0, numerator / denominator));
        //        int smooth = Auxiliary.Clamp((k * k) * (3 - 2 * k));
        //        int col = 255 << 24 | smooth << 16 | smooth << 8 | smooth << 0;
        //        canvas[i] = col;
        //    });
        //    return this;
        //}'
        public Fractal Init(double x0, double x1, double y0, double y1)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            this.highest = 0;
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

        public virtual Fractal Render() { return this; }
        /***********************************************************************************/
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


        public Fractal Binned()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            HashSet<int> set = new HashSet<int>();
            Hashtable table = new Hashtable();
            //1. Build a hashset of distinct values from the exposure.
            for (int i = 0; i < exposure.Length; i++)
            {
                set.Add(exposure[i]);
            }
            //2. Convert the hashset to an array so that it can be sorted.
            int[] sortedArray = new int[set.Count];
            set.CopyTo(sortedArray);
            //3. Sort the array in ascending order.
            Array.Sort(sortedArray);

            //4. Build a hash table where each unique value corresponds to its position in the sorted array. 
            for (int i = 0; i < sortedArray.Length; i++)
            {
                //Console.WriteLine(sortedArray[i]);
                table.Add(sortedArray[i], (double)i);
            }
            //then....
            for (int i = 0; i < exposure.Length; i++)
            {
                double X = (double)table[exposure[i]];//table look-up
                double A = 0;
                double B = table.Count;
                double C = 0;
                double D = 256;
                int Y = (int)((X - A) / (B - A) * (D - C) + C);

                if (Y < 0 || Y > 255) { throw new Exception(Y + " is too dim or too bright!"); }
                canvas[i] = 255 << 24 | Y << 16 | Y << 8 | Y << 0;
            }
            return this;
        }


        public Fractal Mapped()
        {
            _ = Parallel.For(0, exposure.Length, i =>
            {
                double X = exposure[i];
                double A = 0;
                double B = highest;
                double C = 0;
                double D = 255;
                int Y = (int)((X - A) / (B - A) * (D - C) + C);
                if (Y < 0 || Y > 255) { throw new Exception(Y + " is too dim or too bright!"); }
                canvas[i] = 255 << 24 | Y << 16 | Y << 8 | Y << 0;
            });
            return this;
        }

        public Fractal RampExposure(double howMuch)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            double[] arr = new double[exposure.Length];

            //1. Ramp the brightness
            _ = Parallel.For(0, exposure.Length, i =>
            {
                double ramp = exposure[i] / (highest / howMuch);
                if (ramp > 1)
                {
                    ramp = 1;
                }
                //double ramp = howMuch / exposure[i];
                arr[i] = ramp;
            });

            //2. Convert to RGB
            _ = Parallel.For(0, arr.Length, i =>
             {
                 int r = (int)arr[i] * 255;
                 int g = (int)arr[i] * 255;
                 int b = (int)arr[i] * 255;
                 canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;
             });
            return this;
        }

        //Not particularly useful. 
        public Fractal HexColor()
        {
            _ = Parallel.For(0, exposure.Length, i =>
            {
                string hex = exposure[i].ToString("x8");
                //Console.WriteLine(exposure[i]+" --> "+hex);
                byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
                byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
                byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
                byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
                canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;
            });
            return this;

        }
        public Fractal LogBaseHighest()
        {
            _ = Parallel.For(0, exposure.Length, i =>
            {

                double log = Math.Log(exposure[i], highest);

                int r = (int)Auxiliary.MapDouble(log, 0, 1, 0, 255);
                int g = (int)Auxiliary.MapDouble(log, 0, 1, 0, 255);
                int b = (int)Auxiliary.MapDouble(log, 0, 1, 0, 255);
                canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;

            });
            return this;
        }
        public Fractal LogBaseHighestModified()
        {
            var hsv = new ColorMine.ColorSpaces.Hsv();

            _ = Parallel.For(0, exposure.Length, i =>
            {

                double logA = Math.Log(exposure[i], highest);
                //double logB = Math.Log(distance[i], 360);
                hsv.H = distance[i];
                hsv.S = 1;
                hsv.V = 1;

                var rgb = hsv.ToRgb();
                int r = (int)rgb.R;
                int g = (int)rgb.G;
                int b = (int)rgb.B;

                //int r = (int)Auxiliary.MapDouble(distance[i], 0, 359, 0, 255);
                //int g = (int)Auxiliary.MapDouble(distance[i], 0, 359, 0, 255);
                //int b = (int)Auxiliary.MapDouble(distance[i], 0, 359, 0, 255);

                canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;

                //if (r > 1) { canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0; } 
                //else { canvas[i] = 0 << 24 | 0 << 16 | 0 << 8 | 0 << 0; }


            });
            return this;
        }
        public Fractal SmoothDistance()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            Color colA = Color.FromArgb(0, 0, 0);
            Color colB = Color.FromArgb(255, 255, 255);

            //Normalize between 0 and 1
            double[] temp = Auxiliary.NormalizeArray(distance);

            _ = Parallel.For(0, temp.Length, i =>
              {
                  int r = (int)Auxiliary.MapDouble(temp[i], 0, 1, colA.R, colB.R);
                  int g = (int)Auxiliary.MapDouble(temp[i], 0, 1, colA.G, colB.G);
                  int b = (int)Auxiliary.MapDouble(temp[i], 0, 1, colA.B, colB.B);
                  //if (temp[i] > 1) { Console.WriteLine(r + ", " + g + ", " + b); }
                  canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;
              });
            return this;
        }


        public Fractal HSVtoRGB()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            var hsv = new ColorMine.ColorSpaces.Hsv();

            for (int i = 0; i < distance.Length; i++)
            {

                double x = Auxiliary.MapDouble(exposure[i], 0, highest, 0, 1);
                //double x = Math.Log(exposure[i], highest)*360;

                hsv.H = distance[i];
                //Console.WriteLine(x);
                hsv.S = x;
                hsv.V = 1;

                var rgb = hsv.ToRgb();
                int r = (int)rgb.R;
                int g = (int)rgb.G;
                int b = (int)rgb.B;


                canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;

            }

            return this;

        }

        /***********************************************************************************/
        public Fractal SaveImage()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            GCHandle bitsHandle = GCHandle.Alloc(canvas, GCHandleType.Pinned);
            Bitmap image = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, bitsHandle.AddrOfPinnedObject());
            string image_path = Path.Combine(savePath, name + ".png");
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
            int step = sizeof(double);
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
                Highest = this.highest,
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
                highest = data.Highest;
                height = data.Height;
                width = data.Width;
            }
            return this;
        }
    }
}
