#region

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class BoundsExtensions
    {
        public static Bounds GetEncompassingBounds(this GameObject gameObject)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            return GetEncompassingBounds(renderers);
        }

        public static Bounds GetEncompassingBounds(this MeshRenderer[] renderers)
        {
            var result = new Bounds();
            var first = true;

            for (var i = 0; i < renderers.Length; i++)
            {
                var bounds = renderers[i].bounds;

                if (first)
                {
                    result = bounds;
                    first = false;
                }
                else
                {
                    result.Encapsulate(bounds);
                }
            }

            return result;
        }

        public static Bounds GetEncompassingBounds(this SkinnedMeshRenderer[] renderers)
        {
            var result = new Bounds();
            var first = true;

            for (var i = 0; i < renderers.Length; i++)
            {
                var bounds = renderers[i].bounds;

                if (first)
                {
                    result = bounds;
                    first = false;
                }
                else
                {
                    result.Encapsulate(bounds);
                }
            }

            return result;
        }

        public static Bounds GetEncompassingBounds(this Renderer[] renderers)
        {
            var result = new Bounds();
            var first = true;

            for (var i = 0; i < renderers.Length; i++)
            {
                var bounds = renderers[i].bounds;

                if (first)
                {
                    result = bounds;
                    first = false;
                }
                else
                {
                    result.Encapsulate(bounds);
                }
            }

            return result;
        }

        public static Bounds GetEncompassingBounds(this Collider[] colliders)
        {
            var result = new Bounds();
            var first = true;

            for (var i = 0; i < colliders.Length; i++)
            {
                var bounds = colliders[i].bounds;

                if (first)
                {
                    result = bounds;
                    first = false;
                }
                else
                {
                    result.Encapsulate(bounds);
                }
            }

            return result;
        }

        public static Bounds GetEncompassingBounds<T>(this IEnumerable<T> objects, Func<T, Vector3> retriever)
            where T : Object
        {
            var result = new Bounds();
            var first = true;

            foreach (var obj in objects)
            {
                var bounds = retriever(obj);

                if (first)
                {
                    result.center = bounds;
                    first = false;
                }
                else
                {
                    result.Encapsulate(bounds);
                }
            }

            return result;
        }
    }
}
