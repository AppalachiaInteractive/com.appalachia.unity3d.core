#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_Animation : AppaList<Animation>
    {
        public AppaList_Animation()
        {
        }

        public AppaList_Animation(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Animation(AppaList<Animation> list) : base(list)
        {
        }

        public AppaList_Animation(Animation[] values) : base(values)
        {
        }
    }
}
