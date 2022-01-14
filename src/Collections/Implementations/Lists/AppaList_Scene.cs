#region

using System;
using UnityEngine.SceneManagement;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_Scene : AppaList<Scene>
    {
        public AppaList_Scene()
        {
        }

        public AppaList_Scene(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Scene(AppaList<Scene> list) : base(list)
        {
        }

        public AppaList_Scene(Scene[] values) : base(values)
        {
        }
    }
}
