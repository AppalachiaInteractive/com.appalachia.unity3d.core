using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Comparisons.ComponentEquality
{
    [Serializable]
    public class MeshEqualityState : ComponentContentEqualityState<MeshEqualityState, Mesh>
    {
        [SerializeField] public Bounds bounds;
        [SerializeField] public int triangleCount;
        [SerializeField] public int vertexCount;
        [SerializeField] public Mesh mesh;

        public override void Record(Mesh m)
        {
            mesh = m;
            vertexCount = m.vertexCount;
            triangleCount = m.triangles.Length;
            bounds = m.bounds;
        }

#region IEquatable

        [DebuggerStepThrough] public override bool Equals(Mesh other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return Equals(mesh, other) &&
                   (vertexCount == other.vertexCount) &&
                   (triangleCount == other.triangles.Length) &&
                   bounds.Equals(other.bounds);
        }

        [DebuggerStepThrough] public override bool Equals(MeshEqualityState other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(mesh, other.mesh) &&
                   (vertexCount == other.vertexCount) &&
                   (triangleCount == other.triangleCount) &&
                   bounds.Equals(other.bounds);
        }

        [DebuggerStepThrough] public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MeshEqualityState) obj);
        }

        [DebuggerStepThrough] public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = mesh != null ? mesh.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ vertexCount;
                hashCode = (hashCode * 397) ^ triangleCount;
                hashCode = (hashCode * 397) ^ bounds.GetHashCode();
                return hashCode;
            }
        }

        [DebuggerStepThrough] public static bool operator ==(MeshEqualityState left, MeshEqualityState right)
        {
            return Equals(left, right);
        }

        [DebuggerStepThrough] public static bool operator !=(MeshEqualityState left, MeshEqualityState right)
        {
            return !Equals(left, right);
        }

#endregion
    }
}
