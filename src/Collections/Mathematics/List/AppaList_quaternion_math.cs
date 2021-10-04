#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_quaternion_math : AppaList<quaternion>
    {
        public AppaList_quaternion_math()
        {
        }

        public AppaList_quaternion_math(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_quaternion_math(AppaList<quaternion> list) : base(list)
        {
        }

        public AppaList_quaternion_math(quaternion[] values) : base(values)
        {
        }
    }
}
