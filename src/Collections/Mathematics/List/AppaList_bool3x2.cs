#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_bool3x2 : AppaList<bool3x2>
    {
        public AppaList_bool3x2()
        {
        }

        public AppaList_bool3x2(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_bool3x2(AppaList<bool3x2> list) : base(list)
        {
        }

        public AppaList_bool3x2(bool3x2[] values) : base(values)
        {
        }
    }
}
