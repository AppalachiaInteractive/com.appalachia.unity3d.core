using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Models;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    [Serializable]
    public abstract class ComponentSetData<TSet, TSetMetadata> : AppalachiaObject<TSetMetadata>,
                                                                 IComponentSetMetadata<TSet, TSetMetadata>
        where TSet : ComponentSet<TSet, TSetMetadata>, new()
        where TSetMetadata : ComponentSetData<TSet, TSetMetadata>, IComponentSetMetadata<TSet, TSetMetadata>
    {
        public static void UpdateComponentSet(
            ref TSet target,
            ref Override data,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSet target,
            ref Optional data,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSet target,
            ref TSetMetadata data,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref Override data,
            ref TSet target,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref Optional data,
            ref TSet target,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        public static void UpdateComponentSet(
            ref TSetMetadata data,
            ref TSet target,
            GameObject parent,
            string setName)
        {
            ComponentSetAPI<TSet, TSetMetadata>.UpdateComponentSet(ref target, ref data, parent, setName);
        }

        protected internal abstract void ApplyMetadataToComponentSet(TSet componentSet);

        [ButtonGroup(GROUP_BUTTONS)]
        protected virtual void InitializeMetadata()
        {
            using (_PRF_InitializeMetadata.Auto())
            {
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                InitializeMetadata();
            }
        }

        #region IComponentSetMetadata<TSet,TSetMetadata> Members

        void IComponentSetMetadata<TSet, TSetMetadata>.ApplyMetadataToComponentSet(TSet componentSet)
        {
            ApplyMetadataToComponentSet(componentSet);
        }

        #endregion

        #region Nested type: Optional

        [Serializable]
        public sealed class Optional : Overridable<TSetMetadata, Optional>
        {
            public Optional() : base(false, default)
            {
            }

            public Optional(bool isElected, TSetMetadata value) : base(isElected, value)
            {
            }

            public Optional(Overridable<TSetMetadata, Optional> value) : base(value)
            {
            }

            public bool IsElected => Overriding;

            protected override string DisabledColorPrefName => "Component Set Optional Disabled Color";
            protected override string EnabledColorPrefName => "Component Set Optional Enabled Color";
            protected override string ToggleLabel => "Optional Set";
        }

        #endregion

        #region Nested type: Override

        [Serializable]
        public class Override : Overridable<TSetMetadata, Override>
        {
            public Override(TSetMetadata value) : base(false, value)
            {
            }

            public Override(bool overrideEnabled, TSetMetadata value) : base(overrideEnabled, value)
            {
            }

            public Override(Overridable<TSetMetadata, Override> value) : base(value)
            {
            }

            public Override() : base(false, default)
            {
            }

            protected override string DisabledColorPrefName => "Component Set Override Disabled Color";
            protected override string EnabledColorPrefName => "Component Set Override Enabled Color";
            protected override string ToggleLabel => "Override Set";
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ApplyMetadataToComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyMetadataToComponentSet));

        protected static readonly ProfilerMarker _PRF_InitializeMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeMetadata));

        private static readonly ProfilerMarker _PRF_PrepareAndConfigure =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        private static readonly ProfilerMarker _PRF_UpdateComponentSet =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateComponentSet));

        #endregion
    }
}
