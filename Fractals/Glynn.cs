using System;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{
    class Glynn : Fractal
    {
        public Glynn(int _width, int _height, int _highestExposureTarget)
        {
            this.name = "Glynn Set";
            this.width = _width;
            this.height = _height;
            this.highestExposureTarget = _highestExposureTarget;
        }

        public override Fractal Render()
        {
            Complex c = new Complex(-0.39, -0.7);
            //System.Numerics.Complex c = new System.Numerics.Complex(-0.39, -0.7);
            _ = Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int iterations = 0;
                    double zx = Auxiliary.MapDouble(x, 0, width, domain[0][0][0], domain[width - 1][0][0]);
                    double zy = Auxiliary.MapDouble(y, 0, height, domain[0][0][1], domain[0][height - 1][1]);
                    //Complex z = new Complex(zx, zy);
                    System.Numerics.Complex z = new System.Numerics.Complex(zx, zy);
                    do
                    {
                        z = System.Numerics.Complex.Pow(z, 1.5);
                        z = System.Numerics.Complex.Subtract(z, 0.2);
                        //z = Complex.Pow(z, 1.5);
                        //z = Complex.Subtract(z, 0.2);
                        //z.Add(c);
                        //z.Pow(1.5);
                        //z.Subtract(c);
                    }

                    //while (z.Magnitude() <= 2.0 && iterations++ < highestExposureTarget);
                    while (z.Magnitude <= 2.0 && iterations++ < highestExposureTarget);
                    
                    Complex zTemp = new Complex(z.Real, z.Imaginary);
                    //distance[x + y * width] = iterations / zTemp.Distance(new Complex(0,0));
                    int index = x + y * width;
                    distance[index] = z.Phase;
                    exposure[index] = iterations;

                    if (highest < exposure[index])
                    {
                        highest = exposure[index];
                    }

                }
            });

            return this;
        }
    }
}