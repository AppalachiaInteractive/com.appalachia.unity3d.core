#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_Vector3 : AppaList<Vector3>
    {
        public AppaList_Vector3()
        {
        }

        public AppaList_Vector3(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Vector3(AppaList<Vector3> list) : base(list)
        {
        }

        public AppaList_Vector3(Vector3[] values) : base(values)
        {
        }
    }
}
