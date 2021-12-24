#region

using System;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class intList : AppaList<int>
    {
        public intList()
        {
        }

        public intList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public intList(AppaList<int> list) : base(list)
        {
        }

        public intList(int[] values) : base(values)
        {
        }
    }
}
