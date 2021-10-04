using System;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class doubleAverage : MovingAverage<double>
    {
        protected override double Add(double a, double b)
        {
            return a + b;
        }

        protected override double Subtract(double a, double b)
        {
            return a - b;
        }

        protected override double Divide(double a, int divisor)
        {
            return a / divisor;
        }
    }
}
