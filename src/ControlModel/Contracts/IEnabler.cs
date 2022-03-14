using UnityEngine;

namespace Appalachia.Core.ControlModel.Contracts
{
    public interface IEnabler
    {
        bool Enabled { get; set; }  
        Color EnabledToggleColor { get; }  
    }
}
