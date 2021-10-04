#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_int2 : AppaList<int2>
    {
        public AppaList_int2()
        {
        }

        public AppaList_int2(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_int2(AppaList<int2> list) : base(list)
        {
        }

        public AppaList_int2(int2[] values) : base(values)
        {
        }
    }
}
