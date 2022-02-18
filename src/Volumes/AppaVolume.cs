using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Volumes
{
    /// <summary>
    ///     A generic AppaVolume component holding a <see cref="AppaVolumeProfile" />.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu(PKG.Prefix + nameof(AppaVolume))]
    public class AppaVolume : AppalachiaBehaviour<AppaVolume>, IAppaVolume
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     The shared Profile that this AppaVolume uses.
        ///     Modifying <c>sharedProfile</c> changes every AppaVolumes that uses this Profile and also changes
        ///     the Profile settings stored in the Project.
        /// </summary>
        /// <remarks>
        ///     You should not modify Profiles that <c>sharedProfile</c> returns. If you want
        ///     to modify the Profile of a AppaVolume, use <see cref="profile" /> instead.
        /// </remarks>
        /// <seealso cref="profile" />
        public AppaVolumeProfile sharedProfile;

        /// <summary>
        ///     The outer distance to start blending from. A value of 0 means no blending and Unity applies
        ///     the AppaVolume overrides immediately upon entry.
        /// </summary>
        [PropertyTooltip(
            "Sets the outer distance to start blending from. A value of 0 means no blending and Unity applies the AppaVolume overrides immediately upon entry."
        )]
        public float blendDistance;

        /// <summary>
        ///     The AppaVolume priority in the stack. A higher value means higher priority. This supports negative values.
        /// </summary>
        [PropertyTooltip(
            "When multiple AppaVolumes affect the same settings, Unity uses this value to determine which AppaVolume to use. A AppaVolume with the highest Priority value takes precedence."
        )]
        public float priority;

        /// <summary>
        ///     The total weight of this volume in the Scene. 0 means no effect and 1 means full effect.
        /// </summary>
        [PropertyRange(0f, 1f),
         PropertyTooltip(
             "Sets the total weight of this AppaVolume in the Scene. 0 means no effect and 1 means full effect."
         )]
        public float weight = 1f;

        internal List<Collider> m_Colliders = new List<Collider>();
        private AppaVolumeProfile m_InternalProfile;

        [SerializeField, FormerlySerializedAs("isGlobal")]
        private bool m_IsGlobal = true;

        private float m_PreviousPriority;

        // Needed for state tracking (see the comments in Update)
        private int m_PreviousLayer;

        #endregion

        /// <summary>
        ///     Gets the first instantiated <see cref="AppaVolumeProfile" /> assigned to the AppaVolume.
        ///     Modifying <c>profile</c> changes the Profile for this AppaVolume only. If another AppaVolume
        ///     uses the same Profile, this clones the shared Profile and starts using it from now on.
        /// </summary>
        /// <remarks>
        ///     This property automatically instantiates the Profile and make it unique to this AppaVolume
        ///     so you can safely edit it via scripting at runtime without changing the original Asset
        ///     in the Project.
        ///     Note that if you pass your own Profile, you must destroy it when you finish using it.
        /// </remarks>
        /// <seealso cref="sharedProfile" />
        public AppaVolumeProfile profile
        {
            get
            {
                if (m_InternalProfile == null)
                {
                    m_InternalProfile = ScriptableObject.CreateInstance<AppaVolumeProfile>();

                    if (sharedProfile != null)
                    {
                        foreach (var item in sharedProfile.components)
                        {
                            var itemCopy = Instantiate(item);
                            m_InternalProfile.components.Add(itemCopy);
                        }
                    }
                }

                return m_InternalProfile;
            }
            set => m_InternalProfile = value;
        }

        internal AppaVolumeProfile profileRef =>
            m_InternalProfile == null ? sharedProfile : m_InternalProfile;

        #region Event Functions

        private void Update()
        {
            if (ShouldSkipUpdate)
            {
                return;
            }

            // Unfortunately we need to track the current layer to update the volume manager in
            // real-time as the user could change it at any time in the editor or at runtime.
            // Because no event is raised when the layer changes, we have to track it on every
            // frame :/
            UpdateLayer();

            // Same for priority. We could use a property instead, but it doesn't play nice with the
            // serialization system. Using a custom Attribute/PropertyDrawer for a property is
            // possible but it doesn't work with Undo/Redo in the editor, which makes it useless for
            // our case.
            if (priority != m_PreviousPriority)
            {
                AppaVolumeManager.instance.SetLayerDirty(gameObject.layer);
                m_PreviousPriority = priority;
            }

#if UNITY_EDITOR

            // In the editor, we refresh the list of colliders at every frame because it's frequent to add/remove them
            GetComponents(m_Colliders);
#endif
        }

        #endregion

        /// <summary>
        ///     Checks if the AppaVolume has an instantiated Profile or if it uses a shared Profile.
        /// </summary>
        /// <returns><c>true</c> if the profile has been instantiated.</returns>
        /// <seealso cref="profile" />
        /// <seealso cref="sharedProfile" />
        public bool HasInstantiatedProfile()
        {
            return m_InternalProfile != null;
        }

        internal void UpdateLayer()
        {
            var layer = gameObject.layer;
            if (layer != m_PreviousLayer)
            {
                AppaVolumeManager.instance.UpdateAppaVolumeLayer(this, m_PreviousLayer, layer);
                m_PreviousLayer = layer;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();
            using (_PRF_WhenDisabled.Auto())
            {
                AppaVolumeManager.instance.Unregister(this, gameObject.layer);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                m_PreviousLayer = gameObject.layer;
                AppaVolumeManager.instance.Register(this, m_PreviousLayer);

                GetComponents(m_Colliders);
            }
        }

        #region IAppaVolume Members

        /// <summary>
        ///     The colliders of the volume if <see cref="isGlobal" /> is false
        /// </summary>
        public List<Collider> colliders => m_Colliders;

        /// <summary>
        ///     Specifies whether to apply the AppaVolume to the entire Scene or not.
        /// </summary>
        [PropertyTooltip("When enabled, the AppaVolume is applied to the entire Scene.")]
        public bool isGlobal
        {
            get => m_IsGlobal;
            set => m_IsGlobal = value;
        }

        #endregion
    }
}
