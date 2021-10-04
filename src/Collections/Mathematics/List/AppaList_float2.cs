#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_float2 : AppaList<float2>
    {
        public AppaList_float2()
        {
        }

        public AppaList_float2(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_float2(AppaList<float2> list) : base(list)
        {
        }

        public AppaList_float2(float2[] values) : base(values)
        {
        }
    }
}
