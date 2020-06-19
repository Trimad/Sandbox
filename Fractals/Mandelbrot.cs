using System.Linq;
using System.Threading.Tasks;


namespace Sandbox.Fractals
{
    class Mandelbrot : Fractal
    {
        int bailout;
        public Mandelbrot(int _width, int _height, int _bailout, int _highestExposureTarget)
        {
            this.name = "Mandelbrot";
            this.width = _width;
            this.height = _height;
            this.bailout = _bailout;
            this.highestExposureTarget = _highestExposureTarget;
        }

        public override Fractal Render()
        {
          
            Parallel.For(0, width, x =>
            {
             
                for (int y = 0; y < height; y++)
                {

                    double r = domain[x][y][0];
                    double i = domain[x][y][1];
                    Complex c = new Complex(r, i);
                    Complex z = new Complex(0, 0);
                    int iterations = 0;
                    do
                    {
                        z.Square();
                        z.Add(c);
                    } while (z.MagnitudeOpt() < 4.0 && iterations++ < bailout);
                    int index = x + y * width;
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