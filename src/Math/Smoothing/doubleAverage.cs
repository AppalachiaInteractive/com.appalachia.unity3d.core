using System;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class doubleAverage : MovingAverage<double>
    {
        /// <inheritdoc />
        protected override double Add(double a, double b)
        {
            return a + b;
        }

        /// <inheritdoc />
        protected override double Divide(double a, int divisor)
        {
            return a / divisor;
        }

        /// <inheritdoc />
        protected override double Subtract(double a, double b)
        {
            return a - b;
        }
    }
}
