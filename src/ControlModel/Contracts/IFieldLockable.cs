using UnityEngine;

namespace Appalachia.Core.ControlModel.Contracts
{
    public interface IFieldLockable : IAdvancedInspector
    {
        Color LockToggleColor { get; }
        Color SuspendToggleColor { get; }
        bool DisableAllFields { get; set; }
        bool SuspendFieldApplication { get; set; }
    }
}
