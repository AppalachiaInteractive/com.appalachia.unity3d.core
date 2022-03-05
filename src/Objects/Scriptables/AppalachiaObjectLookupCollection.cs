using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Collections;
using Appalachia.Core.Collections.Interfaces;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
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
    [AssetLabel(Brand.AppalachiaObjectLookupCollection.Label)]
    public abstract partial class AppalachiaObjectLookupCollection<
        TKey, TValue, TKeyList, TValueList, TLookup, T> : AppalachiaObject<T>
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
            if (_items.TryGetValue(key, out var result)) return result;

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
                var removed = 0;

                for (var i = _items.Count - 1; i >= 0; i--)
                {
                    var item = _items.at[i];

                    if (!IsValid(item))
                    {
                        _items.RemoveAt(i);
                        removed += 1;
                    }
                }

                if (removed > 0)
                {
                    MarkAsModified();
                }
            }
        }

        public void RemoveNulls()
        {
            using (_PRF_RemoveNulls.Auto())
            {
                _items.RemoveNulls();
            }
        }

        protected abstract TKey GetUniqueKeyFromValue(TValue value);

        // ReSharper disable once UnusedParameter.Global
        protected virtual bool IsValid(TValue element)
        {
            return true;
        }

        /// <inheritdoc />
        protected override string GetBackgroundColor()
        {
            return Brand.AppalachiaObjectLookupCollection.Banner;
        }

        /// <inheritdoc />
        protected override string GetFallbackTitle()
        {
            return Brand.AppalachiaObjectLookupCollection.Fallback;
        }

        /// <inheritdoc />
        protected override string GetTitle()
        {
            return Brand.AppalachiaObjectLookupCollection.Text;
        }

        /// <inheritdoc />
        protected override string GetTitleColor()
        {
            return Brand.AppalachiaObjectLookupCollection.Color;
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                if (_items == null)
                {
                    _items = new TLookup();
                }

                _items.Changed.Event += OnChanged;

#if UNITY_EDITOR
                InitializeInEditor();
#endif
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Clear = new ProfilerMarker(_PRF_PFX + nameof(Clear));
        private static readonly ProfilerMarker _PRF_DoForAll = new(_PRF_PFX + nameof(DoForAll));

        private static readonly ProfilerMarker _PRF_DoForAllIf = new(_PRF_PFX + nameof(DoForAllIf));

        private static readonly ProfilerMarker _PRF_Items = new(_PRF_PFX + nameof(Items));

        private static readonly ProfilerMarker _PRF_RemoveByKey =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveByKey));

        private static readonly ProfilerMarker _PRF_RemoveInvalid = new(_PRF_PFX + nameof(RemoveInvalid));

        private static readonly ProfilerMarker _PRF_RemoveNulls =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveNulls));

        #endregion
    }
}
