using System;
using Unity.Mathematics;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class double4Average : MovingAverage<double4>
    {
        /// <inheritdoc />
        protected override double4 Add(double4 a, double4 b)
        {
            return a + b;
        }

        /// <inheritdoc />
        protected override double4 Divide(double4 a, int divisor)
        {
            return a / divisor;
        }

        /// <inheritdoc />
        protected override double4 Subtract(double4 a, double4 b)
        {
            return a - b;
        }
    }
}
