#if UNITY_EDITOR
using System.Linq;
using Appalachia.Utility.Framing;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaBehaviour<T>
    {
        #region Fields and Autoproperties

        private bool? _showButtons;

        #endregion

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

        [ButtonGroup(GROUP_BUTTONS)]
        [PropertyOrder(-1000)]
        [ShowIf(nameof(ShowButtons))]
        private void SelectAllInScene()
        {
            var type = GetType();

            var instances = FindObjectsOfType(type);

            UnityEditor.Selection.objects = instances;
        }

        [ButtonGroup(GROUP_BUTTONS)]
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
    }
}

#endif
