#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_uint3x2 : AppaList<uint3x2>
    {
        public AppaList_uint3x2()
        {
        }

        public AppaList_uint3x2(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_uint3x2(AppaList<uint3x2> list) : base(list)
        {
        }

        public AppaList_uint3x2(uint3x2[] values) : base(values)
        {
        }
    }
}
