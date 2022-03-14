using System;
using Appalachia.Core.ControlModel.Contracts;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Components
{
    public abstract partial class AppaComponentConfig<TComponent, TConfig> : IEnabler
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _enabled;

        #endregion

        #region IEnabler Members

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public Color EnabledToggleColor => Enabled ? EnabledToggleColorOn.v : EnabledToggleColorOff.v;

        #endregion
    }
}
