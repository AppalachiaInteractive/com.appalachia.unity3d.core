#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_uint4x2 : AppaList<uint4x2>
    {
        public AppaList_uint4x2()
        {
        }

        public AppaList_uint4x2(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_uint4x2(AppaList<uint4x2> list) : base(list)
        {
        }

        public AppaList_uint4x2(uint4x2[] values) : base(values)
        {
        }
    }
}
