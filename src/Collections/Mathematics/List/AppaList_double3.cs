#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_double3 : AppaList<double3>
    {
        public AppaList_double3()
        {
        }

        public AppaList_double3(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_double3(AppaList<double3> list) : base(list)
        {
        }

        public AppaList_double3(double3[] values) : base(values)
        {
        }
    }
}
