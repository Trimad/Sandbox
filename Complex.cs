using System;
using System.IO;

//https://github.com/microsoft/referencesource/blob/master/System.Numerics/System/Numerics/Complex.cs

namespace Sandbox
{
    public struct Complex
    {
        public double r; //real
        public double i; //imaginary

        public Complex(double a, double b)
        {
            this.r = a;
            this.i = b;
        }

        public double Angle()
        {
            var radian = Math.Atan2(i, r);
            var angle = (radian * (180 / Math.PI) + 360) % 360;
            return angle;
        }
        public double Angle(Complex c)
        {
            var radian = Math.Atan2(i - c.i, r - c.r);
            var angle = (radian * (180 / Math.PI) + 360) % 360;
            return angle;
        }

        public void Copy(Complex c)
        {
            this.r = c.r;
            this.i = c.i;
        }
        public void Square()
        {
            double temp = (r * r) - (i * i);
            i = 2.0 * r * i;
            r = temp;
        }

        public double Magnitude()
        {
            return Math.Sqrt(r * r + i * i);//less than 2
        }
        public double MagnitudeOpt()
        {
            return r * r + i * i;//less than 4
        }
        public void SquareRotated()
        {
            double temp = (r * r) - (i * i);
            r = 2.0 * r * i;
            i = temp;
        }

        public double Distance(Complex c)
        {
            double p1 = c.r - r;
            double p2 = c.i - i;
            double hypotenuse = Math.Sqrt((p1 * p1) + (p2 * p2));
            //Console.WriteLine(hypotenuse);
            return hypotenuse;

        }
        public Complex Power(double x)
        {
            double modulus = Math.Sqrt(r * r + i * i);
            double arg = Math.Atan2(i, r);
            double log_re = Math.Log(modulus);
            double log_im = arg;
            double x_log_re = x * log_re;
            double x_log_im = x * log_im;
            double modulus_ans = Math.Exp(x_log_re);
            return new Complex(modulus_ans * Math.Cos(x_log_im), modulus_ans * Math.Sin(x_log_im));
        }

        public void Add(Complex c)
        {
            r += c.r;
            i += c.i;
        }

        public void Set(double _r, double _i)
        {
            r = _r;
            i = _i;
        }

        public void Subtract(Complex c)
        {
            r -= c.r;
            i -= c.i;
        }
        public Complex Subtract(double x)
        {
            return new Complex(r - x, i);
        }
        public void AddRotated(Complex c)
        {
            r += c.i;
            i += c.r;
        }
        public void Reset()
        {
            r = 0;
            i = 0;
        }


        public static Complex operator +(Complex left, Complex right)
        {
            return (new Complex((left.r + right.r), (left.i + right.i)));

        }

        public static Complex operator -(Complex left, Complex right)
        {
            return (new Complex((left.r - right.r), (left.i - right.i)));
        }

    }
}
