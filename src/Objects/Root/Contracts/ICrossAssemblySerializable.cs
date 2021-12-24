#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface ICrossAssemblySerializable
    {
        public ScriptableObject GetSerializable();
    }
}
