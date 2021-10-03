#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_float4x3 : AppaList<float4x3>
    {
        public AppaList_float4x3()
        {
        }

        public AppaList_float4x3(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_float4x3(AppaList<float4x3> list) : base(list)
        {
        }

        public AppaList_float4x3(float4x3[] values) : base(values)
        {
        }
    }
}
