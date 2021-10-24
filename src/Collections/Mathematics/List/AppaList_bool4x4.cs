#region

using System;
using Unity.Mathematics;

#endregion

namespace Appalachia.Core.Collections.Mathematics.List
{
    [Serializable]
    public sealed class AppaList_bool4x4 : AppaList<bool4x4>
    {
        public AppaList_bool4x4()
        {
        }

        public AppaList_bool4x4(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_bool4x4(AppaList<bool4x4> list) : base(list)
        {
        }

        public AppaList_bool4x4(bool4x4[] values) : base(values)
        {
        }
    }
}
