using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Objects.Filtering
{
    public static class ComponentComparisons
    {
        #region Nested type: BestMeshMeshFilterComparer

        public class BestMeshMeshFilterComparer : SimpleComparer<BestMeshMeshFilterComparer, MeshFilter>
        {
            /// <inheritdoc />
            public override int Compare(MeshFilter x, MeshFilter y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }

                if ((x == null) && (y != null))
                {
                    return 1;
                }

                if ((x != null) && (y == null))
                {
                    return -1;
                }

                var smx = x.sharedMesh;
                var smy = y.sharedMesh;

                if ((smx == null) && (smy == null))
                {
                    return 0;
                }

                if ((smx == null) && (smy != null))
                {
                    return 1;
                }

                if ((smx != null) && (smy == null))
                {
                    return -1;
                }

                return -smx.vertexCount.CompareTo(smy.vertexCount);
            }
        }

        #endregion

        #region Nested type: BestMeshRendererComparer

        public class BestMeshRendererComparer : SimpleComparer<BestMeshRendererComparer, Renderer>
        {
            /// <inheritdoc />
            public override int Compare(Renderer x, Renderer y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }

                if ((x == null) && (y != null))
                {
                    return 1;
                }

                if ((x != null) && (y == null))
                {
                    return -1;
                }

                var mfx = x.GetComponent<MeshFilter>();
                var mfy = y.GetComponent<MeshFilter>();

                if ((mfx == null) && (mfy == null))
                {
                    return 0;
                }

                if ((mfx == null) && (mfy != null))
                {
                    return 1;
                }

                if ((mfx != null) && (mfy == null))
                {
                    return -1;
                }

                var smx = mfx.sharedMesh;
                var smy = mfy.sharedMesh;

                if ((smx == null) && (smy == null))
                {
                    return 0;
                }

                if ((smx == null) && (smy != null))
                {
                    return 1;
                }

                if ((smx != null) && (smy == null))
                {
                    return -1;
                }

                return -smx.vertexCount.CompareTo(smy.vertexCount);
            }
        }

        #endregion

        #region Nested type: SimpleComparer

        public abstract class SimpleComparer<T, TC> : IComparer<TC>
            where T : SimpleComparer<T, TC>, new()
        {
            #region Constants and Static Readonly

            public static readonly T Instance = new();

            #endregion

            #region IComparer<TC> Members

            public abstract int Compare(TC x, TC y);

            #endregion
        }

        #endregion

        #region Nested type: WorstMeshMeshFilterComparer

        public class WorstMeshMeshFilterComparer : SimpleComparer<WorstMeshMeshFilterComparer, MeshFilter>
        {
            /// <inheritdoc />
            public override int Compare(MeshFilter x, MeshFilter y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }

                if ((x == null) && (y != null))
                {
                    return 1;
                }

                if ((x != null) && (y == null))
                {
                    return -1;
                }

                var smx = x.sharedMesh;
                var smy = y.sharedMesh;

                if ((smx == null) && (smy == null))
                {
                    return 0;
                }

                if ((smx == null) && (smy != null))
                {
                    return 1;
                }

                if ((smx != null) && (smy == null))
                {
                    return -1;
                }

                return smx.vertexCount.CompareTo(smy.vertexCount);
            }
        }

        #endregion

        #region Nested type: WorstMeshRendererComparer

        public class WorstMeshRendererComparer : SimpleComparer<WorstMeshRendererComparer, Renderer>
        {
            /// <inheritdoc />
            public override int Compare(Renderer x, Renderer y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }

                if ((x == null) && (y != null))
                {
                    return 1;
                }

                if ((x != null) && (y == null))
                {
                    return -1;
                }

                var mfx = x.GetComponent<MeshFilter>();
                var mfy = y.GetComponent<MeshFilter>();

                if ((mfx == null) && (mfy == null))
                {
                    return 0;
                }

                if ((mfx == null) && (mfy != null))
                {
                    return 1;
                }

                if ((mfx != null) && (mfy == null))
                {
                    return -1;
                }

                var smx = mfx.sharedMesh;
                var smy = mfy.sharedMesh;

                if ((smx == null) && (smy == null))
                {
                    return 0;
                }

                if ((smx == null) && (smy != null))
                {
                    return 1;
                }

                if ((smx != null) && (smy == null))
                {
                    return -1;
                }

                return smx.vertexCount.CompareTo(smy.vertexCount);
            }
        }

        #endregion
    }
}
