#region

using System;
using Appalachia.Core.Collections.Implementations.Lists;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lookups
{
    [Serializable]
    public class GameObjectLookup : AppaLookup<int, GameObject, AppaList_int, AppaList_GameObject>
    {
        protected override Color GetDisplayColor(int key, GameObject value)
        {
            return Color.white;
        }

        protected override string GetDisplaySubtitle(int key, GameObject value)
        {
            return $"InstancedID {key})";
        }

        protected override string GetDisplayTitle(int key, GameObject value)
        {
            return value == null ? "MISSING" : value.name;
        }
    }
}
