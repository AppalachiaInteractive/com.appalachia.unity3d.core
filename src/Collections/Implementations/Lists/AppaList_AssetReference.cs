using System;
using UnityEngine.AddressableAssets;

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_AssetReference : AppaList<AssetReference>
    {
        public AppaList_AssetReference()
        {
        }

        public AppaList_AssetReference(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_AssetReference(AppaList<AssetReference> list) : base(list)
        {
        }

        public AppaList_AssetReference(AssetReference[] values) : base(values)
        {
        }
    }
}
