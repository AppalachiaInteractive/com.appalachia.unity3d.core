#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Volumes.Components;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Volumes
{
    [ExecuteAlways]
    public sealed class PropertyMaster : AppalachiaBehaviour<PropertyMaster>,
                                         IExposedPropertyTable,
                                         ISerializationCallbackReceiver
    {
        public enum UpdateMode
        {
            Automatic,
            Manual
        }

        #region Constants and Static Readonly

        internal static readonly HashSet<Type> componentTypes = new();

        #endregion

        #region Fields and Autoproperties

        [Space(9)] public bool updateAppaVolumes;
        public LayerMask volumeLayerMask = 0;
        public Transform volumeTrigger;

        public UpdateMode updateMode;

        private Dictionary<PropertyName, Object> _exposedReferenceTable = new();

        [SerializeField]
        [HideInInspector]
        private List<ExposedReferenceData> _exposedReferenceList = new();

        #endregion

        public void UpdateProperties()
        {
            using (_PRF_UpdateProperties.Auto())
            {
                var manager = AppaVolumeManager.instance;
                var stack = manager.stack;

                if (updateAppaVolumes && volumeTrigger && (volumeLayerMask != 0))
                {
                    manager.Update(volumeTrigger, volumeLayerMask);
                }

                foreach (var type in componentTypes)
                {
                    var component = (PropertyAppaVolumeComponentBase)stack.GetComponent(type);

                    if (component.active)
                    {
                        component.OverrideProperties(this);
                    }
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                try
                {
                    Camera.onPreCull -= OnCameraPreCull;
                    Camera.onPreCull += OnCameraPreCull;
                }
                catch (Exception ex)
                {
                    Context.Log.Error("Failed to subscribe to OnPreCull.", this, ex);
                }
            }
        }

        protected override async AppaTask WhenDisabled()
        {
            using (_PRF_WhenDisabled.Auto())
            {
                await base.WhenDisabled();

                try
                {
                    if (Camera.onPreCull == null)
                    {
                        return;
                    }

                    Camera.onPreCull -= OnCameraPreCull;
                }
                catch (Exception ex)
                {
                    Context.Log.Error("Failed to UNsubscribe to OnPreCull.", this, ex);
                }
            }
        }

        private void OnCameraPreCull(Camera cam)
        {
            using (_PRF_OnPreCull.Auto())
            {
                try
                {
                    if ((updateMode == UpdateMode.Automatic) &&
                        ((cam.cameraType == CameraType.SceneView) || (cam.cameraType == CameraType.Game)))
                    {
                        UpdateProperties();
                    }
                }
                catch (Exception ex)
                {
                    Context.Log.Error("Failed to handle pre-cull.", this);
                    Debug.LogException(ex, this);
                }
            }
        }

        #region IExposedPropertyTable Members

        public Object GetReferenceValue(PropertyName n, out bool valid)
        {
            using (_PRF_GetReferenceValue.Auto())
            {
                Object value;
                valid = _exposedReferenceTable.TryGetValue(n, out value);
                return value;
            }
        }

        public void ClearReferenceValue(PropertyName n)
        {
            using (_PRF_ClearReferenceValue.Auto())
            {
                _exposedReferenceTable.Remove(n);
            }
        }

        public void SetReferenceValue(PropertyName n, Object value)
        {
            using (_PRF_SetReferenceValue.Auto())
            {
                _exposedReferenceTable[n] = value;
            }
        }

        #endregion

        #region ISerializationCallbackReceiver Members

        public void OnAfterDeserialize()
        {
            using var scope = APPASERIALIZE.OnAfterDeserialize();

            using (_PRF_OnAfterDeserialize.Auto())
            {
                _exposedReferenceTable = new Dictionary<PropertyName, Object>();

                foreach (var i in _exposedReferenceList)
                {
                    _exposedReferenceTable.Add(i.name, i.value);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            using var scope = APPASERIALIZE.OnBeforeSerialize();

            using (_PRF_OnBeforeSerialize.Auto())
            {
                _exposedReferenceList = new List<ExposedReferenceData>();

                foreach (var i in _exposedReferenceTable)
                {
                    _exposedReferenceList.Add(new ExposedReferenceData { name = i.Key, value = i.Value });
                }
            }
        }

        #endregion

        #region Nested type: ExposedReferenceData

        [Serializable]
        private struct ExposedReferenceData
        {
            #region Fields and Autoproperties

            public Object value;
            public PropertyName name;

            #endregion
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ClearReferenceValue =
            new(_PRF_PFX + nameof(ClearReferenceValue));

        private static readonly ProfilerMarker _PRF_GetReferenceValue =
            new(_PRF_PFX + nameof(GetReferenceValue));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new(_PRF_PFX + nameof(OnAfterDeserialize));

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new(_PRF_PFX + nameof(OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnPreCull = new(_PRF_PFX + nameof(OnCameraPreCull));

        private static readonly ProfilerMarker _PRF_SetReferenceValue =
            new(_PRF_PFX + nameof(SetReferenceValue));

        private static readonly ProfilerMarker _PRF_UpdateProperties =
            new(_PRF_PFX + nameof(UpdateProperties));

        #endregion
    }
}
