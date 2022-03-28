using System;
using Appalachia.Core.ControlModel.Controls.Contracts;
using Appalachia.Core.ControlModel.Controls.Model;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Controls
{
    [Serializable]
    [FoldoutGroup("Components", false)]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public abstract partial class AppaControl<TControl, TConfig> : AppalachiaBehaviour<TControl>,
                                                                   IAppaControl<TControl, TConfig>
        where TControl : AppaControl<TControl, TConfig>
        where TConfig : AppaControlConfig<TControl, TConfig>, new()
    {
        #region Constants and Static Readonly

        protected const int ORDER_ELEMENTS = ORDER_ROOT + 20;
        protected const int ORDER_OBJECTS = ORDER_ELEMENTS + 20;

        protected const int ORDER_ROOT = -250;
        protected const string GROUP_COMP = "Components";
        protected const string GROUP_STATE = "State";

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private TConfig _config;

        [FoldoutGroup(GROUP_COMP)]
        [SerializeField, HideInInspector]
        private string _namePrefix;

        #endregion

        public static string NamePostfix => typeof(TControl).Name;

        public virtual bool IsUI => false;

        public virtual ControlSorting DesiredComponentOrder => ControlSorting.Anywhere;

        public string NamePrefix
        {
            get => _namePrefix;
            set => _namePrefix = value;
        }

        public virtual void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                gameObject.DestroySafely(false);
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
                var root = control == null ? null : control.gameObject;
                var fullName = AppaControlNamer.GetStyledName(namePrefix, NamePostfix);
                parent.GetOrAddChild(ref root, fullName, false);

                Refresh(ref control, root);
            }
        }

        protected abstract void OnRefresh();

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                Refresh();
            }
        }

        protected override void OnChanged()
        {
            using (_PRF_OnChanged.Auto())
            {
                base.OnChanged();

                Config?.Apply(this as TControl);
            }
        }
        
        #region IAppaControl<TControl,TConfig> Members

        [ButtonGroup(nameof(IAppaControl))]
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
        public void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                OnRefresh();
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

        protected static readonly ProfilerMarker _PRF_OnRefresh = new ProfilerMarker(_PRF_PFX + nameof(OnRefresh));

        protected static readonly ProfilerMarker _PRF_Refresh = new ProfilerMarker(_PRF_PFX + nameof(Refresh));

        #endregion
    }
}
