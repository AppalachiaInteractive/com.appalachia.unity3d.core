#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_half3 : AppaList<half3>
    {
        public AppaList_half3()
        {
        }

        public AppaList_half3(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_half3(AppaList<half3> list) : base(list)
        {
        }

        public AppaList_half3(half3[] values) : base(values)
        {
        }
    }
}
