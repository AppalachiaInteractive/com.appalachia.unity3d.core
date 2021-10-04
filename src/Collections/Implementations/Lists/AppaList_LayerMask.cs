#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_LayerMask : AppaList<LayerMask>
    {
        public AppaList_LayerMask()
        {
        }

        public AppaList_LayerMask(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_LayerMask(AppaList<LayerMask> list) : base(list)
        {
        }

        public AppaList_LayerMask(LayerMask[] values) : base(values)
        {
        }
    }
}
