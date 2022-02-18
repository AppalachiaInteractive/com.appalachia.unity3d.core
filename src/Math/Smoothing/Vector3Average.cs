using System;
using UnityEngine;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public class Vector3Average : MovingAverage<Vector3>
    {
        /// <inheritdoc />
        protected override Vector3 Add(Vector3 a, Vector3 b)
        {
            return a + b;
        }

        /// <inheritdoc />
        protected override Vector3 Divide(Vector3 a, int divisor)
        {
            return a / divisor;
        }

        /// <inheritdoc />
        protected override Vector3 Subtract(Vector3 a, Vector3 b)
        {
            return a - b;
        }
    }
}
