//https://www.youtube.com/watch?v=8RcjQPbvvRU
//https://www.youtube.com/watch?v=8RcjQPbvvRU
//https://www.youtube.com/watch?v=8RcjQPbvvRU

using Sandbox.Fractals;
using System;

namespace Sandbox
{
    class Program
    {
        private static readonly int W = 2048;
        private static readonly int H = 2048;
        //private static readonly int W = 13500;
        //private static readonly int H = 13500;
        //private static readonly int HIGHEST = int.MaxValue;
        private static readonly int HIGHEST = 2048;
        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(" ", args));
            Console.WriteLine("Environment Version: {0}", Environment.Version.ToString());

            Fractal fractal = new Fractal();

            foreach (string a in args)
            {
                switch (a)
                {
                    case "animate":
                        int upper = 900;
                        for (int i = 0; i < upper; i++)
                        {
                            double zoom = Helper.Map(i, 0, upper, 1, 5);
                            //centered on (-0.54,0.0)
                            //fractal = new Glynn(W, H, i,0,0).Init(-0.79 + zoom, -0.29 - zoom, -0.25 + zoom, 0.25 - zoom);//focuses on the top tree
                            //fractal = new Glynn(W, H, HIGHEST, zoom, 1).Init(-0.8, 1.4, -1.2, 1.2);//focuses on all the trees
                            double x0 = Helper.Map(i, 0, upper, -2, 1.225);
                            double x1 = Helper.Map(i, 0, upper, 2, 1.325);
                            double y0 = Helper.Map(i, 0, upper, -2, -0.05);
                            double y1 = Helper.Map(i, 0, upper, 2, 0.05);

                            fractal = new Glynn(W, H, HIGHEST, zoom, 1).Init(x0, x1, y0, y1);
                            fractal.Render().DistanceMapped();
                            //DistanceHSV();
                            fractal.SaveImage(i.ToString("D5"));

                        }
                        break;
                    case "buddhabrot":
                        fractal = new Buddhabrot(W, H, 0, 1000, HIGHEST)
                            //.Init(-2, 2, -2, 2);
                            .Init(-1.9, 1.9, -2.1, 1.7);
                        break;

                    case "glynn":
                        fractal = new Glynn(W, H, HIGHEST, 0, 0)
                        //.Init(0.066, 0.42, -0.6677, -0.323);
                        .Init(1, -1, -1, 1);
                        //.InitPoint(-0.539, 0, 0.2, 0.2);//r, i, width, height
                        break;
                    case "julia":
                        fractal = new Julia(W, H, HIGHEST)
                        //.Init(-0.39, -0.7, 0.19, -0.12);
                        .Init(-0.8, 0.8, -0.8, 0.8);
                        break;
                    case "load":
                        fractal.LoadSettings("settings.json").LoadDistance("distance.dat").LoadExposure("exposure.dat");
                        break;
                    case "logisticmap":
                        fractal = new LogisticMap(W, H, HIGHEST)
                            .Init(0, 0, 0, 0);
                        break;
                    case "mandelbrot":
                        fractal = new Mandelbrot(W, H, 3000, HIGHEST).Init(-1, 1, -1, 1);
                        break;
                    case "render":
                        fractal.Render();
                        break;
                    case "shade":
                        fractal.
                        //DistanceBinned();
                        //DistanceHSV();
                        //DistanceMapped();
                        //Exponential(2);
                        //ExposureBinned();
                        //ExposureHSV();
                        //HexColor();
                        LogBaseHighest();
                        //Mapped();
                        //SmoothStep();
                        break;
                    case "save":
                        fractal.SaveSettings("settings.json").SaveDistance("distance.dat").SaveExposure("exposure.dat");
                        break;
                    case "draw":
                        fractal.Draw();
                        break;
                    case "/?":
                        Console.WriteLine(String.Format("{0,5}", "Options:"));
                        Console.WriteLine(String.Format("{0,-10} {1,-40}", "/?", "Display this help message"));
                        Console.WriteLine(String.Format("{0,-10} {1,-40}", "buddhabrot", "Renders a fractal based on the Buddhabrot set"));
                        Console.WriteLine(String.Format("{0,-10} {1,-40}", "glynn", "Renders a fractal based on the Glynn Set"));
                        Console.WriteLine(String.Format("{0,-10} {1,-40}", "julia", "Renders a fractal based on the Julia Set"));
                        Console.WriteLine(String.Format("{0,-10} {1,-40}", "mandelbrot", "Renders a fractal based on the Mandelbrot Set"));
                        break;
                }
            }
        }
    }
}
