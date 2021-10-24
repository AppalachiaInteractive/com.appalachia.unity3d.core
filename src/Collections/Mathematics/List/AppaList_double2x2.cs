#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_double2x2 : AppaList<double2x2>
    {
        public AppaList_double2x2()
        {
        }

        public AppaList_double2x2(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_double2x2(AppaList<double2x2> list) : base(list)
        {
        }

        public AppaList_double2x2(double2x2[] values) : base(values)
        {
        }
    }
}
