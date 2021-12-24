using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Collections;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
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
    /// <typeparam name="TThis">A reference to itself, the lookup collection.</typeparam>
    /// <typeparam name="TOLookup">The internal lookup scriptable.</typeparam>
    [Serializable]
    [InspectorIcon(Brand.SingletonAppalachiaObjectLookupCollection.Icon)]
    public abstract class SingletonAppalachiaObjectLookupCollection<
        TKey, TValue, TKeyList, TValueList, TLookup, TOLookup, TThis> : SingletonAppalachiaObject<TThis>
        where TValue : UnityEngine.Object
        where TKeyList : AppaList<TKey>, new()
        where TValueList : AppaList<TValue>, new()
        where TLookup : AppaLookup<TKey, TValue, TKeyList, TValueList>, new()
        where TOLookup :
        AppalachiaObjectLookupCollection<TKey, TValue, TKeyList, TValueList, TLookup, TOLookup>, new()
        where TThis : SingletonAppalachiaObjectLookupCollection<TKey, TValue, TKeyList, TValueList, TLookup,
            TOLookup, TThis>
    {
        #region Fields and Autoproperties

        [SerializeField, InlineProperty, HideLabel, InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        private TOLookup _lookup;

        #endregion

        public TOLookup Lookup => _lookup;

        protected override string GetBackgroundColor()
        {
            return Brand.SingletonAppalachiaObjectLookupCollection.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.SingletonAppalachiaObjectLookupCollection.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.SingletonAppalachiaObjectLookupCollection.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.SingletonAppalachiaObjectLookupCollection.Color;
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
#if UNITY_EDITOR
                if (_lookup == null)
                {
                    _lookup = AppalachiaObjectFactory.LoadExistingOrCreateNewAsset<TOLookup>(
                        ZString.Concat(typeof(TOLookup).Name, "_MainInternal")
                    );
                }
#endif

                await AppaTask.CompletedTask;
            }
        }

        #region Profiling

        private const string _PRF_PFX =
            nameof(SingletonAppalachiaObjectLookupCollection<TKey, TValue, TKeyList, TValueList, TLookup,
                TOLookup, TThis>) +
            ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
