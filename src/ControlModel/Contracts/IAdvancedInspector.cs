using UnityEngine;

namespace Appalachia.Core.ControlModel.Contracts
{
    public interface IAdvancedInspector
    {
        Color AdvancedToggleColor { get; }
        bool ShowAdvancedOptions { get; set; }
    }
}
