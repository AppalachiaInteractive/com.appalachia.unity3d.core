using UnityEngine;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IFieldLockable : IAdvancedInspector
    {
        bool DisableAllFields { get; set; }
        bool SuspendFieldApplication { get; set; }
        
        Color LockToggleColor { get; }
        Color SuspendToggleColor { get; }
    }
}
