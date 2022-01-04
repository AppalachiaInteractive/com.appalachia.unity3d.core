using System;
using Appalachia.Core.Collections;
using Appalachia.Core.Objects.Models;

namespace Appalachia.Core.Objects.Collections
{
    [Serializable]
    public sealed class
        AppalachiaRepositoryPrefabReferenceList : AppaList<AppalachiaRepositoryPrefabReference>
    {
        public AppalachiaRepositoryPrefabReferenceList()
        {
        }

        public AppalachiaRepositoryPrefabReferenceList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppalachiaRepositoryPrefabReferenceList(
            AppaList<AppalachiaRepositoryPrefabReference> list) : base(list)
        {
        }

        public AppalachiaRepositoryPrefabReferenceList(AppalachiaRepositoryPrefabReference[] values) : base(
            values
        )
        {
        }
    }
}
