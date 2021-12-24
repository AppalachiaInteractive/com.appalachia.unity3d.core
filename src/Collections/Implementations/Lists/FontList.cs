using System;
using UnityEngine;

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class FontList : AppaList<Font>
    {
        public FontList()
        {
        }

        public FontList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public FontList(AppaList<Font> list) : base(list)
        {
        }

        public FontList(Font[] values) : base(values)
        {
        }
    }
}
