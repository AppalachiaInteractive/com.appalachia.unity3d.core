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
    public abstract partial class ComponentSet<TSet, TSetData> : AppalachiaSimpleBase,
                                                                 IComponentSet<TSet, TSetData>
        where TSet : ComponentSet<TSet, TSetData>, new()
        where TSetData : ComponentSetData<TSet, TSetData>
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

        [PropertyOrder(-300)]
        [SerializeField]
        public bool isSortingDisabled;

        #endregion

        public abstract ComponentSetSorting DesiredComponentOrder { get; }

        protected abstract bool IsUI { get; }

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

        internal static void GetOrAddComponents(
            ref TSet set,
            TSetData data,
            GameObject setParent,
            string setName)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                set ??= new TSet();

                set.GetOrAddComponents(data, setParent, setName);
            }
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

        /// <summary>
        ///     Creates the set underneath the <paramref name="setParent" />, using the provided <paramref name="setName" />, and adds the required components.
        /// </summary>
        /// <param name="data">The component set data.</param>
        /// <param name="setParent">The parent of the set.</param>
        /// <param name="setName">The name of the set.</param>
        protected virtual void GetOrAddComponents(TSetData data, GameObject setParent, string setName)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                var nameFormatString = GetGameObjectNameFormat();

                var setObjectName = ZString.Format(nameFormatString, setName);

                if (gameObject == null)
                {
                    setParent.GetOrAddChild(ref gameObject, setObjectName, IsUI);
                }

                gameObject.name = setObjectName;

                var siblingCount = setParent.transform.childCount - 1;

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
            if (isSortingDisabled)
            {
                return currentIndex;
            }

            return SiblingIndexCalculator.GetDesiredSiblingIndex(
                DesiredComponentOrder,
                currentIndex,
                siblingCount
            );
        }

        #region IComponentSet<TSet,TSetData> Members

        void IComponentSet<TSet, TSetData>.GetOrAddComponents(
            TSetData data,
            GameObject setParent,
            string setName)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                GetOrAddComponents(data, setParent, setName);
            }
        }

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

        #endregion

        #region Nested type: List

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
    }
}
