using System;
using UnityEngine.AddressableAssets;

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AssetReferenceList : AppaList<AssetReference>
    {
        public AssetReferenceList()
        {
        }

        public AssetReferenceList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AssetReferenceList(AppaList<AssetReference> list) : base(list)
        {
        }

        public AssetReferenceList(AssetReference[] values) : base(values)
        {
        }
    }
}
