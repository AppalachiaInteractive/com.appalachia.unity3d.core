#region

using System;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Utility.Strings;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lookups
{
    [Serializable]
    public class GameObjectLookup : AppaLookup<int, GameObject, intList, AppaList_GameObject>
    {
        /// <inheritdoc />
        protected override Color GetDisplayColor(int key, GameObject value)
        {
            return Color.white;
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(int key, GameObject value)
        {
            return ZString.Format("InstancedID {0})", key);
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(int key, GameObject value)
        {
            return value == null ? "MISSING" : value.name;
        }
    }
}
