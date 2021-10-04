using System;
using Unity.Mathematics;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class double2Average : MovingAverage<double2>
    {
        protected override double2 Add(double2 a, double2 b)
        {
            return a + b;
        }

        protected override double2 Subtract(double2 a, double2 b)
        {
            return a - b;
        }

        protected override double2 Divide(double2 a, int divisor)
        {
            return a / divisor;
        }
    }
}
