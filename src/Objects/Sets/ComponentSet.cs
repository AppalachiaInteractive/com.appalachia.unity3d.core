using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    [Serializable]
    [InlineProperty, HideLabel]
    public abstract partial class ComponentSet<TSet, TSetMetadata> : AppalachiaSimpleBase,
                                                                     IComponentSet<TSet, TSetMetadata>
        where TSet : ComponentSet<TSet, TSetMetadata>, new()
        where TSetMetadata : ComponentSetMetadata<TSet, TSetMetadata>
    {
        #region Constants and Static Readonly

        protected const string GAME_OBJECT_NAME_FORMAT = "{0} - {{0}}";

        #endregion

        #region Fields and Autoproperties

        [PropertyOrder(-250)]
        [SerializeField]
        private GameObject gameObject;

        [SerializeField, HideInInspector]
        private Initializer _initialer;

        #endregion

        public abstract ComponentSetSorting DesiredComponentOrder { get; }

        protected Initializer initializer
        {
            get
            {
                if (_initialer == null)
                {
                    _initialer = new Initializer();
                }

                return _initialer;
            }
        }

        protected virtual string GetGameObjectNameFormat()
        {
            using (_PRF_GetGameObjectNameFormat.Auto())
            {
                return ZString.Format(GAME_OBJECT_NAME_FORMAT, ComponentSetName);
            }
        }

        private int GetDesiredSiblingIndex(int currentIndex, int siblingCount)
        {
            return SiblingIndexCalculator.GetDesiredSiblingIndex(
                DesiredComponentOrder,
                currentIndex,
                siblingCount
            );
        }

        #region IComponentSet<TSet,TSetMetadata> Members

        public abstract string ComponentSetName { get; }

        public GameObject GameObject => gameObject;

        public virtual void GetOrAddComponents(
            GameObject parent,
            string name,
            TSetMetadata data /*, string prefixOverride = null*/)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                var nameFormatString = /*prefixOverride.IsNullOrWhiteSpace()
                    ? */GetGameObjectNameFormat()
                    /*: prefixOverride*/;

                var targetName = ZString.Format(nameFormatString, name);

                if (gameObject == null)
                {
                    parent.GetOrAddChild(ref gameObject, targetName, true);
                }

                gameObject.name = targetName;

                var siblingCount = parent.transform.childCount - 1;

                if (siblingCount == 0)
                {
                    return;
                }

                var currentIndex = gameObject.transform.GetSiblingIndex();

                var targetIndex = GetDesiredSiblingIndex(currentIndex, siblingCount);

                if (currentIndex != targetIndex)
                {
                    gameObject.transform.SetSiblingIndex(targetIndex);
                }
            }
        }

        #endregion

        #region Profiling

        protected static readonly string _PRF_PFX = typeof(TSet).Name + ".";

        private static readonly ProfilerMarker _PRF_GetGameObjectNameFormat =
            new ProfilerMarker(_PRF_PFX + nameof(GetGameObjectNameFormat));

        protected static readonly ProfilerMarker _PRF_GetOrAddComponents =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrAddComponents));

        #endregion
    }
}
