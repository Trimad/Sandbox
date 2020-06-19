using System;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{
    public class Julia : Fractal
    {
        public Julia(int _width, int _height, int _highestExposureTarget)
        {
            this.name = "Julia";
            this.width = _width;
            this.height = _height;
            this.highestExposureTarget = _highestExposureTarget;
        }

        public override Fractal Render()
        {
            //Complex c = new Complex(-0.7, 0.27015);
            //Complex c = new Complex(0.285, 0.01);
            //Complex c = new Complex(-0.4, 0.6);
            Complex c = new Complex(-0.75, 0.156);
            _ = Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int iterations = 0;
                    double zx = Auxiliary.MapDouble(x, 0, width, domain[0][0][0], domain[width - 1][0][0]);
                    double zy = Auxiliary.MapDouble(y, 0, height, domain[0][0][1], domain[0][height - 1][1]);
                    Complex z = new Complex(zx, zy);
                    do
                    {
                        z.Square();
                        z.Add(c);

                    }
                    while (z.MagnitudeOpt() <= 4.0 && iterations++ < highestExposureTarget);
                    int index = x + y * width;
                    //http://www.iquilezles.org/www/articles/mset_smooth/mset_smooth.htm
                    //distance[x + y * width] = Math.Log(z.Distance(c));
                    distance[x + y * width] = z.Distance(z);
                    exposure[index] = iterations;
                    if (highest < exposure[index])
                    {
                        highest = exposure[index];
                    }
                    //distance[x + y * width] = Math.Log(z.Distance(c) - z.Magnitude());
                    //distance[x + y * width] = iterations - Math.Log(Math.Log(z.Magnitude()) / Math.Log(256))/ Math.Log(2.0);
                }
            });
            return this;
        }
    }
}