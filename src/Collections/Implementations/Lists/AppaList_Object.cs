using System;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_Object : AppaList<Object>
    {
        public AppaList_Object()
        {
        }

        public AppaList_Object(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Object(AppaList<Object> list) : base(list)
        {
        }

        public AppaList_Object(Object[] values) : base(values)
        {
        }
    }
}
