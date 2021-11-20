#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Debugging;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Volumes
{
    [ExecuteAlways]
    public class AppaVolume : AppalachiaBehaviour
    {
        #region Fields and Autoproperties

        // Modifying sharedProfile will change the behavior of all volumes using this profile, and
        // change profile settings that are stored in the project too
        public AppaVolumeProfile sharedProfile;

//custom-end

        [Tooltip("A global volume is applied to the whole scene.")]
        public bool isGlobal;

        [Tooltip(
            "Outer distance to start blending from. A value of 0 means no blending and the volume overrides will be applied immediately upon entry."
        )]
        public float blendDistance;

        [Tooltip(
            "AppaVolume priority in the stack. Higher number means higher priority. Negative values are supported."
        )]
        public float priority;

        [Range(0f, 1f)]
        [Tooltip(
            "Total weight of this volume in the scene. 0 means it won't do anything, 1 means full effect."
        )]
        public float weight = 1f;

//custom-begin: malte: context reference for exposed property resolver
        public Object context;
        private AppaVolumeProfile m_InternalProfile;
        private float m_PreviousPriority;

        // Needed for state tracking (see the comments in Update)
        private int m_PreviousLayer;

        #endregion

        // This property automatically instantiates the profile and makes it unique to this volume
        // so you can safely edit it via scripting at runtime without changing the original asset
        // in the project.
        // Note that if you pass in your own profile, it is your responsability to destroy it once
        // it's not in use anymore.
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
            using (_PRF_Update.Auto())
            {
                // Unfortunately we need to track the current layer to update the volume manager in
                // real-time as the user could change it at any time in the editor or at runtime.
                // Because no event is raised when the layer changes, we have to track it on every
                // frame :/
                var layer = gameObject.layer;
                if (layer != m_PreviousLayer)
                {
                    AppaVolumeManager.instance.UpdateAppaVolumeLayer(this, m_PreviousLayer, layer);
                    m_PreviousLayer = layer;
                }

                // Same for priority. We could use a property instead, but it doesn't play nice with the
                // serialization system. Using a custom Attribute/PropertyDrawer for a property is
                // possible but it doesn't work with Undo/Redo in the editor, which makes it useless for
                // our case.
                if (Math.Abs(priority - m_PreviousPriority) > float.Epsilon)
                {
                    AppaVolumeManager.instance.SetLayerDirty(layer);
                    m_PreviousPriority = priority;
                }
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();
                
                m_PreviousLayer = gameObject.layer;
                AppaVolumeManager.instance.Register(this, m_PreviousLayer);
            }
        }

        protected override void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                base.OnDisable();
                
                AppaVolumeManager.instance.Unregister(this, gameObject.layer);
            }
        }

        #endregion

        public bool HasInstantiatedProfile()
        {
            return m_InternalProfile != null;
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AppaVolume) + ".";
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + "Awake");
        private static readonly ProfilerMarker _PRF_Start = new(_PRF_PFX + "Start");
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + "OnEnable");
        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + "Update");
        private static readonly ProfilerMarker _PRF_LateUpdate = new(_PRF_PFX + "LateUpdate");
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + "OnDisable");
        private static readonly ProfilerMarker _PRF_OnDestroy = new(_PRF_PFX + "OnDestroy");

        private static readonly ProfilerMarker _PRF_Reset = new(_PRF_PFX + "Reset");

        #endregion

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_OnDrawGizmos = new(_PRF_PFX + "OnDrawGizmos");

        private static readonly ProfilerMarker _PRF_OnDrawGizmosSelected =
            new(_PRF_PFX + "OnDrawGizmosSelected");

        // TODO: Look into a better volume previsualization system
        private List<Collider> m_TempColliders;

//custom-begin: malte: hide collider gizmos
        public bool hideColliderGizmos { get; set; }

//custom-end

        private void OnDrawGizmos()
        {
            if (!GizmoCameraChecker.ShouldRenderGizmos())
            {
                return;
            }

            using (_PRF_OnDrawGizmos.Auto())
            {
                if (m_TempColliders == null)
                {
                    m_TempColliders = new List<Collider>();
                }

                var colliders = m_TempColliders;
                GetComponents(colliders);

//custom-begin: malte: hide collider gizmos
                if (hideColliderGizmos)
                {
                    return;
                }

                //custom-end

                if (isGlobal || (colliders == null))
                {
                    return;
                }

                var transform1 = transform;
                var scale = transform1.localScale;
                var invScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
                Gizmos.matrix = Matrix4x4.TRS(transform1.position, transform1.rotation, scale);
                Gizmos.color = new Color(0f, 1f, 0.1f, 0.35f);

                // Draw a separate gizmo for each collider
                foreach (var coll in colliders)
                {
                    if (!coll.enabled)
                    {
                        continue;
                    }

                    // We'll just use scaling as an approximation for volume skin. It's far from being
                    // correct (and is completely wrong in some cases). Ultimately we'd use a distance
                    // field or at least a tesselate + push modifier on the collider's mesh to get a
                    // better approximation, but the current Gizmo system is a bit limited and because
                    // everything is dynamic in Unity and can be changed at anytime, it's hard to keep
                    // track of changes in an elegant way (which we'd need to implement a nice cache
                    // system for generated volume meshes).
                    var type = coll.GetType();

                    if (type == typeof(BoxCollider))
                    {
                        var c = (BoxCollider) coll;
                        Gizmos.DrawCube(c.center, c.size);
                        Gizmos.DrawWireCube(c.center, c.size + (invScale * (blendDistance * 2f)));
                    }
                    else if (type == typeof(SphereCollider))
                    {
                        var c = (SphereCollider) coll;
                        Gizmos.DrawSphere(c.center, c.radius);
                        Gizmos.DrawWireSphere(c.center, c.radius + (invScale.x * blendDistance));
                    }
                    else if (type == typeof(MeshCollider))
                    {
                        var c = (MeshCollider) coll;

                        // Only convex mesh colliders are allowed
                        if (!c.convex)
                        {
                            c.convex = true;
                        }

                        // Mesh pivot should be centered or this won't work
                        Gizmos.DrawMesh(c.sharedMesh);
                        Gizmos.DrawWireMesh(
                            c.sharedMesh,
                            Vector3.zero,
                            Quaternion.identity,
                            Vector3.one + (invScale * (blendDistance * 2f))
                        );
                    }

                    // Nothing for capsule (DrawCapsule isn't exposed in Gizmo), terrain, wheel and
                    // other colliders...
                }

                colliders.Clear();
            }
        }

#endif
    }
}
