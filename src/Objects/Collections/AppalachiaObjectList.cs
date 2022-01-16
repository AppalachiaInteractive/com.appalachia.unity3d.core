using System;
using Appalachia.Core.Collections;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Components.Styling
{
    [Serializable]
    public sealed class AppalachiaObjectList : AppaList<AppalachiaObject>
    {
        public AppalachiaObjectList()
        {
        }

        public AppalachiaObjectList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppalachiaObjectList(AppaList<AppalachiaObject> list) : base(list)
        {
        }

        public AppalachiaObjectList(AppalachiaObject[] values) : base(values)
        {
        }
    }
}
