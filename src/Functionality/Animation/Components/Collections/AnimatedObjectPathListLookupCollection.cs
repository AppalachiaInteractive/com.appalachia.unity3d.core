#if UNITY_EDITOR
using System;
using Appalachia.Core.Collections;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Functionality.Animation.Components.Collections
{
    [Serializable]
    [ListDrawerSettings(ShowItemCount = false)]
    public sealed class AnimatedObjectPathListLookupCollection : AppaList<AnimatedObjectPathListLookup>
    {
        public AnimatedObjectPathListLookupCollection()
        {
        }

        public AnimatedObjectPathListLookupCollection(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AnimatedObjectPathListLookupCollection(AppaList<AnimatedObjectPathListLookup> list) : base(list)
        {
        }

        public AnimatedObjectPathListLookupCollection(AnimatedObjectPathListLookup[] values) : base(values)
        {
        }
    }
}
#endif
