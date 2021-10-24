#region

using System;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_double : AppaList<double>
    {
        public AppaList_double()
        {
        }

        public AppaList_double(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_double(AppaList<double> list) : base(list)
        {
        }

        public AppaList_double(double[] values) : base(values)
        {
        }
    }
}
