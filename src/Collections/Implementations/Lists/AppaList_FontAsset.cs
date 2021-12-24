using System;
using UnityEngine.TextCore.Text;

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_FontAsset : AppaList<FontAsset>
    {
        public AppaList_FontAsset()
        {
        }

        public AppaList_FontAsset(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_FontAsset(AppaList<FontAsset> list) : base(list)
        {
        }

        public AppaList_FontAsset(FontAsset[] values) : base(values)
        {
        }
    }
}
