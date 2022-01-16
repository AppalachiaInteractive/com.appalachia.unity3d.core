using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Sets
{
    [Serializable]
    [InlineProperty, FoldoutGroup("Components"), HideLabel]
    public abstract class ComponentSetMetadata<TSet, TSetMetadata> : AppalachiaObject<TSetMetadata>,
                                                                     IComponentSetMetadata<TSet, TSetMetadata>
        where TSet : ComponentSet<TSet, TSetMetadata>
        where TSetMetadata : ComponentSetMetadata<TSet, TSetMetadata>,
        IComponentSetMetadata<TSet, TSetMetadata>
    {
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

        public virtual void ConfigureComponents(TSet componentSet)
        {
            using (_PRF_ConfigureComponents.Auto())
            {
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_ConfigureComponents =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureComponents));

        protected static readonly ProfilerMarker _PRF_InitializeMetadata =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeMetadata));

        #endregion
    }
}
