#region

using System;
using Appalachia.Core.Collections.Implementations.Lists;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Special
{
    [Serializable]
    public sealed class DirtyStringCollection : IsDirtyCollection<string, AppaList_string>
    {
        protected override string GetDisplayTitle(string key, bool value)
        {
            return key;
        }

        protected override string GetDisplaySubtitle(string key, bool value)
        {
            return value.ToString();
        }

        protected override Color GetDisplayColor(string key, bool value)
        {
            return Color.white;
        }
    }
}
