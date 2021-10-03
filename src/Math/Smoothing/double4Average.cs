using System;
using Unity.Mathematics;

namespace Appalachia.Core.Math
{
    [Serializable]
    public class double4Average : MovingAverage<double4>
    {
        protected override double4 Add(double4 a, double4 b)
        {
            return a + b;
        }

        protected override double4 Subtract(double4 a, double4 b)
        {
            return a - b;
        }

        protected override double4 Divide(double4 a, int divisor)
        {
            return a / divisor;
        }
    }
}