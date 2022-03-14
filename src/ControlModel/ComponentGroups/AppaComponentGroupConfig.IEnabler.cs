using Appalachia.Core.ControlModel.Contracts;
using UnityEngine;

namespace Appalachia.Core.ControlModel.ComponentGroups
{
    public abstract partial class AppaComponentGroupConfig<TGroup, TConfig> : IEnabler
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
