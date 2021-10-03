#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_AudioDistortionFilter : AppaList<AudioDistortionFilter>
    {
        public AppaList_AudioDistortionFilter()
        {
        }

        public AppaList_AudioDistortionFilter(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_AudioDistortionFilter(AppaList<AudioDistortionFilter> list) : base(list)
        {
        }

        public AppaList_AudioDistortionFilter(AudioDistortionFilter[] values) : base(values)
        {
        }
    }
}
