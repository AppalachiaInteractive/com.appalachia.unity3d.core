#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_bool3 : AppaList<bool3>
    {
        public AppaList_bool3()
        {
        }

        public AppaList_bool3(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_bool3(AppaList<bool3> list) : base(list)
        {
        }

        public AppaList_bool3(bool3[] values) : base(values)
        {
        }
    }
}
