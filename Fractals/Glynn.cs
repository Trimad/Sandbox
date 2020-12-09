using System;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{
    class Glynn : Fractal
    {
        //private readonly double modifier_A;
        //private readonly double modifier_B;

        public Glynn(int _width, int _height, int _highestExposureTarget, double modifier_A, double modifier_B)
        {
            this.name = "Glynn Set";
            this.width = _width;
            this.height = _height;
            this.highestExposureTarget = _highestExposureTarget;
            //this.modifier_A = modifier_A;
            //this.modifier_B = modifier_B;
        }

        public override Fractal Render()
        {
            Complex c = new Complex(0, 0);

            //System.Numerics.Complex c = new System.Numerics.Complex(-0.39, -0.7);
            _ = Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int iterations = 0;
                    double zx = Helper.Map(x, 0, width, domain[0][0][0], domain[width - 1][0][0]);
                    double zy = Helper.Map(y, 0, height, domain[0][0][1], domain[0][height - 1][1]);
                    //double lastDistance = 0;
                    double totalDistance = 0;

                    Complex z = new Complex(zx, zy);
                    Complex last = new Complex(0, 0);//for shading
                    do
                    {
                        last = z;
                        z = z.Power(1.5);
                        z = z.Subtract(0.2);
                        totalDistance += z.Distance(last);
                    }

                    //while (z.Magnitude() <= 2 && iterations++ < highestExposureTarget);
                    while (z.MagnitudeOpt() <= 4 && iterations++ < highestExposureTarget);
                    //lastDistance = z.Distance(last);
                    int index = x + y * width;
                    exposure[index] = iterations;
                    distance[index] = Math.Log(totalDistance);
                    //distance[index] = (iterations > 51) ? totalDistance : 0;

                    if (highestActual < exposure[index])
                    {
                        //Console.WriteLine(zTemp.Distance(new Complex(0, 0)));
                        highestActual = exposure[index];
                    }

                }
            });

            return this;
        }
    }
}