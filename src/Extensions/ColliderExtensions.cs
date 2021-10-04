#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class ColliderExtensions
    {
        private const float FourThirds = 4.0f / 3.0f;
        private const float sphereInitial = FourThirds * Mathf.PI;

        public static float GetVolume(this IEnumerable<Collider> colliders)
        {
            var volume = 0.0f;

            foreach (var c in colliders)
            {
                volume += GetVolume(c);
            }

            return volume;
        }

        public static float GetVolume(this Collider c)
        {
            if (c is MeshCollider mc)
            {
                return mc.sharedMesh.GetVolume();
            }

            if (c is SphereCollider sc)
            {
                // 4/3 x π x (diameter / 2)3
                return sphereInitial * Mathf.Pow(sc.radius, 3.0f);
            }

            if (c is BoxCollider bc)
            {
                var size = bc.size;
                return size.x * size.y * size.z;
            }

            if (c is CapsuleCollider cc)
            {
                // V = πr2((4/3)r + a)
                var r = cc.radius;
                var a = cc.height - r - r;
                return Mathf.PI * Mathf.Pow(r, 2.0f) * ((FourThirds * r) + a);
            }

            if (c is CharacterController chc)
            {
                var r = chc.radius;
                var a = chc.height - r - r;
                return Mathf.PI * Mathf.Pow(r, 2.0f) * ((FourThirds * r) + a);
            }

            return 0.0f;
        }
    }
}
