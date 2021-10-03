#region

using System;
using UnityEngine.Animations;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_ParentConstraint : AppaList<ParentConstraint>
    {
        public AppaList_ParentConstraint()
        {
        }

        public AppaList_ParentConstraint(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_ParentConstraint(AppaList<ParentConstraint> list) : base(list)
        {
        }

        public AppaList_ParentConstraint(ParentConstraint[] values) : base(values)
        {
        }
    }
}
