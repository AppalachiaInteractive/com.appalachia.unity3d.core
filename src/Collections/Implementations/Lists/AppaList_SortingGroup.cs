#region

using System;
using UnityEngine.Rendering;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_SortingGroup : AppaList<SortingGroup>
    {
        public AppaList_SortingGroup()
        {
        }

        public AppaList_SortingGroup(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_SortingGroup(AppaList<SortingGroup> list) : base(list)
        {
        }

        public AppaList_SortingGroup(SortingGroup[] values) : base(values)
        {
        }
    }
}
