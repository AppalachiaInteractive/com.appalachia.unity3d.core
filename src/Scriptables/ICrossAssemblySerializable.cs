#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Scriptables
{
    public interface ICrossAssemblySerializable
    {
        public ScriptableObject GetSerializable();
    }
}
