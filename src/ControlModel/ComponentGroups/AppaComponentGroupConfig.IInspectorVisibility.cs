using System;
using Appalachia.Core.ControlModel.Contracts;
using UnityEngine;

namespace Appalachia.Core.ControlModel.ComponentGroups
{
    public abstract partial class AppaComponentGroupConfig<TGroup, TConfig> : IInspectorVisibility
    {
        #region Fields and Autoproperties

        [NonSerialized] private bool _showAllFields;

        [SerializeField, HideInInspector]
        private bool _hideAllFields;

        #endregion

        #region IInspectorVisibility Members

        public Color ShowToggleColor => ShowAllFields ? ShowToggleColorOn.v : ShowToggleColorOff.v;
        public Color HideToggleColor => HideAllFields ? HideToggleColorOn.v : HideToggleColorOff.v;

        public bool HideAllFields
        {
            get => _hideAllFields && !_showAllFields;
            set => _hideAllFields = value;
        }

        public bool ShowAllFields
        {
            get => _showAllFields;
            set => _showAllFields = value;
        }

        #endregion
    }
}
