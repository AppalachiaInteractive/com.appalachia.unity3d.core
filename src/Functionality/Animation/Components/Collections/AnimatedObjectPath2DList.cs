#if UNITY_EDITOR
using System;
using Appalachia.Core.Collections;

namespace Appalachia.Core.Functionality.Animation.Components.Collections
{
    [Serializable]
    public sealed class AnimatedObjectPath2DList : AppaList<AnimatedObjectPathList>
    {
        public AnimatedObjectPath2DList()
        {
        }

        public AnimatedObjectPath2DList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AnimatedObjectPath2DList(AppaList<AnimatedObjectPathList> list) : base(list)
        {
        }

        public AnimatedObjectPath2DList(AnimatedObjectPathList[] values) : base(values)
        {
        }
    }
}
#endif
