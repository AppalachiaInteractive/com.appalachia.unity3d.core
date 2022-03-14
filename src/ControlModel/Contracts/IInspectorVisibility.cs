using UnityEngine;

namespace Appalachia.Core.ControlModel.Contracts
{
    public interface IInspectorVisibility : IAdvancedInspector
    {
        Color HideToggleColor { get; }
        Color ShowToggleColor { get; }
        bool HideAllFields { get; set; }
        bool ShowAllFields { get; set; }
    }
}
