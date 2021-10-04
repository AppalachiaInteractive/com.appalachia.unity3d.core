#region

using System;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_uint : AppaList<uint>
    {
        public AppaList_uint()
        {
        }

        public AppaList_uint(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_uint(AppaList<uint> list) : base(list)
        {
        }

        public AppaList_uint(uint[] values) : base(values)
        {
        }
    }
}
