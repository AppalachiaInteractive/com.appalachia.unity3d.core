using System;
using Appalachia.Core.Objects.Components.Contracts;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Colors;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Components.Sets
{
    public abstract partial class ComponentSetData<TComponentSet, TComponentSetData>
    {
        #region Preferences

        [NonSerialized] private PREF<Color> _advancedToggleColorOff;
        [NonSerialized] private PREF<Color> _advancedToggleColorOn;

        [NonSerialized] private PREF<Color> _hideToggleColorOff;
        [NonSerialized] private PREF<Color> _hideToggleColorOn;
        [NonSerialized] private PREF<Color> _lockToggleColorOff;
        [NonSerialized] private PREF<Color> _lockToggleColorOn;
        [NonSerialized] private PREF<Color> _showToggleColorOff;
        [NonSerialized] private PREF<Color> _showToggleColorOn;
        [NonSerialized] private PREF<Color> _suspendToggleColorOff;
        [NonSerialized] private PREF<Color> _suspendToggleColorOn;

        private PREF<Color> AdvancedToggleColorOff
        {
            get
            {
                if (_advancedToggleColorOff == null)
                {
                    _advancedToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOff;
            }
        }

        private PREF<Color> AdvancedToggleColorOn
        {
            get
            {
                if (_advancedToggleColorOn == null)
                {
                    _advancedToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(AdvancedToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _advancedToggleColorOn;
            }
        }

        private PREF<Color> HideToggleColorOff
        {
            get
            {
                if (_hideToggleColorOff == null)
                {
                    _hideToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOff;
            }
        }

        private PREF<Color> HideToggleColorOn
        {
            get
            {
                if (_hideToggleColorOn == null)
                {
                    _hideToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(HideToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _hideToggleColorOn;
            }
        }

        private PREF<Color> LockToggleColorOff
        {
            get
            {
                if (_lockToggleColorOff == null)
                {
                    _lockToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOff;
            }
        }

        private PREF<Color> LockToggleColorOn
        {
            get
            {
                if (_lockToggleColorOn == null)
                {
                    _lockToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(LockToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _lockToggleColorOn;
            }
        }

        private PREF<Color> ShowToggleColorOff
        {
            get
            {
                if (_showToggleColorOff == null)
                {
                    _showToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOff;
            }
        }

        private PREF<Color> ShowToggleColorOn
        {
            get
            {
                if (_showToggleColorOn == null)
                {
                    _showToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(ShowToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _showToggleColorOn;
            }
        }

        private PREF<Color> SuspendToggleColorOff
        {
            get
            {
                if (_suspendToggleColorOff == null)
                {
                    _suspendToggleColorOff = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOff),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOff;
            }
        }

        private PREF<Color> SuspendToggleColorOn
        {
            get
            {
                if (_suspendToggleColorOn == null)
                {
                    _suspendToggleColorOn = PREFS.REG(
                        PKG.Prefs.Colors.Group,
                        nameof(SuspendToggleColorOn),
                        Colors.WhiteSmokeGray96
                    );
                }

                return _suspendToggleColorOn;
            }
        }

        #endregion

        public override void InitializePreferences()
        {
            using (_PRF_InitializePreferences.Auto())
            {
                base.InitializePreferences();

                _advancedToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(AdvancedToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _advancedToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(AdvancedToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _hideToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(HideToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _hideToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(HideToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _lockToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(LockToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _lockToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(LockToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _showToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(ShowToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _showToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(ShowToggleColorOn),
                    Colors.WhiteSmokeGray96
                );

                _suspendToggleColorOff = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(SuspendToggleColorOff),
                    Colors.WhiteSmokeGray96
                );

                _suspendToggleColorOn = PREFS.REG(
                    PKG.Prefs.Colors.Group,
                    nameof(SuspendToggleColorOn),
                    Colors.WhiteSmokeGray96
                );
            }
        }

        #region IFieldLockable Members

        public Color LockToggleColor => DisableAllFields ? LockToggleColorOn.v : LockToggleColorOff.v;

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

        public Color SuspendToggleColor => SuspendFieldApplication ? SuspendToggleColorOn.v : SuspendToggleColorOff.v;

        #endregion

        #region IInspectorVisibility Members

        public bool ShowAdvancedOptions
        {
            get =>
                _showAdvancedOptions || HideAllFields || ShowAllFields || SuspendFieldApplication || DisableAllFields;
            set => _showAdvancedOptions = value;
        }

        public Color AdvancedToggleColor => ShowAdvancedOptions ? AdvancedToggleColorOn.v : AdvancedToggleColorOff.v;

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

        #region IRemotelyEnabled Members

        public Func<bool> ShouldEnableFunction
        {
            get => _shouldEnableFunction;
            set => _shouldEnableFunction = value;
        }

        public bool ShouldEnable => _shouldEnableFunction?.Invoke() ?? true;

        public void BindEnabledStateTo(IRemotelyEnabledController controller)
        {
            using (_PRF_BindEnabledStateTo.Auto())
            {
                ShouldEnableFunction = () => controller.ShouldEnable;
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_BindEnabledStateTo =
            new ProfilerMarker(_PRF_PFX + nameof(BindEnabledStateTo));

        #endregion
    }
}
