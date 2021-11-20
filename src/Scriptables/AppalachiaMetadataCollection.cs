using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Collections;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Scriptables
{
    public abstract class AppalachiaMetadataCollection<T, TValue, TL> : SingletonAppalachiaObject<T>
        where T : AppalachiaMetadataCollection<T, TValue, TL>
        where TValue : AppalachiaObject, ICategorizable
        where TL : AppaList<TValue>, new()
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("generic")]
        [FormerlySerializedAs("defaultWrapper")]
        [FoldoutGroup("Default")]
        public TValue defaultValue;

        [NonSerialized]
        [HideInInspector]
        private TL _all;

        #endregion

        public IReadOnlyList<TValue> all
        {
            get
            {
                if ((_all != null) && (_all.Count > 0))
                {
                    return _all;
                }

                _all = new TL();

#if UNITY_EDITOR
                PopulateAll(_all);
#endif

                return _all;
            }
        }

        protected TL all_internal
        {
            get
            {
                if ((_all != null) && (_all.Count > 0))
                {
                    return _all;
                }

                _all = new TL();

#if UNITY_EDITOR
                PopulateAll(_all);
#endif

                return _all;
            }
        }

        protected override void WhenEnabled()
        {
            if (defaultValue == null)
            {
                defaultValue = all_internal.FirstOrDefault_NoAlloc();
            }

#if UNITY_EDITOR
            using (new AssetEditingScope())
            {
                RegisterNecessaryInstances();
            }
#endif
        }

#if UNITY_EDITOR
        protected virtual void RegisterNecessaryInstances()
        {
        }

        protected void PopulateAll(TL values)
        {
            values.Clear();

            var assets = AssetDatabaseManager.FindAssets<TValue>();

            for (var i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];

                values.Add(asset);
            }
        }
#endif
    }
}
