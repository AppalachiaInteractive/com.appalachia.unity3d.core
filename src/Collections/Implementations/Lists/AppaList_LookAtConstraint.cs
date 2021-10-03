#region

using System;
using UnityEngine.Animations;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_LookAtConstraint : AppaList<LookAtConstraint>
    {
        public AppaList_LookAtConstraint()
        {
        }

        public AppaList_LookAtConstraint(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_LookAtConstraint(AppaList<LookAtConstraint> list) : base(list)
        {
        }

        public AppaList_LookAtConstraint(LookAtConstraint[] values) : base(values)
        {
        }
    }
}
