using System;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{
    class Transform
    {
        public static int[] Antialias(int[] arr, int width, int height)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            int[] temp = new int[arr.Length];
            int howMuch = 1;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int value = 0;
                    for (int col = Helper.LimitToRange(x - howMuch, 0, width); col < Helper.LimitToRange(x + howMuch, 0, width); col++)
                    {
                        for (int row = Helper.LimitToRange(y - howMuch, 0, height); row < Helper.LimitToRange(y + howMuch, 0, height); row++)
                        {
                            int index = col + row * width;
                            value += arr[index];
                        }
                    }
                    temp[x + y * width] = value;
                }
            }
            return temp;
        }

        public static double[] Sin(int[] arr)
        {
            double[] temp = new double[arr.Length];
            _ = Parallel.For(0, arr.Length, i =>
            {
                temp[i] = Math.Sin(arr[i]);
            });
            return temp;
        }

        public static double[] Sin(double[] arr)
        {
            double[] temp = new double[arr.Length];
            _ = Parallel.For(0, arr.Length, i =>
            {
                temp[i] = Math.Sin(arr[i]);
            });
            return temp;
        }

        public static double[] Log(int[] arr)
        {
            double[] temp = new double[arr.Length];
            _ = Parallel.For(0, arr.Length, i =>
            {
                temp[i] = Math.Log(arr[i]);
            });

            return temp;
        }
        //public static void Cos(int[] arr)
        //{
        //    double[] temp = new double[arr.Length];
        //    _ = Parallel.For(0, arr.Length, i =>
        //    {
        //        temp[i] = Math.Cos(arr[i]);
        //    });
        //}

        //public static void Cos(double[] arr)
        //{
        //    double[] temp = new double[arr.Length];
        //    _ = Parallel.For(0, arr.Length, i =>
        //    {
        //        temp[i] = Math.Cos(arr[i]);
        //    });

        //}
    }
}
