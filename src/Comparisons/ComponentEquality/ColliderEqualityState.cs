using System;
using System.Diagnostics;
using Appalachia.Core.Types.Enums;
using UnityEngine;

namespace Appalachia.Core.Comparisons.ComponentEquality
{
    [Serializable]
    public class ColliderEqualityState : ComponentContentEqualityState<ColliderEqualityState, Collider>
    {
        #region Fields and Autoproperties

        public bool convex;
        public ColliderType colliderType;

        //public CapsuleCollider cc;
        //public Vector3 center;
        //public float radius;
        public float height;

        //public SphereCollider sc;
        //public Vector3 center;
        public float radius;
        public int direction;

        //public MeshCollider mc;
        public Mesh mesh;

        //public BoxCollider bc;
        public Vector3 center;
        public Vector3 size;

        #endregion

        /// <inheritdoc />
        public override void Record(Collider c)
        {
            if (c is MeshCollider mc)
            {
                mesh = mc.sharedMesh;
                convex = mc.convex;
            }
            else if (c is BoxCollider bc)
            {
                center = bc.center;
                size = bc.size;
            }
            else if (c is SphereCollider sc)
            {
                center = sc.center;
                radius = sc.radius;
            }
            else if (c is CapsuleCollider cc)
            {
                center = cc.center;
                radius = cc.radius;
                height = cc.height;
                direction = cc.direction;
            }
        }

        #region IEquatable

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(Collider other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return (other is MeshCollider mc && (mesh == mc.sharedMesh) && (convex == mc.convex)) ||
                   (other is BoxCollider bc && (center == bc.center) && (size == bc.size)) ||
                   (other is SphereCollider sc &&
                    (center == sc.center) &&
                    (Math.Abs(radius - sc.radius) < float.Epsilon)) ||
                   (other is CapsuleCollider cc &&
                    (center == cc.center) &&
                    (Math.Abs(radius - cc.radius) < float.Epsilon) &&
                    (Math.Abs(height - cc.height) < float.Epsilon) &&
                    (direction == cc.direction));
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(ColliderEqualityState other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (colliderType == other.colliderType) &&
                   Equals(mesh, other.mesh) &&
                   (convex == other.convex) &&
                   center.Equals(other.center) &&
                   size.Equals(other.size) &&
                   radius.Equals(other.radius) &&
                   height.Equals(other.height) &&
                   (direction == other.direction);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override bool Equals(object obj)
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

            return Equals((ColliderEqualityState)obj);
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)colliderType;
                hashCode = (hashCode * 397) ^ (mesh != null ? mesh.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ convex.GetHashCode();
                hashCode = (hashCode * 397) ^ center.GetHashCode();
                hashCode = (hashCode * 397) ^ size.GetHashCode();
                hashCode = (hashCode * 397) ^ radius.GetHashCode();
                hashCode = (hashCode * 397) ^ height.GetHashCode();
                hashCode = (hashCode * 397) ^ direction;
                return hashCode;
            }
        }

        [DebuggerStepThrough]
        public static bool operator ==(ColliderEqualityState left, ColliderEqualityState right)
        {
            return Equals(left, right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(ColliderEqualityState left, ColliderEqualityState right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
