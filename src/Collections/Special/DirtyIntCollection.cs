#region

using System;
using Appalachia.Core.Collections.Implementations.Lists;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Special
{
    [Serializable]
    public sealed class DirtyIntCollection : IsDirtyCollection<int, intList>
    {
        protected override Color GetDisplayColor(int key, bool value)
        {
            return Color.white;
        }

        protected override string GetDisplaySubtitle(int key, bool value)
        {
            throw new NotImplementedException();
        }

        protected override string GetDisplayTitle(int key, bool value)
        {
            throw new NotImplementedException();
        }
    }
}
