using System;
using System.Linq;
using Appalachia.Utility.Framing;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Behaviours
{
    [Serializable]
    public abstract class AppalachiaBehaviour : MonoBehaviour
    {
        #region Fields

        private Bounds ___renderingBounds;
        private Transform ___transform;

        #endregion

        public Bounds renderingBounds
        {
            get
            {
                if (___renderingBounds == default)
                {
                    ___renderingBounds = gameObject.GetRenderingBounds(out _);
                }

                return ___renderingBounds;
            }
        }

        protected Transform _transform
        {
            get
            {
                if (___transform == null)
                {
                    ___transform = transform;
                }

                return ___transform;
            }
        }

        #region Event Functions

        private void Reset()
        {
            ___renderingBounds = default; 
            ___transform = default;
            
            OnReset();
        }

        #endregion

        protected virtual void OnReset()
        {
            
        }

        protected void DontDestroyOnLoadSafe(Object obj)
        {
#if !UNITY_EDITOR
            DontDestroyOnLoad(obj);
#endif
        }

        protected float3 LocalToWorldDirection(float3 direction)
        {
            return _transform.TransformDirection(direction);
        }

        protected float3 LocalToWorldPoint(float3 point)
        {
            return _transform.TransformPoint(point);
        }

        protected float3 LocalToWorldVector(float3 vector)
        {
            return _transform.TransformVector(vector);
        }

        protected void RecalculateBounds()
        {
            ___renderingBounds = default;
        }

        protected float3 WorldToLocalDirection(float3 direction)
        {
            return _transform.InverseTransformDirection(direction);
        }

        protected float3 WorldToLocalPoint(float3 point)
        {
            return _transform.InverseTransformPoint(point);
        }

        protected float3 WorldToLocalVector(float3 vector)
        {
            return _transform.InverseTransformVector(vector);
        }

#if UNITY_EDITOR

        protected void SetDirty()
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }

        private bool? _showButtons;

        private bool ShowButtons
        {
            get
            {
                if (!_showButtons.HasValue)
                {
                    _showButtons = !GetType().InheritsFrom(typeof(SingletonAppalachiaBehaviour<>));
                }

                return _showButtons.Value;
            }
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Small)]
        [ButtonGroup("base_A")]
        [PropertyOrder(-1000)]
        [ShowIf(nameof(ShowButtons))]
        private void SelectAllInScene()
        {
            var type = GetType();

            var instances = FindObjectsOfType(type);

            UnityEditor.Selection.objects = instances;
        }

        [Button(ButtonSizes.Small)]
        [ButtonGroup("base_A")]
        [PropertyOrder(-1000)]
        [ShowIf(nameof(ShowButtons))]
        private void SelectObjectsInScene()
        {
            var type = GetType();

            // ReSharper disable once CoVariantArrayConversion
            Object[] instances = FindObjectsOfType(type)
                                .Select(
                                     o =>
                                     {
                                         if (o is Component c)
                                         {
                                             return c.gameObject;
                                         }

                                         return null;
                                     }
                                 )
                                .ToArray();

            UnityEditor.Selection.objects = instances;
        }
#endif

        public void Frame(bool recalculateBounds = false, bool adjustAngle = true)
        {
            if (recalculateBounds)
            {
                RecalculateBounds();

                var filters = GetComponentsInChildren<MeshFilter>();

                for (var i = 0; i < filters.Length; i++)
                {
                    var mf = filters[i];

                    if (mf != null)
                    {
                        var mesh = mf.sharedMesh;
                        mesh.RecalculateBounds();
                        mesh.UploadMeshData(false);
                    }
                }
            }

            gameObject.Frame(FrameTarget.SceneView, adjustAngle);
        }
#endif
    }
}
