#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_AudioChorusFilter : AppaList<AudioChorusFilter>
    {
        public AppaList_AudioChorusFilter()
        {
        }

        public AppaList_AudioChorusFilter(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_AudioChorusFilter(AppaList<AudioChorusFilter> list) : base(list)
        {
        }

        public AppaList_AudioChorusFilter(AudioChorusFilter[] values) : base(values)
        {
        }
    }
}
