using UnityEngine;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IAdvancedInspector
    {
        Color AdvancedToggleColor { get; }
        bool ShowAdvancedOptions { get; set; }
    }
}
