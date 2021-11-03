using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Scriptables
{
    public abstract class AppalachiaMetadataCollection<T, TValue> : SingletonAppalachiaObject<T>
        where T : AppalachiaMetadataCollection<T, TValue>
        where TValue : AppalachiaObject<TValue>, ICategorizable
    {
        [FormerlySerializedAs("generic")]
        [FormerlySerializedAs("defaultWrapper")]
        [FoldoutGroup("Default")]
        public TValue defaultValue;

        [NonSerialized]
        [HideInInspector]
        private List<TValue> _all;

        public IReadOnlyList<TValue> all
        {
            get
            {
                if ((_all != null) && (_all.Count > 0))
                {
                    return _all;
                }

                _all = new List<TValue>();

                PopulateAll(_all);

                return _all;
            }
        }

        protected List<TValue> all_internal
        {
            get
            {
                if ((_all != null) && (_all.Count > 0))
                {
                    return _all;
                }

                _all = new List<TValue>();

                PopulateAll(_all);

                return _all;
            }
        }

        protected virtual void RegisterNecessaryInstances()
        {
        }

        protected void PopulateAll(List<TValue> values)
        {
#if UNITY_EDITOR
            var assets = AssetDatabaseManager.FindAssets<TValue>();

            for (var i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];

                values.Add(asset);
            }
#endif
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
    }
}
