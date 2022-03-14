using Appalachia.Core.ControlModel.Contracts;
using UnityEngine;

namespace Appalachia.Core.ControlModel.ComponentGroups
{
    public abstract partial class AppaComponentGroupConfig<TGroup, TConfig> : IAdvancedInspector
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _showAdvancedOptions;

        #endregion

        #region IInspectorVisibility Members

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
