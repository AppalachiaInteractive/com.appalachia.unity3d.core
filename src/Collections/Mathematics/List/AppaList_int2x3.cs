#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_int2x3 : AppaList<int2x3>
    {
        public AppaList_int2x3()
        {
        }

        public AppaList_int2x3(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_int2x3(AppaList<int2x3> list) : base(list)
        {
        }

        public AppaList_int2x3(int2x3[] values) : base(values)
        {
        }
    }
}
