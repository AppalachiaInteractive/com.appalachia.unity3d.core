using Appalachia.Core.ControlModel.Contracts;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Controls
{
    public abstract partial class AppaControlConfig<TControl, TConfig> : IAdvancedInspector
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        #endregion

        #region IFieldLockable Members

        public bool ShowAdvancedOptions
        {
            get =>
                _showAdvancedOptions || HideAllFields || ShowAllFields || SuspendFieldApplication || DisableAllFields;
            set => _showAdvancedOptions = value;
        }

        public Color AdvancedToggleColor => ShowAdvancedOptions ? AdvancedToggleColorOn.v : AdvancedToggleColorOff.v;

        #endregion
    }
}
