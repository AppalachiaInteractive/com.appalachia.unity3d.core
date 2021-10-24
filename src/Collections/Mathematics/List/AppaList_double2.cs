#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_double2 : AppaList<double2>
    {
        public AppaList_double2()
        {
        }

        public AppaList_double2(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_double2(AppaList<double2> list) : base(list)
        {
        }

        public AppaList_double2(double2[] values) : base(values)
        {
        }
    }
}
