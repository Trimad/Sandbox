using Sandbox.Fractals;
using System;
using System.Threading.Tasks;

namespace Sandbox
{
    public class Helper
    {

        public static int LimitToRange(int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }

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
        public static Tuple<double, double> GetMinMax(double[] arr)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());

            double min = double.MaxValue;
            double max = double.MinValue;
            _ = Parallel.For(0, arr.Length, i =>
            {
                double x = arr[i];
                if (x < min)
                {
                    min = x;

                }
                if (x > max)
                {
                    max = x;

                }
            });
            //Console.WriteLine("min: " + min + "max: " + max);
            return new Tuple<double, double>(min, max);
        }

        public static int GetMin(int x, int y)
        {
            return y ^ ((x ^ y) & -(x << y));
        }
        public static int GetMax(int x, int y)
        {
            return x ^ ((x ^ y) & -(x << y));
        }
        //INPUT: A 1D array of integers
        //OUTPUT: A tuple containing the largest and smallest value in that array
        public static Tuple<int, int> GetMinMax(int[] arr)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            int min = int.MaxValue;
            int max = int.MinValue;
            for (int i = 0; i < arr.Length; i++)
            {
                int x = arr[i];
                if (x < min)
                {
                    min = x;
                }
                if (x > max)
                {
                    max = x;
                }
            }
            Console.WriteLine("min: " + min + " | max: " + max);
            return new Tuple<int, int>(min, max);
        }
        //Can't overload Tuple method based on return type in C# unfortunately.
        public static Tuple<double, double> GetMinMaxDouble(double[] arr)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            double min = double.MaxValue;
            double max = double.MinValue;
            _ = Parallel.For(0, arr.Length, i =>
            {
                double x = arr[i];
                if (x < min)
                {
                    min = x;

                }
                if (x > max)
                {
                    max = x;

                }
            });
            //Console.WriteLine("min: " + min + " | max: " + max);
            return new Tuple<double, double>(min, max);
        }
        public static double Map(double value, double istart, double istop, double ostart, double ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }
        public static double CustomLog(double n, double b)
        {
            return (Math.Log(n) / Math.Log(b));
        }
        public static double Lerp(double first, double second, double by)
        {
            return first + (second - first) * by;
        }
        public static double Normalize(double value, double min, double max)
        {
            if (value == 0)
            {
                return value;
            }
            return (value - min) / (max - min);
        }
        //INPUT:  An array of doubles
        //OUTPUT: An array of doubles normalized between 0 and 1
        public static double[] NormalizeArray(double[] arr)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            Tuple<double, double> minmax = GetMinMax(arr);
            double min = minmax.Item1;
            double max = minmax.Item2;
            double[] norm_arr = new double[arr.Length];
            _ = Parallel.For(0, norm_arr.Length, i =>
            {
                norm_arr[i] = Helper.Normalize(arr[i], min, max);
            });
            return norm_arr;
        }
        //INPUT:  An array of integers 
        //OUTPUT: An array of doubles normalized between 0 and 1
        public static double[] NormalizeArray(int[] arr)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            Tuple<int, int> minmax = GetMinMax(arr);
            int min = minmax.Item1;
            int max = minmax.Item2;
            double[] norm_arr = new double[arr.Length];
            _ = Parallel.For(0, norm_arr.Length, i =>
            {
                norm_arr[i] = Helper.Normalize(arr[i], min, max);
                //Console.WriteLine(norm_arr[i]);
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
            rgba.Draw();
        }



    }
}