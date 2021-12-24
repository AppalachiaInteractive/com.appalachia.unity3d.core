using System;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Core.Collections.Implementations.Lookups
{
    [Serializable]
    public class FontLookup : AppaLookup<string, Font, stringList, FontList>
    {
        protected override Color GetDisplayColor(string key, Font value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(string key, Font value)
        {
            return value.name;
        }

        protected override string GetDisplayTitle(string key, Font value)
        {
            return key;
        }
    }
}
