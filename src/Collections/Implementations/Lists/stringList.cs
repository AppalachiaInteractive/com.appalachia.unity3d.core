#region

using System;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class stringList : AppaList<string>
    {
        public stringList()
        {
        }

        public stringList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public stringList(AppaList<string> list) : base(list)
        {
        }

        public stringList(string[] values) : base(values)
        {
        }
    }
}
