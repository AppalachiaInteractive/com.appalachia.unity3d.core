using Appalachia.Core.ControlModel.Contracts;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Components
{
    public abstract partial class AppaComponentConfig<TComponent, TConfig> : IAdvancedInspector
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        #endregion

        #region IAdvancedInspector Members

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
