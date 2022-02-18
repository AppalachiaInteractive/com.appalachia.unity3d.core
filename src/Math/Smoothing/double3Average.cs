using System;
using Unity.Mathematics;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class double3Average : MovingAverage<double3>
    {
        /// <inheritdoc />
        protected override double3 Add(double3 a, double3 b)
        {
            return a + b;
        }

        /// <inheritdoc />
        protected override double3 Divide(double3 a, int divisor)
        {
            return a / divisor;
        }

        /// <inheritdoc />
        protected override double3 Subtract(double3 a, double3 b)
        {
            return a - b;
        }
    }
}
