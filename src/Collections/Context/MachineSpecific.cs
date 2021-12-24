using System;
using UnityEngine;

namespace Appalachia.Core.Collections.Context
{
    [Serializable]
    public abstract class MachineSpecific<T, TList> : ContextSpecific<T, TList>
        where TList : AppaList<T>, new()
        where T : ScriptableObject
    {
        public override string GetContextKey()
        {
            var machineName = Environment.MachineName;

            return machineName;
        }
    }
}
