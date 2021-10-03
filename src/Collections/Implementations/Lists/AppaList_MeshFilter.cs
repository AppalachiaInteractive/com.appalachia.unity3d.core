#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_MeshFilter : AppaList<MeshFilter>
    {
        public AppaList_MeshFilter()
        {
        }

        public AppaList_MeshFilter(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_MeshFilter(AppaList<MeshFilter> list) : base(list)
        {
        }

        public AppaList_MeshFilter(MeshFilter[] values) : base(values)
        {
        }
    }
}
