#if UNITY_EDITOR
using System;
using Appalachia.Core.Collections;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Functionality.Animation.Components.Collections
{
    [Serializable, HideLabel, Title("Mappings")]
    [ListDrawerSettings(ShowItemCount = false, DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
    public class AnimatedObjectPathLookup : AppaLookup2<string, AnimationClip, AnimatedObjectPathList, stringList,
        AnimationClipList, AnimatedObjectPath2DList, AnimatedObjectPathListLookup,
        AnimatedObjectPathListLookupCollection>
    {
        /// <inheritdoc />
        protected override bool ShouldDisplayTitle => true;

        /// <inheritdoc />
        protected override Color GetDisplayColor(string key, AnimatedObjectPathListLookup value)
        {
            return Colors.WhiteSmokeGray96;
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(string key, AnimatedObjectPathListLookup value)
        {
            return ZString.Format("{0} Clips", value.Keys.Count);
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(string key, AnimatedObjectPathListLookup value)
        {
            return key;
        }
    }
}

#endif
