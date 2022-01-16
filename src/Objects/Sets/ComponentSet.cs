using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Sets
{
    [Serializable]
    [InlineProperty, FoldoutGroup("Components"), HideLabel]
    public abstract class ComponentSet<TSet, TSetMetadata> : AppalachiaSimpleBase,
                                                             IComponentSet<TSet, TSetMetadata>
        where TSet : ComponentSet<TSet, TSetMetadata>
        where TSetMetadata : ComponentSetMetadata<TSet, TSetMetadata>
    {
        #region Constants and Static Readonly

        protected const string GAME_OBJECT_NAME_FORMAT = "{0} - {{0}}";

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private GameObject gameObject;

        [SerializeField] private Initializer _initialer;

        #endregion

        public abstract string ComponentSetName { get; }

        protected Initializer initializer
        {
            get
            {
                if (_initialer == null)
                {
                    _initialer = new Initializer();
                }

                return _initialer;
            }
        }

        protected virtual string GetGameObjectNameFormat()
        {
            using (_PRF_GetGameObjectNameFormat.Auto())
            {
                return ZString.Format(GAME_OBJECT_NAME_FORMAT, ComponentSetName);
            }
        }

        #region IComponentSet<TSet,TSetMetadata> Members

        public GameObject GameObject => gameObject;

        public virtual void CreateComponents(
            GameObject parent,
            string name /*, string prefixOverride = null*/)
        {
            using (_PRF_CreateComponents.Auto())
            {
                var nameFormatString = /*prefixOverride.IsNullOrWhiteSpace()
                    ? */GetGameObjectNameFormat()
                    /*: prefixOverride*/;

                var targetName = ZString.Format(nameFormatString, name);

                if (gameObject == null)
                {
                    parent.GetOrCreateChild(ref gameObject, targetName, true);
                }

                gameObject.name = targetName;
            }
        }

        #endregion

        #region Profiling

        protected static readonly string _PRF_PFX = typeof(TSet).Name + ".";

        protected static readonly ProfilerMarker _PRF_CreateComponents =
            new ProfilerMarker(_PRF_PFX + nameof(CreateComponents));

        private static readonly ProfilerMarker _PRF_GetGameObjectNameFormat =
            new ProfilerMarker(_PRF_PFX + nameof(GetGameObjectNameFormat));

        #endregion
    }
}
