using UnityEngine;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IAdvancedInspector
    {
        bool ShowAdvancedOptions { get; set; }
        Color AdvancedToggleColor { get; }
    }
}
