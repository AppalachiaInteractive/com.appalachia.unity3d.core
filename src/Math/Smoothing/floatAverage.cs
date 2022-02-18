using System;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class floatAverage : MovingAverage<float>
    {
        /// <inheritdoc />
        protected override float Add(float a, float b)
        {
            return a + b;
        }

        /// <inheritdoc />
        protected override float Divide(float a, int divisor)
        {
            return a / divisor;
        }

        /// <inheritdoc />
        protected override float Subtract(float a, float b)
        {
            return a - b;
        }
    }
}
