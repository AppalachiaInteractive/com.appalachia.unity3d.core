using System;
using Appalachia.Core.Collections;
using Appalachia.Core.Objects.Models;

namespace Appalachia.Core.Objects.Collections
{
    [Serializable]
    public sealed class
        AppalachiaRepositorySingletonReferenceList : AppaList<AppalachiaRepositorySingletonReference>
    {
        public AppalachiaRepositorySingletonReferenceList()
        {
        }

        public AppalachiaRepositorySingletonReferenceList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppalachiaRepositorySingletonReferenceList(
            AppaList<AppalachiaRepositorySingletonReference> list) : base(list)
        {
        }

        public AppalachiaRepositorySingletonReferenceList(AppalachiaRepositorySingletonReference[] values) :
            base(values)
        {
        }
    }
}
