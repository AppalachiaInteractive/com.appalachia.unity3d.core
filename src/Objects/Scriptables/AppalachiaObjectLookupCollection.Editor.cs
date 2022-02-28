#if UNITY_EDITOR
using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Scriptables
{
    public abstract partial class AppalachiaObjectLookupCollection<
        TKey, TValue, TKeyList, TValueList, TLookup, T>
    {
        private void InitializeInEditor()
        {
            AppaTask.FireAndForget(PopulateItems);
        }

        [ButtonGroup]
        private void PopulateItems()
        {
            using (_PRF_PopulateItems.Auto())
            {
                if (_items == null)
                {
                    _items = new TLookup();
                    MarkAsModified();
                }

                _items.Changed.Event += OnChanged;

                for (var i = _items.Count - 1; i >= 0; i--)
                {
                    if (_items.at[i] == null)
                    {
                        _items.RemoveAt(i);
                        MarkAsModified();
                    }
                }

                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    var assets = AssetDatabaseManager.FindAssets<TValue>();

                    for (var index = 0; index < assets.Count; index++)
                    {
                        var asset = assets[index];

                        if (asset == null)
                        {
                            continue;
                        }

                        var assetKey = GetUniqueKeyFromValue(asset);

                        if (_items.ContainsKey(assetKey))
                        {
                            if (_items[assetKey] == null)
                            {
                                _items[assetKey] = asset;
                                MarkAsModified();
                            }
                        }
                        else
                        {
                            _items.Add(assetKey, asset);
                            MarkAsModified();
                        }
                    }
                }
            }
        }

        [ButtonGroup]
        private void RepopulateItems()
        {
            using (_PRF_RepopulateItems.Auto())
            {
                _items.Clear();
                PopulateItems();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_PopulateItems = new(_PRF_PFX + nameof(PopulateItems));

        private static readonly ProfilerMarker _PRF_RepopulateItems =
            new ProfilerMarker(_PRF_PFX + nameof(RepopulateItems));

        #endregion
    }
}

#endif