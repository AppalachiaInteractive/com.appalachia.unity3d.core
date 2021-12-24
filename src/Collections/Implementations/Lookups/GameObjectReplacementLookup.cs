#region

using System;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Utility.Strings;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lookups
{
    [Serializable]
    public class GameObjectReplacementLookup : AppaLookup<GameObject, GameObject,
        AppaList_GameObject, AppaList_GameObject>
    {
        protected override Color GetDisplayColor(GameObject key, GameObject value)
        {
            return Color.white;
        }

        protected override string GetDisplaySubtitle(GameObject key, GameObject value)
        {
            return ZString.Format("Replacement: {0}", value.name);
        }

        protected override string GetDisplayTitle(GameObject key, GameObject value)
        {
            return ZString.Format("Replacing: {0}", key.name);
        }
    }
}
