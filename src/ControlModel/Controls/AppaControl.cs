using System;
using Appalachia.Core.ControlModel.Controls.Contracts;
using Appalachia.Core.ControlModel.Controls.Model;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Controls
{
    [Serializable]
    [FoldoutGroup("Components", false)]
    public abstract partial class AppaControl<TControl, TConfig> : AppalachiaBehaviour<TControl>,
                                                                   IAppaControl<TControl, TConfig>
        where TControl : AppaControl<TControl, TConfig>
        where TConfig : AppaControlConfig<TControl, TConfig>, new()
    {
        #region Constants and Static Readonly

        protected const int ORDER_ELEMENTS = ORDER_ROOT + 20;
        protected const int ORDER_OBJECTS = ORDER_ELEMENTS + 20;

        protected const int ORDER_ROOT = -250;

        #endregion

        #region Fields and Autoproperties

        [PropertyOrder(ORDER_ROOT - 50)]
        [SerializeField]
        public bool isSortingDisabled;

        [SerializeField] private TConfig _config;

        [SerializeField, HideInInspector]
        private string _namePrefix;

        #endregion

        public static string NamePostfix => typeof(TControl).Name;

        public abstract ControlSorting DesiredComponentOrder { get; }

        public virtual bool IsUI => false;

        public string NamePrefix
        {
            get => _namePrefix;
            set => _namePrefix = value;
        }

        public abstract void DestroySafely();

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

        public virtual void Enable(TConfig config)
        {
            using (_PRF_Enable.Auto())
            {
                _config = config;

                if (config.ShouldEnable)
                {
                    if (GameObject)
                    {
                        GameObject.SetActive(true);
                    }
                }
            }
        }

        public static void Refresh(ref TControl control, GameObject root)
        {
            using (_PRF_Refresh.Auto())
            {
                root.GetOrAddComponent(ref control);

                control.Refresh();
            }
        }

        public static void Refresh(ref TControl control, GameObject parent, string namePrefix)
        {
            using (_PRF_Refresh.Auto())
            {
                GameObject root = null;
                var fullName = $"{namePrefix} {NamePostfix}";
                parent.GetOrAddChild(ref root, fullName, false);

                Refresh(ref control, root);
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

        #region IAppaControl<TControl,TConfig> Members

        public void ApplyConfig()
        {
            using (_PRF_ApplyConfig.Auto())
            {
                _config.Apply(this as TControl);
            }
        }

        /// <summary>
        ///     Adds the required component groups to the control.
        /// </summary>
        public virtual void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                ValidateSiblingSort();
            }
        }

        string IAppaControl.NamePostfix => NamePostfix;

        public TConfig Config
        {
            get => _config;
            set => _config = value;
        }

        public void DestroySafely(bool includeGameObject)
        {
            DestroySafely();

            if (includeGameObject)
            {
                gameObject.DestroySafely();
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyConfig = new ProfilerMarker(_PRF_PFX + nameof(ApplyConfig));

        protected static readonly ProfilerMarker _PRF_DestroySafely =
            new ProfilerMarker(_PRF_PFX + nameof(DestroySafely));

        protected static readonly ProfilerMarker _PRF_Disable = new ProfilerMarker(_PRF_PFX + nameof(Disable));

        protected static readonly ProfilerMarker _PRF_Enable = new ProfilerMarker(_PRF_PFX + nameof(Enable));

        protected static readonly ProfilerMarker _PRF_OnGetOrAddComponents =
            new ProfilerMarker(_PRF_PFX + nameof(Refresh));

        protected static readonly ProfilerMarker _PRF_Refresh = new ProfilerMarker(_PRF_PFX + nameof(Refresh));

        private static readonly ProfilerMarker _PRF_ValidateSiblingSort =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSiblingSort));

        #endregion
    }
}
