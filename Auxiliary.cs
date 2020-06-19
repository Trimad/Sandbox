using Newtonsoft.Json.Linq;
using Sandbox.Fractals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Sandbox
{
    public class Auxiliary
    {

        public static int Clamp(double d, int low, int high)
        {
            int temp = (int)d;
            if (temp >= high) { return high; }
            else if (temp <= low)
            {
                return low;
            }
            else { return temp; }
        }
        public static double RandomGaussian(double mean, int stdDev)
        {
            Random rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }

        //public static double Random(double step)
        //{
        //    Random rand = new Random();
        //    if (rand.NextDouble() - 0.5 < 0) { return -step; }
        //    else { return step; }
        //}

        public static void Save(int[] pixels, string file)
        {

            byte[] myFinalBytes = new byte[pixels.Length * 4];
            for (int cnt = 0; cnt < pixels.Length; cnt++)
            {
                byte[] myBytes = BitConverter.GetBytes(pixels[cnt]);
                Array.Copy(myBytes, 0, myFinalBytes, cnt * 4, 4);
            }

            File.WriteAllBytes(file, myFinalBytes);
        }
        public static int[] Load(string file)
        {
            byte[] inputElements = File.ReadAllBytes(file);

            int[] myFinalIntegerArray = new int[inputElements.Length / 4];
            for (int cnt = 0; cnt < inputElements.Length; cnt += 4)
            {
                myFinalIntegerArray[cnt / 4] = BitConverter.ToInt32(inputElements, cnt);
            }

            return myFinalIntegerArray;
        }

        public static float MapFloat(float value, float istart, float istop, float ostart, float ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }

        public static double MapDouble(double value, double istart, double istop, double ostart, double ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }
        public static int MapInt(int value, int istart, int istop, int ostart, int ostop)
        {
            return (int)(ostart + ((double)ostop - (double)ostart) * (((double)value - (double)istart) / ((double)istop - (double)istart)));

        }
        public static double CustomLog(double n, double b)
        {
            return (Math.Log(n) / Math.Log(b));
        }

        public static double Lerp(double first, double second, double by)
        {
            return first + (second - first) * by;
        }

        public static double Normalize(int value, int min, int max)
        {
            if (min == 0 || max == 0)
            {
                return value;
            }
            return (value - min) / (max - min);
        }
        public static double Normalize(double value, double min, double max)
        {
            if (min == 0 || max == 0)
            {
                return value;
            }
            return (value - min) / (max - min);
        }
        //INPUT: An array of doubles
        //OUTPUT: An array of doubles normalized between 0 and 1
        public static double[] NormalizeArray(double[] arr)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            double min = int.MaxValue;
            int max = -int.MaxValue;

            _ = Parallel.For(0, arr.Length, i =>
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                    //Console.WriteLine("min: " + min); 
                }
                if (arr[i] > max)
                {
                    max = (int)arr[i];
                    //Console.WriteLine("max: " + max); 
                }
            });
            Console.WriteLine("min: " + min);
            Console.WriteLine("max: " + max);
            double[] norm_arr = new double[arr.Length];
            _ = Parallel.For(0, norm_arr.Length, i =>
            {
                double norm = Auxiliary.MapDouble(arr[i], min, max,0,1);
                norm_arr[i] = norm;

            });
            return norm_arr;
        }

        //INPUT: An array of integers 
        //OUTPUT: An array of doubles normalized between 0 and 1
        public static double[] NormalizeArray(int[] arr)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            double min = int.MaxValue;
            int max = -int.MaxValue;
            _ = Parallel.For(0, arr.Length, i =>
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                    //Console.WriteLine("min: " + min); 
                }
                if (arr[i] > max)
                {
                    max = arr[i];
                    //Console.WriteLine("max: " + max); 
                }
            });
            Console.WriteLine("min: " + min);
            Console.WriteLine("max: " + max);
            double[] norm_arr = new double[arr.Length];
            _ = Parallel.For(0, norm_arr.Length, i =>
            {
                double norm = Auxiliary.Normalize(arr[i], min, max);
                norm_arr[i] = norm;

            });
            return norm_arr;
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            double p1 = x2 - x1;
            double p2 = y2 - y1;
            double hypotenuse = Math.Sqrt((p1 * p1) + (p2 * p2));
            return hypotenuse;
        }

        public static void Merge(int _width, int _height, int[] r, int[] g, int[] b)
        {
            Fractal rgba = new Fractal
            {
                width = _width,
                height = _height,
                canvas = new int[_width * _height],
            };

            for (int i = 0; i < rgba.canvas.Length; i++)
            {
                int col = 255 << 24 | r[i] << 16 | g[i] << 8 | b[i] << 0;
                rgba.canvas[i] = col;
            }
            rgba.SaveImage();
        }
    }
}
