#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_float2x4 : AppaList<float2x4>
    {
        public AppaList_float2x4()
        {
        }

        public AppaList_float2x4(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_float2x4(AppaList<float2x4> list) : base(list)
        {
        }

        public AppaList_float2x4(float2x4[] values) : base(values)
        {
        }
    }
}
