using Appalachia.Core.ControlModel.Contracts;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Controls
{
    public abstract partial class AppaControlConfig<TControl, TConfig> : IFieldLockable
    {
        #region Fields and Autoproperties

        [SerializeField, HideInInspector]
        private bool _disableAllFields;

        [SerializeField, HideInInspector]
        private bool _suspendFieldApplication;

        #endregion

        #region IFieldLockable Members

        public Color LockToggleColor => DisableAllFields ? LockToggleColorOn.v : LockToggleColorOff.v;
        public Color SuspendToggleColor => SuspendFieldApplication ? SuspendToggleColorOn.v : SuspendToggleColorOff.v;

        public bool DisableAllFields
        {
            get => _disableAllFields;
            set => _disableAllFields = value;
        }

        public bool SuspendFieldApplication
        {
            get => _suspendFieldApplication;
            set => _suspendFieldApplication = value;
        }

        #endregion
    }
}
