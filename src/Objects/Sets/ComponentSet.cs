using System;
using Appalachia.Core.Collections;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Objects.Sets
{
    [Serializable]
    [FoldoutGroup("Components", false)]
    public abstract partial class ComponentSet<TSet, TSetMetadata> : AppalachiaSimpleBase,
                                                                     IComponentSet<TSet, TSetMetadata>
        where TSet : ComponentSet<TSet, TSetMetadata>, new()
        where TSetMetadata : ComponentSetData<TSet, TSetMetadata>
    {
        #region Constants and Static Readonly

        protected const string GAME_OBJECT_NAME_FORMAT = "{0} - {{0}}";

        #endregion

        #region Fields and Autoproperties

        [PropertyOrder(-250)]
        [SerializeField]
        private GameObject gameObject;

        [FormerlySerializedAs("_initialer")]
        [SerializeField, HideInInspector]
        private Initializer _initializer;

        #endregion

        public abstract ComponentSetSorting DesiredComponentOrder { get; }

        protected Initializer initializer
        {
            get
            {
                if (_initializer == null)
                {
                    _initializer = new Initializer();
                }

                return _initializer;
            }
        }

        public static void UpdateComponentSet(
            ref TSet target,
            ref ComponentSetData<TSet, TSetMetadata>.Override data,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSet target,
            ref ComponentSetData<TSet, TSetMetadata>.Optional data,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSet target,
            ref TSetMetadata data,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref ComponentSetData<TSet, TSetMetadata>.Override data,
            ref TSet target,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref ComponentSetData<TSet, TSetMetadata>.Optional data,
            ref TSet target,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSetMetadata data,
            ref TSet target,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        internal static void GetOrAddComponents(TSetMetadata data, TSet set, GameObject parent, string name)
        {
            set.GetOrAddComponents(ref data, parent, name);
        }

        protected virtual string GetGameObjectNameFormat()
        {
            using (_PRF_GetGameObjectNameFormat.Auto())
            {
                // GAME_OBJECT_NAME_FORMAT = "{0} - {{0}}";
                // ComponentSetName = "ABCDEFG";
                return ZString.Format(GAME_OBJECT_NAME_FORMAT, ComponentSetName);

                // ABCDERG - {0}
            }
        }

        protected virtual void GetOrAddComponents(ref TSetMetadata data, GameObject parent, string name)
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

        private int GetDesiredSiblingIndex(int currentIndex, int siblingCount)
        {
            return SiblingIndexCalculator.GetDesiredSiblingIndex(
                DesiredComponentOrder,
                currentIndex,
                siblingCount
            );
        }

        #region IComponentSet<TSet,TSetMetadata> Members

        public virtual void DestroySet()
        {
            using (_PRF_Destroy.Auto())
            {
                if (GameObject)
                {
                    GameObject.DestroySafely();
                }
            }
        }

        public virtual void DisableSet()
        {
            using (_PRF_Disable.Auto())
            {
                if (GameObject)
                {
                    GameObject.SetActive(false);
                }
            }
        }

        public virtual void EnableSet()
        {
            using (_PRF_Enable.Auto())
            {
                if (GameObject)
                {
                    GameObject.SetActive(true);
                }
            }
        }

        public abstract string ComponentSetName { get; }

        public GameObject GameObject => gameObject;

        void IComponentSet<TSet, TSetMetadata>.GetOrAddComponents(
            GameObject parent,
            string name,
            TSetMetadata data)
        {
            GetOrAddComponents(ref data, parent, name);
        }

        #endregion

        #region Profiling

        protected static readonly string _PRF_PFX = typeof(TSet).Name + ".";

        protected static readonly ProfilerMarker _PRF_Destroy =
            new ProfilerMarker(_PRF_PFX + nameof(DestroySet));

        protected static readonly ProfilerMarker _PRF_Disable =
            new ProfilerMarker(_PRF_PFX + nameof(DisableSet));

        protected static readonly ProfilerMarker _PRF_Enable =
            new ProfilerMarker(_PRF_PFX + nameof(EnableSet));

        private static readonly ProfilerMarker _PRF_GetGameObjectNameFormat =
            new ProfilerMarker(_PRF_PFX + nameof(GetGameObjectNameFormat));

        protected static readonly ProfilerMarker _PRF_GetOrAddComponents =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrAddComponents));

        #endregion

        [Serializable]
        public sealed class List : AppaList<TSet>
        {
            public List()
            {
            }

            public List(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
                capacity,
                capacityIncreaseMultiplier,
                noTracking
            )
            {
            }

            public List(AppaList<TSet> list) : base(list)
            {
            }

            public List(TSet[] values) : base(values)
            {
            }
        }
    }
}
