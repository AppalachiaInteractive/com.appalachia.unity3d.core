using System;
using Appalachia.Core.Collections;
using UnityEngine;

namespace Appalachia.Core.Functionality.Animation.Components.Collections
{
    [Serializable]
    public sealed class AnimationClipList : AppaList<AnimationClip>
    {
        public AnimationClipList()
        {
        }

        public AnimationClipList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AnimationClipList(AppaList<AnimationClip> list) : base(list)
        {
        }

        public AnimationClipList(AnimationClip[] values) : base(values)
        {
        }
    }
}
