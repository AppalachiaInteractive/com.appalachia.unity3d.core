#region

using System;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_bool : AppaList<bool>
    {
        public AppaList_bool()
        {
        }

        public AppaList_bool(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_bool(AppaList<bool> list) : base(list)
        {
        }

        public AppaList_bool(bool[] values) : base(values)
        {
        }
    }
}
