#region

using System;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_float : AppaList<float>
    {
        public AppaList_float()
        {
        }

        public AppaList_float(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_float(AppaList<float> list) : base(list)
        {
        }

        public AppaList_float(float[] values) : base(values)
        {
        }
    }
}
