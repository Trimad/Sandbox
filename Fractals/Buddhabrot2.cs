using System;
using System.Threading;

namespace Sandbox.Fractals
{
    public class Buddhabrot2 : Fractal
    {
        [ThreadStatic]
        static Random rand = new Random();
        int cutoff;
        public Buddhabrot2(int _width, int _height, int _cutoff, int _highestExposureTarget)
        {
            this.name = "Buddhabrot";
            this.width = _width;
            this.height = _height;

            this.cutoff = _cutoff;
            this.highestExposureTarget = _highestExposureTarget;
        }

        public override Fractal Render()
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            Thread[] pool = new Thread[Environment.ProcessorCount];
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = new Thread(Plot);
            }
            foreach (Thread thread in pool)
            {
                thread.Start();
            }

            foreach (Thread thread in pool)
            {
                thread.Join();
            }

            return this;
        }

        private void Plot()
        {

            double startX = domain[0][0][0];
            double stopX = domain[width - 1][0][0];
            double stepX = (domain[width - 1][0][0] - domain[width - 2][0][0])/5;

            double startY = domain[0][0][0];
            double stopY = domain[width - 1][0][0];
            double stepY = (domain[width - 1][0][0] - domain[width - 2][0][0])/5;

            //if (rand == null)
            //{
            //    rand = new Random();
            //}
            while (highest < highestExposureTarget)
            {

                for (double x = startX; x < stopX; x += stepX) {
                    for (double y = startY; y < stopY; y += stepY) {
                        
                        if (Iterate(x, y, false))
                        {
                            Iterate(x, y, true);
                        }
                        
                    }
                }

                //if (highest % 1000 == 0) { Console.WriteLine(highest); }
                //double r = rand.NextDouble() * (domain[width - 1][0][0] - domain[0][0][0]) + domain[0][0][0];
                //double i = rand.NextDouble() * (domain[0][height - 1][1] - domain[0][0][1]) + domain[0][0][1];


            }

        }

        private bool Iterate(double r, double i, bool drawIt)
        {
            Complex temp = new Complex(0, 0);
            Complex c = new Complex(r, i);
            Complex znew = new Complex(0, 0);
            double iterations = 0;
            do
            {
                znew.Copy(temp);
                znew.Square();
                znew.AddRotated(c);

                if (drawIt && iterations >= cutoff)
                {
                    double Ax = domain[0][0][0];
                    double Bx = domain[width - 1][0][0];
                    //double Cx = 0;
                    double Dx = width;
                    int rx = (int)((znew.i - Ax) / (Bx - Ax) * Dx);

                    double Ay = domain[0][0][1];
                    double By = domain[0][height - 1][1];
                    //double Cy = 0;
                    double Dy = height;

                    int iy = (int)((znew.r - Ay) / (By - Ay) * Dy);

                    if (rx >= 0 && iy >= 0 && iy < height && rx < width)
                    {
                        int index = rx + iy * width;
                        exposure[index]++;//1 divided by 255
                        distance[index] = iterations;

                        if (highest < exposure[index])
                        {
                            highest = exposure[index];
                        }

                    }
                }

                if (znew.MagnitudeOpt() > 4.0)
                {
                    // escapes
                    return true;
                }
                temp.Copy(znew);

            }
            while (iterations++ < highestExposureTarget);

            ////does not escape
            return false;
        }

    }
}