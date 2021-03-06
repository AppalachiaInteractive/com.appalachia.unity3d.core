#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_double3x4 : AppaList<double3x4>
    {
        public AppaList_double3x4()
        {
        }

        public AppaList_double3x4(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_double3x4(AppaList<double3x4> list) : base(list)
        {
        }

        public AppaList_double3x4(double3x4[] values) : base(values)
        {
        }
    }
}
