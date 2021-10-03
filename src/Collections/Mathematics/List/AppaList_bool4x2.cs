#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_bool4x2 : AppaList<bool4x2>
    {
        public AppaList_bool4x2()
        {
        }

        public AppaList_bool4x2(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_bool4x2(AppaList<bool4x2> list) : base(list)
        {
        }

        public AppaList_bool4x2(bool4x2[] values) : base(values)
        {
        }
    }
}
