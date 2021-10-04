#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_float3 : AppaList<float3>
    {
        public AppaList_float3()
        {
        }

        public AppaList_float3(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_float3(AppaList<float3> list) : base(list)
        {
        }

        public AppaList_float3(float3[] values) : base(values)
        {
        }
    }
}
