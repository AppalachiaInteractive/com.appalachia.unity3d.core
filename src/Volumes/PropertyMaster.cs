#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Extensions.Helpers;
using Appalachia.Core.Volumes.Components;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Volumes
{
    [ExecuteAlways]
    public class PropertyMaster : MonoBehaviour, IExposedPropertyTable, ISerializationCallbackReceiver
    {
        private const string _PRF_PFX = nameof(PropertyMaster) + ".";

        internal static readonly HashSet<Type> componentTypes = new();

        private static readonly ProfilerMarker _PRF_ClearReferenceValue =
            new(_PRF_PFX + nameof(ClearReferenceValue));

        private static readonly ProfilerMarker _PRF_GetReferenceValue =
            new(_PRF_PFX + nameof(GetReferenceValue));

        private static readonly ProfilerMarker _PRF_SetReferenceValue =
            new(_PRF_PFX + nameof(SetReferenceValue));

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new(_PRF_PFX + nameof(OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new(_PRF_PFX + nameof(OnAfterDeserialize));

        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + nameof(OnDisable));

        private static readonly ProfilerMarker _PRF_OnPreCull = new(_PRF_PFX + nameof(OnPreCull));

        private static readonly ProfilerMarker _PRF_UpdateProperties =
            new(_PRF_PFX + nameof(UpdateProperties));

        [Space(9)] public bool updateVolumes;
        public LayerMask volumeLayerMask = 0;
        public Transform volumeTrigger;

        public UpdateMode updateMode;

        private Camera _propCamera;

        private Dictionary<PropertyName, Object> _exposedReferenceTable = new();

        [SerializeField]
        [HideInInspector]
        private List<ExposedReferenceData> _exposedReferenceList = new();

        public void UpdateProperties()
        {
            using (_PRF_UpdateProperties.Auto())
            {
                var manager = VolumeManager.instance;
                var stack = manager.stack;

                if (updateVolumes && volumeTrigger && (volumeLayerMask != 0))
                {
                    manager.Update(volumeTrigger, volumeLayerMask);
                }

                foreach (var type in componentTypes)
                {
                    var component = (PropertyVolumeComponentBase) stack.GetComponent(type);

                    if (component.active)
                    {
                        component.OverrideProperties(this);
                    }
                }
            }
        }

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

        public void OnAfterDeserialize()
        {
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
            using (_PRF_OnBeforeSerialize.Auto())
            {
                _exposedReferenceList = new List<ExposedReferenceData>();

                foreach (var i in _exposedReferenceTable)
                {
                    _exposedReferenceList.Add(new ExposedReferenceData {name = i.Key, value = i.Value});
                }
            }
        }

        protected void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                try
                {
                    if (Camera.onPreCull == null)
                    {
                        return;
                    }

                    Camera.onPreCull -= cam => OnPreCull();
                }
                catch (Exception ex)
                {
#if UNITY_EDITOR
                    Debug.LogError($"Failed to UNsubscribe to OnPreCull.", this);
                    Debug.LogException(ex, this);
#endif
                }
            }
        }

        protected void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                try
                {
                    if (Camera.onPreCull == null)
                    {
                        Camera.onPreCull = cam => OnPreCull();
                    }
                    else
                    {
                        Camera.onPreCull += cam => OnPreCull();
                    }
                }
                catch (Exception ex)
                {
#if UNITY_EDITOR
                    Debug.LogError($"Failed to subscribe to OnPreCull.", this);
                    Debug.LogException(ex, this);
#endif
                }
            }
        }

        private void OnPreCull()
        {
            using (_PRF_OnPreCull.Auto())
            {
                try
                {
                    if (_propCamera == null)
                    {
                        _propCamera = Camera.main;
                    }

                    if ((updateMode == UpdateMode.Automatic) &&
                        ((_propCamera.cameraType == CameraType.SceneView) ||
                         (_propCamera.cameraType == CameraType.Game)))
                    {
                        UpdateProperties();
                    }
                }
                catch (Exception ex)
                {
#if UNITY_EDITOR
                    Debug.LogError($"Failed to handle pre-cull.", this);
                    Debug.LogException(ex, this);
#endif
                }
            }
        }

        public enum UpdateMode
        {
            Automatic,
            Manual
        }

        [Serializable]
        private struct ExposedReferenceData
        {
            public Object value;
            public PropertyName name;
        }
    }
}
