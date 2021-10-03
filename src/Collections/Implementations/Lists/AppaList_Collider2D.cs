#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_Collider2D : AppaList<Collider2D>
    {
        public AppaList_Collider2D()
        {
        }

        public AppaList_Collider2D(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_Collider2D(AppaList<Collider2D> list) : base(list)
        {
        }

        public AppaList_Collider2D(Collider2D[] values) : base(values)
        {
        }
    }
}
