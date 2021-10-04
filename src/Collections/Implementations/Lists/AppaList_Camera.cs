#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_Camera : AppaList<Camera>
    {
        public AppaList_Camera()
        {
        }

        public AppaList_Camera(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Camera(AppaList<Camera> list) : base(list)
        {
        }

        public AppaList_Camera(Camera[] values) : base(values)
        {
        }
    }
}
