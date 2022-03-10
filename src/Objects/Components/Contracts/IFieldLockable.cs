using UnityEngine;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IFieldLockable : IAdvancedInspector
    {
        Color LockToggleColor { get; }
        Color SuspendToggleColor { get; }
        bool DisableAllFields { get; set; }
        bool SuspendFieldApplication { get; set; }
    }
}
