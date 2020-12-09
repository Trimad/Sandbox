using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{

    public class Shader
    {
        internal int[] exposure, canvas;
        internal int highestActual, highestExposureTarget;
        internal double[] distance;

        public Shader DistanceBinned()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            HashSet<double> set = new HashSet<double>();
            Hashtable table = new Hashtable();
            //1. Build a hashset of distinct values from the exposure.
            for (int i = 0; i < distance.Length; i++)
            {
                set.Add(distance[i]);
            }
            //2. Convert the hashset to an array so that it can be sorted.
            double[] sortedArray = new double[set.Count];
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
            for (int i = 0; i < distance.Length; i++)
            {
                double X = (double)table[distance[i]];//table look-up
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

        public Shader DistanceHSV()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            var hsv = new ColorMine.ColorSpaces.Hsv();
            double[] transform;
            //transform= Transform.Sin(distance);
            transform = Helper.NormalizeArray(distance);

            for (int i = transform.Length - 1; i > 0; i--)
            {
                //double temp = (!double.IsNaN(distance[i]) ? distance[i]: 0 ); //broken
                //double x = (temp != 0) ? distance[i] * 360 : 0; //broken

                double x = transform[i] * 360;
                hsv.H = x;
                hsv.S = 1;
                hsv.V = 1;

                var rgb = hsv.ToRgb();
                int r = (int)rgb.R;
                int g = (int)rgb.G;
                int b = (int)rgb.B;

                canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;

            }
            return this;
        }


        //Distance Algorithm
        public Shader DistanceMapped()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            Color colA = Color.FromArgb(0, 0, 0);
            Color colB = Color.FromArgb(255, 255, 255);

            //Normalize between 0 and 1
            double[] temp;
            // temp = Transform.Sin(distance);
            temp = Helper.NormalizeArray(distance);
            _ = Parallel.For(0, temp.Length, i =>
            {
                int r = (int)Helper.Map(temp[i], 0, 1, colA.R, colB.R);
                int g = (int)Helper.Map(temp[i], 0, 1, colA.G, colB.G);
                int b = (int)Helper.Map(temp[i], 0, 1, colA.B, colB.B);
                //if (temp[i] > 1) { Console.WriteLine(r + ", " + g + ", " + b); }
                canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;
            });
            return this;
        }

        public Shader Exponential(double exponent)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            double[] normalized = Helper.NormalizeArray(distance);
            _ = Parallel.For(0, distance.Length, i =>
            {
                double X = normalized[i];
                int Y = (int)(Math.Pow(X, exponent) * 255);
                if (Y < 0 || Y > 255) { throw new Exception(Y + " is too dim or too bright!"); }
                canvas[i] = 255 << 24 | Y << 16 | Y << 8 | Y << 0;
            });
            return this;
        }

        public Shader ExposureBinned()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            highestActual = Helper.GetMinMax(exposure).Item2;
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

        public Shader ExposureHSV()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            highestActual = Helper.GetMinMax(exposure).Item2;
            var hsv = new ColorMine.ColorSpaces.Hsv();
            Helper.NormalizeArray(exposure);
            for (int i = exposure.Length - 1; i > 0; i--)
            {
                double x = (exposure[i] != 0) ? exposure[i] * 360 : 0;
                hsv.H = x;
                hsv.S = 1;
                hsv.V = 1;

                var rgb = hsv.ToRgb();
                int r = (int)rgb.R;
                int g = (int)rgb.G;
                int b = (int)rgb.B;

                canvas[i] = 255 << 24 | r << 16 | g << 8 | b << 0;

            }
            return this;
        }

        public Shader Mapped()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            
            highestActual = Helper.GetMinMax(exposure).Item2;

            _ = Parallel.For(0, exposure.Length, i =>
            {

                double X = exposure[i];
                double A = 0;
                double B = highestActual;
                double C = 0;
                double D = 255;
                int Y = (int)((X - A) / (B - A) * (D - C) + C);
                if (Y < 0 || Y > 255){throw new Exception(Y + " is too dim or too bright!");}

                canvas[i] = 255 << 24 | Y << 16 | Y << 8 | Y << 0;


            });
            return this;
        }

        //Not particularly useful. 
        public Shader HexColor()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
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
        public Shader LogBaseHighest()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            //exposure = Transform.Antialias(exposure, 3840, 2160);
            highestActual = Helper.GetMinMax(exposure).Item2;
            _ = Parallel.For(0, exposure.Length, i =>
            {

                double log = Math.Log(exposure[i], highestActual);

                int a = (int)Helper.Map(log, 0d, 1d, 0d, 255d);
                //int r = (int)Auxiliary.Map(log, 0d, 1d, 0d, 255d);
                //int g = (int)Auxiliary.Map(log, 0d, 1d, 0d, 255d);
                //int b = (int)Auxiliary.Map(log, 0d, 1d, 0d, 255d);

                canvas[i] = 255 << 24 | a << 16 | a << 8 | a << 0;

            });
            return this;
        }


        //Exposure Algorithm
        public Shader SmoothStep()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            double[] normalized = Helper.NormalizeArray(exposure);
            _ = Parallel.For(0, normalized.Length, i =>
            {
                double t1 = 0;
                double t2 = 1;
                double numerator = normalized[i] - t1;
                double denominator = t2 - t1;
                double k = Math.Max(0, Math.Min(1, numerator / denominator));
                double smooth = (k * k) * (3 - (2 * k));
                double func = Math.Sin(smooth * 1.5708 * 5);

                if (smooth > 1 || smooth < 0)
                {
                    canvas[i] = 255 << 24 | 255 << 16 | 255 << 8 | 255 << 0;
                }
                else
                {
                    canvas[i] = 255 << 24 | (int)(smooth * 255) << 16 | (int)(smooth * 255) << 8 | (int)(smooth * 255) << 0;
                }

            });

            return this;
        }




    }

}
