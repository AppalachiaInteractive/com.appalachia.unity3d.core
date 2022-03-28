#if UNITY_EDITOR
using System;
using Appalachia.Core.Collections;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Functionality.Animation.Components.Collections
{
    [Serializable]
    [ListDrawerSettings(
        ShowItemCount = false,
        DraggableItems = false,
        HideAddButton = true,
        HideRemoveButton = true,
        Expanded = false
    )]
    public sealed class AnimatedObjectPathList : AppaList<AnimatedObjectPath>
    {
        public AnimatedObjectPathList()
        {
        }

        public AnimatedObjectPathList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AnimatedObjectPathList(AppaList<AnimatedObjectPath> list) : base(list)
        {
        }

        public AnimatedObjectPathList(AnimatedObjectPath[] values) : base(values)
        {
        }
    }
}

#endif
