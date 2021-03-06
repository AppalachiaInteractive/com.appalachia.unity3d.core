#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_int3 : AppaList<int3>
    {
        public AppaList_int3()
        {
        }

        public AppaList_int3(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_int3(AppaList<int3> list) : base(list)
        {
        }

        public AppaList_int3(int3[] values) : base(values)
        {
        }
    }
}
