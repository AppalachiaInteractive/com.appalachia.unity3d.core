#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_uint2 : AppaList<uint2>
    {
        public AppaList_uint2()
        {
        }

        public AppaList_uint2(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_uint2(AppaList<uint2> list) : base(list)
        {
        }

        public AppaList_uint2(uint2[] values) : base(values)
        {
        }
    }
}
