using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Collections;
using Appalachia.Core.Collections.Interfaces;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Scriptables
{
    /// <summary>
    ///     An key-value lookup collection for an Appalachia scriptable object.
    /// </summary>
    /// <typeparam name="TKey">The key for the lookup.</typeparam>
    /// <typeparam name="TValue">The value for the lookup.</typeparam>
    /// <typeparam name="TKeyList">A serializable list of <see cref="TKey" /></typeparam>
    /// <typeparam name="TValueList">A serializable list of <see cref="TValue" /></typeparam>
    /// <typeparam name="TLookup">A class derived from <see cref="AppaLookup{TKey,TValue,TKeyList,TValueList}" /></typeparam>
    /// <typeparam name="T">This type.</typeparam>
    [Serializable]
    [InspectorIcon(Brand.AppalachiaObjectLookupCollection.Icon)]
    public abstract class
        AppalachiaObjectLookupCollection<TKey, TValue, TKeyList, TValueList, TLookup, T> : AppalachiaObject<T>
        where TValue : UnityEngine.Object
        where TKeyList : AppaList<TKey>, new()
        where TValueList : AppaList<TValue>, new()
        where TLookup : AppaLookup<TKey, TValue, TKeyList, TValueList>, new()
        where T : AppalachiaObjectLookupCollection<TKey, TValue, TKeyList, TValueList, TLookup, T>
    {
        #region Fields and Autoproperties

        [ShowIf(nameof(HasDefault))]
        [SerializeField]
        private TValue _defaultValue;

        [SerializeField]
        [InlineProperty]
        [HideLabel]
        [LabelWidth(0)]
        [ShowInInspector]
        [ListDrawerSettings(
            Expanded = true,
            DraggableItems = false,

            //HideAddButton = true,
            //HideRemoveButton = true,
            NumberOfItemsPerPage = 5
        )]
        protected TLookup _items;

        #endregion

        public abstract bool HasDefault { get; }

        public IAppaLookupSafeUpdates<TKey, TValue, TValueList> Items
        {
            get
            {
                using (_PRF_Items.Auto())
                {
                    return _items;
                }
            }
        }

        public int Count => _items?.Count ?? 0;

        public TValue defaultValue => _defaultValue;

        public TValue this[TKey key] => _items[key];

        public void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                _items.Clear();
            }
        }

        public void DoForAll(Action<TValue> action)
        {
            using (_PRF_DoForAll.Auto())
            {
                var count = Items.Count;

                for (var i = 0; i < count; i++)
                {
                    action(Items.at[i]);
                }
            }
        }

        public void DoForAllIf(Predicate<TValue> doIf, Action<TValue> action)
        {
            using (_PRF_DoForAllIf.Auto())
            {
                var count = Items.Count;

                for (var i = 0; i < count; i++)
                {
                    var set = Items.at[i];

                    if (doIf(set))
                    {
                        action(set);
                    }
                }
            }
        }

        public TValue Find(TKey key)
        {
            if (_items.ContainsKey(key))
            {
                return _items[key];
            }

            return null;
        }

        public TValue GetOrLoadOrCreateNew(TKey key, string name)
        {
            var items = Items;

            if (items.ContainsKey(key))
            {
                return items.Get(key);
            }

            if (typeof(TValue).InheritsFrom(typeof(ScriptableObject)))
            {
                var i = AppalachiaObjectFactory.LoadExistingOrCreateNewAsset(typeof(TValue), name) as TValue;

                items.Add(key, i);
                return i;
            }

            return null;
        }

        public TValue RemoveByKey(TKey key)
        {
            using (_PRF_RemoveByKey.Auto())
            {
                if (_items.ContainsKey(key))
                {
                    return _items.RemoveByKey(key);
                }

                return default;
            }
        }

        public void RemoveInvalid()
        {
            using (_PRF_RemoveInvalid.Auto())
            {
                for (var i = _items.Count - 1; i >= 0; i--)
                {
                    var item = _items.at[i];

                    if (!IsValid(item))
                    {
                        _items.RemoveAt(i);
                    }
                }

#if UNITY_EDITOR
                MarkAsModified();
#endif
            }
        }

        protected abstract TKey GetUniqueKeyFromValue(TValue value);

        // ReSharper disable once UnusedParameter.Global
        protected virtual bool IsValid(TValue element)
        {
            return true;
        }

        protected override string GetBackgroundColor()
        {
            return Brand.AppalachiaObjectLookupCollection.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.AppalachiaObjectLookupCollection.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.AppalachiaObjectLookupCollection.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.AppalachiaObjectLookupCollection.Color;
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                if (_items == null)
                {
                    _items = new TLookup();
                }

                _items.SetObjectOwnership(this);

#if UNITY_EDITOR
                await PopulateItems();
#endif
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(AppalachiaObjectLookupCollection<TKey, TValue, TKeyList, TValueList, TLookup, T>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Clear = new ProfilerMarker(_PRF_PFX + nameof(Clear));

        private static readonly ProfilerMarker _PRF_RemoveByKey =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveByKey));

        private static readonly ProfilerMarker _PRF_Items = new(_PRF_PFX + nameof(Items));
        private static readonly ProfilerMarker _PRF_RemoveInvalid = new(_PRF_PFX + nameof(RemoveInvalid));
        private static readonly ProfilerMarker _PRF_DoForAll = new(_PRF_PFX + nameof(DoForAll));

        private static readonly ProfilerMarker _PRF_DoForAllIf = new(_PRF_PFX + nameof(DoForAllIf));

        #endregion

#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_PopulateItems = new(_PRF_PFX + nameof(PopulateItems));

        [ButtonGroup]
        private void PopulateItemsSynchronous()
        {
            PopulateItems().Forget();
        }

        private async AppaTask PopulateItems()
        {
            using (_PRF_PopulateItems.Auto())
            {
                if (_items == null)
                {
                    _items = new TLookup();
                    MarkAsModified();
                }

                _items.SetObjectOwnership(this);

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
                        await AppaTask.Yield();

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
            _items.Clear();
#pragma warning disable CS4014
            PopulateItems();
#pragma warning restore CS4014
        }
    }
#endif
}
