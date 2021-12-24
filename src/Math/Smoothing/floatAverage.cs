using System;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class floatAverage : MovingAverage<float>
    {
        protected override float Add(float a, float b)
        {
            return a + b;
        }

        protected override float Divide(float a, int divisor)
        {
            return a / divisor;
        }

        protected override float Subtract(float a, float b)
        {
            return a - b;
        }
    }
}
