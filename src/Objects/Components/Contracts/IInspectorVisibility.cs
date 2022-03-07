using UnityEngine;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IInspectorVisibility : IAdvancedInspector
    {
        bool HideAllFields { get; set; }
        bool ShowAllFields { get; set; }
        
        Color HideToggleColor { get; }
        Color ShowToggleColor { get; }
    }
}
