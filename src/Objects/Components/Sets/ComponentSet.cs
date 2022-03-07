using System;
using System.Diagnostics;
using System.Linq;
using Appalachia.Core.Collections;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Objects.Components.Sets
{
    [Serializable]
    [FoldoutGroup("Components", false)]
    [DebuggerDisplay("{Name} (ComponentSet)")]
    public abstract partial class ComponentSet<TComponentSet, TComponentSetData> : AppalachiaSimpleBase,
        IComponentSet<TComponentSet, TComponentSetData>
        where TComponentSet : ComponentSet<TComponentSet, TComponentSetData>, new()
        where TComponentSetData : ComponentSetData<TComponentSet, TComponentSetData>, new()
    {
        #region Constants and Static Readonly

        protected const string SET_GAME_OBJECT_NAME_FORMAT = "{0} - {{0}}";

        #endregion

        #region Fields and Autoproperties

        [PropertyOrder(-250)]
        [SerializeField]
        [ReadOnly]
        private GameObject gameObject;

        [FormerlySerializedAs("_initialer")]
        [SerializeField, HideInInspector]
        private Initializer _initializer;

        [PropertyOrder(-300)]
        [SerializeField]
        public bool isSortingDisabled;

        private string _name1;

        #endregion

        public abstract ComponentSetSorting DesiredComponentOrder { get; }

        protected abstract bool IsUI { get; }

        public override string Name => GameObject.name;

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

        protected string GameObjectNamePostfix
        {
            get
            {
                var splits = gameObject.name.Split('-');
                var lastNamePart = splits.LastOrDefault();
                return lastNamePart?.Trim();
            }
        }

        public virtual void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                if (GameObject)
                {
                    GameObject.DestroySafely();
                }
            }
        }

        public virtual void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                if (GameObject)
                {
                    GameObject.SetActive(false);
                }
            }
        }

        public virtual void Enable(TComponentSetData data)
        {
            using (_PRF_Enable.Auto())
            {
                if (data.ShouldEnable)
                {
                    if (GameObject)
                    {
                        GameObject.SetActive(true);
                    }
                }
            }
        }

        public static void GetOrAddComponents(ref TComponentSet set, GameObject setRoot)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                set ??= new TComponentSet();

                set.GetOrAddComponents(setRoot);
            }
        }

        public static void GetOrAddComponents(ref TComponentSet set, GameObject setParent, string gameObjectNamePostfix)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                set ??= new TComponentSet();

                set.GetOrAddComponents(setParent, gameObjectNamePostfix);
            }
        }

        /// <summary>
        ///     Adds the required components to the set.  The <see cref="GameObject" /> will have been created at this point.
        /// </summary>
        /// <param name="data">The component set data.</param>
        protected abstract void OnGetOrAddComponents();

        protected virtual string GetComponentSetNamePrefixFormat()
        {
            using (_PRF_GetGameObjectNameFormat.Auto())
            {
                // GAME_OBJECT_NAME_FORMAT = "{0} - {{0}}";
                // ComponentSetName = "ABCDEFG";
                return ZString.Format(SET_GAME_OBJECT_NAME_FORMAT, ComponentSetNamePrefix);

                // ABCDERG - {0}
            }
        }

        /// <summary>
        ///     Creates the set on the <paramref name="setRoot" /> by adding the required components.
        /// </summary>
        /// <param name="setRoot">The root of the set.</param>
        protected virtual void GetOrAddComponents(GameObject setRoot)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                gameObject = setRoot;

                OnGetOrAddComponents();
                ValidateSiblingSort();
            }
        }

        /// <summary>
        ///     Creates the set underneath the <paramref name="setParent" />, using the provided <paramref name="gameObjectNamePostfix" />, and adds the required
        ///     components.
        /// </summary>
        /// <param name="setParent">The parent of the set.</param>
        /// <param name="gameObjectNamePostfix">The name of the set.</param>
        protected virtual void GetOrAddComponents(GameObject setParent, string gameObjectNamePostfix)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                var prefixNameFormatString = GetComponentSetNamePrefixFormat();

                var setObjectName = ZString.Format(prefixNameFormatString, gameObjectNamePostfix);

                if (gameObject == null)
                {
                    setParent.GetOrAddChild(ref gameObject, setObjectName, IsUI);
                }

                gameObject.name = setObjectName;

                OnGetOrAddComponents();
                ValidateSiblingSort();
            }
        }

        private int GetDesiredSiblingIndex(int currentIndex, int siblingCount)
        {
            if (isSortingDisabled)
            {
                return currentIndex;
            }

            return SiblingIndexCalculator.GetDesiredSiblingIndex(DesiredComponentOrder, currentIndex, siblingCount);
        }

        private void ValidateSiblingSort()
        {
            using (_PRF_ValidateSiblingSort.Auto())
            {
                var siblingCount = gameObject.transform.parent.childCount - 1;

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

        #region IComponentSet<TComponentSet,TComponentSetData> Members

        void IComponentSet<TComponentSet, TComponentSetData>.GetOrAddComponents(
            GameObject setParent,
            string gameObjectNamePostfix)
        {
            using (_PRF_GetOrAddComponents.Auto())
            {
                GetOrAddComponents(setParent, gameObjectNamePostfix);
            }
        }

        public abstract string ComponentSetNamePrefix { get; }

        public GameObject GameObject => gameObject;

        #endregion

        #region Nested type: List

        [Serializable]
        public sealed class List : AppaList<TComponentSet>
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

            public List(AppaList<TComponentSet> list) : base(list)
            {
            }

            public List(TComponentSet[] values) : base(values)
            {
            }
        }

        #endregion

        #region Profiling

        protected static readonly string _PRF_PFX = typeof(TComponentSet).Name + ".";

        protected static readonly ProfilerMarker _PRF_DestroySafely =
            new ProfilerMarker(_PRF_PFX + nameof(DestroySafely));

        protected static readonly ProfilerMarker _PRF_Disable = new ProfilerMarker(_PRF_PFX + nameof(Disable));

        protected static readonly ProfilerMarker _PRF_Enable = new ProfilerMarker(_PRF_PFX + nameof(Enable));

        private static readonly ProfilerMarker _PRF_GetGameObjectNameFormat =
            new ProfilerMarker(_PRF_PFX + nameof(GetComponentSetNamePrefixFormat));

        protected static readonly ProfilerMarker _PRF_GetOrAddComponents =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrAddComponents));

        protected static readonly ProfilerMarker _PRF_OnGetOrAddComponents =
            new ProfilerMarker(_PRF_PFX + nameof(OnGetOrAddComponents));

        private static readonly ProfilerMarker _PRF_ValidateSiblingSort =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSiblingSort));

        #endregion
    }
}
