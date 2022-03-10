using UnityEngine;

namespace Appalachia.Core.Objects.Components.Contracts
{
    public interface IEnabler
    {
        bool Enabled { get; set; }  
        Color EnabledToggleColor { get; }  
    }
}
