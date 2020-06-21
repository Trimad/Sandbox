//https://www.youtube.com/watch?v=8RcjQPbvvRU
//https://www.youtube.com/watch?v=8RcjQPbvvRU
//https://www.youtube.com/watch?v=8RcjQPbvvRU

using Sandbox.Fractals;
using System;

namespace Sandbox
{
    class Program
    {
        private static readonly int H = 2048;
        private static readonly int W = 4096;
        //private static readonly int W = 2048;
        //private static readonly int H = 2048;
        private static readonly int HIGHEST = 1000;
        static void Main(string[] args)
        {

            Console.WriteLine("Version: {0}", Environment.Version.ToString());

            if (args[0] == "draw")
            {
                if (args[1] == "fractal")
                {
                    switch (args[2])
                    {

                        case "/?":
                            Console.WriteLine(String.Format("{0,5}", "Options:"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "/?", "Display this help message"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "buddhabrot", "Renders a fractal based on the Buddhabrot set"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "glynn", "Renders a fractal based on the Glynn Set"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "julia", "Renders a fractal based on the Julia Set"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "mandelbrot", "Renders a fractal based on the Mandelbrot Set"));
                            break;
                        case "buddhabrot":
                            _ = new Buddhabrot(W, H, 3, HIGHEST)
                                .Init(-1.9, 1.9, -2.1, 1.7)
                                .Render()
                                .SaveSettings("settings.json")
                                .SaveDistance("distance.dat")
                                .SaveExposure("exposure.dat")
                                //.HSVtoRGB()
                                .LogBaseHighest()
                                .SaveImage();
                            break;
                        case "glynn":
                            _ = new Glynn(W, H, HIGHEST)
                                .Init(-2, 2, -1, 1)
                                //.Init(0.085, 0.485, -0.69, -0.29)
                                .Render()
                                .SaveSettings("settings.json")
                                .SaveDistance("distance.dat")
                                .SaveExposure("exposure.dat")
                                //.ExposureHSV()
                                .DistanceHSV()
                                //.Binned()
                                .SaveImage();
                            break;
                        case "julia":
                            _ = new Julia(W, H, HIGHEST).Init(-0.39, -0.7, 0.19, -0.12);
                            //.Init(-0.8, 0.8, -0.8, 0.8)
                            break;
                        case "mandelbrot":
                            _ = new Mandelbrot(W, H, 3000, HIGHEST).Init(-2, 2, -2, 2);
                            break;
                        default:
                            Console.WriteLine("Error: unrecognized or incomplete command line.");
                            break;
                    }

                }
                else if (args[1] == "other") { }
            }
            else if (args[0] == "load")
            {
                if (args[1] == "fractal")
                {
                    switch (args[2])
                    {
                        case "/?":
                            Console.WriteLine(String.Format("{0,5}", "Options:"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "/?", "Display this help message"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "buddhabrot", "Renders a fractal based on the Buddhabrot set"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "glynn", "Renders a fractal based on the Glynn Set"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "julia", "Renders a fractal based on the Julia Set"));
                            Console.WriteLine(String.Format("{0,-10} {1,-40}", "mandelbrot", "Renders a fractal based on the Mandelbrot Set"));
                            break;
                        case "buddhabrot":
                            _ = new Buddhabrot(W, H, 3, HIGHEST)
                                .Init(-1.9, 1.9, -2.1, 1.7)
                                .LoadSettings("settings.json")
                                .LoadDistance("distance.dat")
                                .LoadExposure("exposure.dat")
                                //.ExposureHSV()
                                .LogBaseHighest()
                                .SaveImage();
                            break;
                        case "glynn":
                            _ = new Glynn(W, H, HIGHEST)
                                //.Init(-2, 2, -2, 2)
                                //.Init(0.085, 0.485, -0.69, -0.29)
                                .Init(-2, 2, -1, 1)
                                .LoadSettings("settings.json")
                                .LoadDistance("distance.dat")
                                .LoadExposure("exposure.dat")
                                //.ExposureHSV()
                                .DistanceHSV()
                                //.LogBaseHighest()
                                //.Binned()
                                .SaveImage();
                            break;
                        case "julia":
                            _ = new Julia(W, H, HIGHEST).Init(-0.39, -0.7, 0.19, -0.12)
                                .LoadSettings("settings.json")
                                .LoadDistance("distance.dat")
                                .LoadExposure("exposure.dat")
                                .Binned()
                                .SaveImage();
                            break;
                        case "mandelbrot":
                            _ = new Mandelbrot(W, H, 3000, HIGHEST).Init(-2, 2, -2, 2)
                                .LoadSettings("settings.json")
                                .LoadDistance("distance.dat")
                                .LoadExposure("exposure.dat")
                                .Binned()
                                .SaveImage();
                            break;
                        default:
                            Console.WriteLine("Error: unrecognized or incomplete command line.");
                            break;
                    }
                }
                else if (args[1] == "other") { }
            }
        }
    }
}
