using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Fractals
{
    class LogisticMap : Fractal
    {

        public LogisticMap(int _width, int _height, int _highestExposureTarget)
        {
            this.name = "Logistic Map";
            this.width = _width;
            this.height = _height;
            this.highestExposureTarget = _highestExposureTarget;
        }

        public override Fractal Render()
        {

            _ = Parallel.For(0, highestExposureTarget, i =>
            {
                Expose();
            });

            return this;
        }

        private void Expose()
        {
            double startX = 3.5;
            double stopX = 4;
            Random bag = new Random();
            double fertility = bag.NextDouble() * (stopX - startX) + startX;
            double population = 0.5;
            int cutoff = 500;
            int bailout = 1000;

            for (int i = 0; i < bailout; i++)
            {
                population = fertility * population * (1 - population);

                if (i > cutoff)
                {
                    int x = (int)Helper.Map(fertility, startX, stopX, 0, width - 1);
                    int y = (int)Helper.Map(population, 0, 1, height - 1, 0);
                    //int e = 2;
                    //int index = Helper.LimitToRange(x + bag.Next(-e, e), 0, width-1) + Helper.LimitToRange(y + bag.Next(-e,e), 0, height-1) * width;
                    //int e = 2;
                    int index = x + y * width;
                    exposure[index]++;

                }
            }
        }
    }
}
