#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Extensions.Helpers;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class GameObjectExtensions
    {
        private static readonly ProfilerMarker _PRF_GameObjectExtensions_DestroySafely =
            new("GameObjectExtensions.DestroySafely");

        private static readonly ProfilerMarker _PRF_GameObjectExtensions_InstantiatePrefab =
            new("GameObjectExtensions.InstantiatePrefab");

        private static readonly ProfilerMarker _PRF_GameObjectExtensions_MoveToLayerRecursive =
            new("GameObjectExtensions.MoveToLayerRecursive");

        private static readonly ProfilerMarker _PRF_GameObjectExtensions_GetAsset =
            new("GameObjectExtensions.GetAsset");

        private static readonly ProfilerMarker
            _PRF_GameObjectExtensions_GetComponentInImmediateChildren =
                new("GameObjectExtensions.GetComponentInImmediateChildren");

        private static readonly ProfilerMarker
            _PRF_GameObjectExtensions_MoveToLayerRecursiveRecoverable =
                new("GameObjectExtensions.MoveToLayerRecursiveRecoverable");

        private static readonly ProfilerMarker _PRF_GameObjectExtensions_RecoverLayersRecursive =
            new("GameObjectExtensions.RecoverLayersRecursive");

        private static Dictionary<int, string> _assetGUIDLookup = new();

        public static GameObject InstantiatePrefab(
            this GameObject prefab,
            Transform parent = null,
            Matrix4x4 worldPosition = default)
        {
            using (_PRF_GameObjectExtensions_InstantiatePrefab.Auto())
            {
#if UNITY_EDITOR
                var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
#else
            var go = GameObject.Instantiate(prefab) as GameObject;
#endif

                var transform = go.transform;

                if (parent != null)
                {
                    transform.SetParent(parent, false);
                }

                var column0 = worldPosition.GetColumn(0);
                var column1 = worldPosition.GetColumn(1);
                var column2 = worldPosition.GetColumn(2);
                var column3 = worldPosition.GetColumn(3);

                if (worldPosition != default)
                {
                    transform.position = column3;
                    transform.localScale = new Vector3(
                        column0.magnitude,
                        column1.magnitude,
                        column2.magnitude
                    );
                    transform.rotation = Quaternion.LookRotation(column2, column1);
                }

                return go;
            }
        }

        public static T GetComponentInImmediateChildren<T>(this GameObject go)
            where T : Component
        {
            using (_PRF_GameObjectExtensions_GetComponentInImmediateChildren.Auto())
            {
                var cs = go.GetComponentsInChildren<T>();
                for (var index = 0; index < cs.Length; index++)
                {
                    var c = cs[index];
                    if (c.transform.parent == go.transform)
                    {
                        return c;
                    }
                }

                return null;
            }
        }

        public static void MoveToLayerRecursive(this GameObject go, int layer)
        {
            using (_PRF_GameObjectExtensions_MoveToLayerRecursive.Auto())
            {
                go.layer = layer;

                var children = go.GetComponentsInChildren<Transform>();
                foreach (var child in children)
                {
                    child.gameObject.layer = layer;
                }
            }
        }

        public static Dictionary<Transform, int> MoveToLayerRecursiveRecoverable(
            this GameObject go,
            int layer)
        {
            using (_PRF_GameObjectExtensions_MoveToLayerRecursiveRecoverable.Auto())
            {
                var originalLayers = new Dictionary<Transform, int>();
                originalLayers.Add(go.transform, go.layer);
                go.layer = layer;

                var children = go.GetComponentsInChildren<Transform>();
                foreach (var child in children)
                {
                    if (!originalLayers.ContainsKey(child))
                    {
                        originalLayers.Add(child, child.gameObject.layer);
                    }

                    child.gameObject.layer = layer;
                }

                return originalLayers;
            }
        }

        public static void RecoverLayersRecursive(
            this GameObject go,
            Dictionary<Transform, int> originalLayers)
        {
            using (_PRF_GameObjectExtensions_RecoverLayersRecursive.Auto())
            {
                go.layer = originalLayers[go.transform];
                var children = go.GetComponentsInChildren<Transform>();
                foreach (var child in children)
                {
                    child.gameObject.layer = originalLayers[child];
                }
            }
        }

        public static void DestroySafely(this Object o)
        {
            using (_PRF_GameObjectExtensions_DestroySafely.Auto())
            {
                var wasNull = false;

                try
                {
                    if (o is IDisposable i)
                    {
                        i.Dispose();
                    }
                }
                catch (NullReferenceException)
                {
                    wasNull = true;
                }
                catch (Exception ex)
                {
                    ex.LogException($"Not able to dispose of [{o.name}] before destroying.");
                }
                finally
                {
                    if (!wasNull)
                    {
                        try
                        {
                            if (Application.isPlaying)
                            {
                                Object.Destroy(o);
                            }
                            else
                            {
                                Object.DestroyImmediate(o);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException("Exception while destroying object.");
                        }
                    }
                }
            }
        }

        public static void DestroySafely<T>(this T o)
            where T : Object, IDisposable
        {
            using (_PRF_GameObjectExtensions_DestroySafely.Auto())
            {
                var wasNull = false;

                try
                {
                    o.Dispose();
                }
                catch (NullReferenceException)
                {
                    wasNull = true;
                }
                catch (Exception ex)
                {
                    ex.LogException(
                        $"Not able to dispose of [{typeof(T).GetReadableName()} before destroying."
                    );
                }
                finally
                {
                    if (!wasNull)
                    {
                        try
                        {
                            if (Application.isPlaying)
                            {
                                Object.Destroy(o);
                            }
                            else
                            {
                                Object.DestroyImmediate(o);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException("Exception while destroying object.");
                        }
                    }
                }
            }
        }

        public static void DestroySafely(this GameObject o)
        {
            using (_PRF_GameObjectExtensions_DestroySafely.Auto())
            {
                try
                {
                    if (Application.isPlaying)
                    {
                        Object.Destroy(o);
                    }
                    else
                    {
                        Object.DestroyImmediate(o);
                    }
                }
                catch (NullReferenceException)
                {
                }
                catch (Exception ex)
                {
                    ex.LogException("Exception while destroying GameObject.");
                }
            }
        }

        public static void DestroySafely(this Transform o)
        {
            using (_PRF_GameObjectExtensions_DestroySafely.Auto())
            {
                try
                {
                    if (Application.isPlaying)
                    {
                        Object.Destroy(o.gameObject);
                    }
                    else
                    {
                        if (o != null)
                        {
                            Object.DestroyImmediate(o.gameObject);
                        }
                    }
                }
                catch (NullReferenceException)
                {
                }
                catch (Exception ex)
                {
                    ex.LogException("Exception while destroying transform.");
                }
            }
        }

        public static string GetAssetGUID(this GameObject go)
        {
            if (_assetGUIDLookup == null)
            {
                _assetGUIDLookup = new Dictionary<int, string>();
            }

            var hashCode = go.GetHashCode();

            if (_assetGUIDLookup.ContainsKey(hashCode))
            {
                return _assetGUIDLookup[hashCode];
            }

            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(go, out var assetGUID, out long _))
            {
                _assetGUIDLookup.Add(hashCode, assetGUID);

                return assetGUID;
            }

            return null;
        }

#if UNITY_EDITOR
        public static GameObject GetAsset(this GameObject prefab)
        {
            using (_PRF_GameObjectExtensions_GetAsset.Auto())
            {
                if (!PrefabUtility.IsPartOfPrefabInstance(prefab))
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(prefab))
                    {
                        return null;
                    }

                    return prefab;
                }

                return AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab)
                );
            }
        }
#endif
    }
}
