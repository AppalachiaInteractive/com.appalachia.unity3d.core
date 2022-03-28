#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface ICrossAssemblySerializable
    {
        ScriptableObject GetSerializable();
    }
}
