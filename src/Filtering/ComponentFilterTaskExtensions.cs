using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Filtering
{
    public static class ComponentFilterTaskExtensions
    {
        public static Mesh BestMesh(this IComponentFilterTask<MeshFilter> task)
        {
            var result = task.SortBy(ComponentComparisons.BestMeshMeshFilterComparer.Instance)
                             .LimitResults(1)
                             .RunFilter();

            if (result.Length > 0)
            {
                return result[0].sharedMesh;
            }

            return null;
        }

        public static Mesh BestMesh<T>(this IComponentFilterTask<T> task)
            where T : Renderer
        {
            var result = task.SortBy(ComponentComparisons.BestMeshRendererComparer.Instance)
                             .LimitResults(1)
                             .RunFilter();

            if (result.Length > 0)
            {
                return result[0].GetSharedMesh();
            }

            return null;
        }

        public static T BestRenderer<T>(this IComponentFilterTask<T> task)
            where T : Renderer
        {
            var result = task.SortBy(ComponentComparisons.BestMeshRendererComparer.Instance)
                             .LimitResults(1)
                             .RunFilter();

            if (result.Length > 0)
            {
                return result[0];
            }

            return null;
        }

        public static Mesh CheapestMesh(this IComponentFilterTask<MeshFilter> task)
        {
            var result = task.SortBy(ComponentComparisons.WorstMeshMeshFilterComparer.Instance)
                             .LimitResults(1)
                             .RunFilter();

            if (result.Length > 0)
            {
                return result[0].sharedMesh;
            }

            return null;
        }

        public static Mesh CheapestMesh<T>(this IComponentFilterTask<T> task)
            where T : Renderer
        {
            var result = task.SortBy(ComponentComparisons.WorstMeshRendererComparer.Instance)
                             .LimitResults(1)
                             .RunFilter();

            if (result.Length > 0)
            {
                return result[0].GetSharedMesh();
            }

            return null;
        }

        public static T CheapestRenderer<T>(this IComponentFilterTask<T> task)
            where T : Renderer
        {
            var result = task.SortBy(ComponentComparisons.WorstMeshRendererComparer.Instance)
                             .LimitResults(1)
                             .RunFilter();

            if (result.Length > 0)
            {
                return result[0];
            }

            return null;
        }

        public static T CheapestRenderer<T>(this IComponentFilterTask<T> task, int rank)
            where T : Renderer
        {
            rank = Mathf.Clamp(rank, 0, 100);
            var result = task.SortBy(ComponentComparisons.WorstMeshRendererComparer.Instance)
                             .LimitResults(rank + 1)
                             .RunFilter();

            if (result.Length <= 0)
            {
                return null;
            }

            if (result.Length > rank)
            {
                return result[rank];
            }

            return result[result.Length - 1];
        }

        public static IComponentFilterTask<T> FilterComponents<T>(this Component c, bool includeChildren)
            where T : Component
        {
            using (_PRF_ComponentExtensions_FilterComponents.Auto())
            {
                var input = includeChildren ? c.GetComponentsInChildren<T>(true) : c.GetComponents<T>();

                return new ComponentFilterTask<T>(input);
            }
        }

        public static IComponentFilterTask<T> FilterComponents<T>(this GameObject go, bool includeChildren)
            where T : Component
        {
            using (_PRF_ComponentExtensions_FilterComponents.Auto())
            {
                var input = includeChildren ? go.GetComponentsInChildren<T>(true) : go.GetComponents<T>();

                return new ComponentFilterTask<T>(input);
            }
        }

        public static IComponentFilterTask<T> FilterComponentsFromChildren<T>(this Component c)
            where T : Component
        {
            using (_PRF_ComponentExtensions_FilterComponents.Auto())
            {
                var input = c.GetComponentsInChildren<T>(true);

                return new ComponentFilterTask<T>(input);
            }
        }

        public static IComponentFilterTask<T> FilterComponentsFromChildren<T>(this GameObject go)
            where T : Component
        {
            using (_PRF_ComponentExtensions_FilterComponents.Auto())
            {
                var input = go.GetComponentsInChildren<T>(true);

                return new ComponentFilterTask<T>(input);
            }
        }

        public static IComponentFilterTask<T> NoTriggers<T>(this IComponentFilterTask<T> task)
            where T : Collider
        {
            return task.ExcludeIf(c => c.isTrigger);
        }

        public static IComponentFilterTask<T> OnlyTriggers<T>(this IComponentFilterTask<T> task)
            where T : Collider
        {
            return task.IncludeOnlyIf(c => c.isTrigger);
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ComponentExtensions_FilterComponents =
            new("ComponentExtensions.FilterComponents");

        #endregion
    }
}
