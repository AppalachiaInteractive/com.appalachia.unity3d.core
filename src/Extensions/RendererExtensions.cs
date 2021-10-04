#region

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

#endregion

namespace Appalachia.Core.Extensions
{
    public static class RendererExtensions
    {
        //[CanBeNull]
        public static Mesh GetSharedMesh(this Renderer renderer)
        {
            if (renderer is SkinnedMeshRenderer skinned)
            {
                return skinned.sharedMesh;
            }

            var filter = renderer.GetComponent<MeshFilter>();

            if (filter == null)
            {
                return null;
            }

            return filter.sharedMesh;
        }

        public static void SetMesh(this Renderer renderer, Mesh mesh)
        {
            if (renderer is SkinnedMeshRenderer skinned)
            {
                skinned.sharedMesh = mesh;
            }
            else
            {
                var filter = renderer.GetComponent<MeshFilter>();

                if (filter == null)
                {
                    return;
                }

                filter.sharedMesh = mesh;
            }
        }

#if UNITY_EDITOR

        public static void EnableGIForBakeOnly(this GameObject prefab)
        {
            var renderers = prefab.GetComponentsInChildren<MeshRenderer>();

            var save = false;

            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];

                save |= EnableGIForBakeOnly(renderer);
            }

            if (save)
            {
                PrefabUtility.SavePrefabAsset(prefab);
                EditorUtility.SetDirty(prefab);
            }
        }

        public static bool EnableGIForBakeOnly(this MeshRenderer renderer)
        {
            var save = false;

            var flags = GameObjectUtility.GetStaticEditorFlags(renderer.gameObject);

            var contributeGI = flags.HasFlag(StaticEditorFlags.ContributeGI);

            if (!contributeGI)
            {
                GameObjectUtility.SetStaticEditorFlags(renderer.gameObject, StaticEditorFlags.ContributeGI);
                save = true;
            }

            if (renderer.receiveGI != ReceiveGI.LightProbes)
            {
                renderer.receiveGI = ReceiveGI.LightProbes;
                save = true;
            }

            var o = new SerializedObject(renderer);
            var apply = false;

            var lightmapScale = o.FindProperty("m_ScaleInLightmap");

            if (lightmapScale.floatValue != 0.0f)
            {
                lightmapScale.floatValue = 0.0f;

                save = true;
                apply = true;
            }

            var lightmapChartSize = o.FindProperty("m_MinimumChartSize");

            if (lightmapChartSize.intValue != 2)
            {
                lightmapChartSize.intValue = 2;

                save = true;
                apply = true;
            }

            if (apply)
            {
                o.ApplyModifiedProperties();
            }

            return save;
        }

#endif
    }
}
