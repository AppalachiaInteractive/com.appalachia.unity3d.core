using System;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Utility.Colors;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Appalachia.Core.Collections.Implementations.Lookups
{
    [Serializable]
    public class AppaLookup_FontAsset : AppaLookup<string, FontAsset, stringList, AppaList_FontAsset>
    {
        protected override Color GetDisplayColor(string key, FontAsset value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(string key, FontAsset value)
        {
            return value.version;
        }

        protected override string GetDisplayTitle(string key, FontAsset value)
        {
            return key;
        }
    }
}
