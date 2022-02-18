using System;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Core.Collections.Implementations.Lookups
{
    [Serializable]
    public class AppaLookup_string : AppaLookup<string, string, stringList, stringList>
    {
        /// <inheritdoc />
        protected override Color GetDisplayColor(string key, string value)
        {
            return Colors.WhiteSmokeGray96;
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(string key, string value)
        {
            return key;
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(string key, string value)
        {
            return value;
        }
    }
}
