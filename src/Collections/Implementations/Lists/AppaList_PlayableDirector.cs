#region

using System;
using UnityEngine.Playables;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_PlayableDirector : AppaList<PlayableDirector>
    {
        public AppaList_PlayableDirector()
        {
        }

        public AppaList_PlayableDirector(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_PlayableDirector(AppaList<PlayableDirector> list) : base(list)
        {
        }

        public AppaList_PlayableDirector(PlayableDirector[] values) : base(values)
        {
        }
    }
}
