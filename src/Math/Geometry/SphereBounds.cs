#region

using Appalachia.Utility.src.Constants;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Math.Geometry
{
    public struct SphereBounds
    {
        public SphereBounds(float3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public float3 center;
        public float radius;

        public bool Contains(Vector3 position)
        {
            return Contains((float3) position);
        }

        public bool Contains(float3 position)
        {
            return math.distance(position, center) <= radius;
        }

        public bool Intersects(SphereBounds sb)
        {
            var distance = math.distance(sb.center, center);

            return distance < (sb.radius + radius);
        }

        public bool ContainedBy(SphereBounds sb)
        {
            var dist = math.distance(sb.center, center);

            return (dist + radius) <= sb.radius;
        }

        public bool IsOutsideOf(SphereBounds b)
        {
            return !(ContainedBy(b) || Intersects(b));
        }

        public float DistanceToPlane(float3 planeNormal, float planeDistance, float3 point)
        {
            return math.dot(planeNormal, point) + planeDistance;
        }

        public bool IsInsidePlane(Plane p)
        {
            return IsInsidePlane(p.normal, p.distance);
        }

        public bool IsOutsidePlane(Plane p)
        {
            return IsOutsidePlane(p.normal, p.distance);
        }

        public bool IntersectsPlane(Plane p)
        {
            return IntersectsPlane(p.normal, p.distance);
        }

        public bool IsInsidePlane(float3 planeNormal, float planeDistance)
        {
            return -DistanceToPlane(planeNormal, planeDistance, center) > radius;
        }

        public bool IsOutsidePlane(float3 planeNormal, float planeDistance)
        {
            return DistanceToPlane(planeNormal, planeDistance, center) > radius;
        }

        public bool IntersectsPlane(float3 planeNormal, float planeDistance)
        {
            return math.abs(DistanceToPlane(planeNormal, planeDistance, center)) <= radius;
        }

        public bool ContainedBy(Bounds bounds)
        {
            if (!IsInsidePlane(float3c.forward, bounds.max.z))
            {
                return false;
            }

            if (!IsInsidePlane(float3c.back, bounds.min.z))
            {
                return false;
            }

            if (!IsInsidePlane(float3c.up, bounds.max.y))
            {
                return false;
            }

            if (!IsInsidePlane(float3c.down, bounds.min.y))
            {
                return false;
            }

            if (!IsInsidePlane(float3c.right, bounds.max.x))
            {
                return false;
            }

            if (!IsInsidePlane(float3c.left, bounds.min.x))
            {
                return false;
            }

            return true;
        }

        public bool DoesIntersectPlane(
            Plane p,
            out float3 intersectionPoint,
            out float radiusAtIntersection)
        {
            return DoesIntersectPlane(
                p.normal,
                p.distance,
                out intersectionPoint,
                out radiusAtIntersection
            );
        }

        public bool DoesIntersectPlane(
            float3 planeNormal,
            float planeDistance,
            out float3 intersectionPoint,
            out float radiusAtIntersection)
        {
            var d = DistanceToPlane(planeNormal, planeDistance, center);
            var proj = planeNormal * d;
            intersectionPoint = center - proj;
            radiusAtIntersection = math.sqrt(math.max((radius * radius) - (d * d), 0));
            return math.abs(d) <= radius;
        }

        public bool Intersects(Bounds b)
        {
            float3 intersectionPoint;
            float radiusAtIntersection;

            if (DoesIntersectPlane(
                float3c.up,
                b.max.y,
                out intersectionPoint,
                out radiusAtIntersection
            ))
            {
                if ((DistanceToPlane(float3c.left, b.min.x, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.right, b.max.x, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.forward, b.max.z, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.back, b.min.z, intersectionPoint) <=
                     radiusAtIntersection))
                {
                    return true;
                }
            }

            if (DoesIntersectPlane(
                float3c.down,
                b.min.y,
                out intersectionPoint,
                out radiusAtIntersection
            ))
            {
                if ((DistanceToPlane(float3c.left, b.min.x, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.right, b.max.x, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.up, b.max.z, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.back, b.min.z, intersectionPoint) <=
                     radiusAtIntersection))
                {
                    return true;
                }
            }

            if (DoesIntersectPlane(
                float3c.left,
                b.max.x,
                out intersectionPoint,
                out radiusAtIntersection
            ))
            {
                if (
                    (DistanceToPlane(float3c.up, b.max.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.down, b.min.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.forward, b.max.z, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.back, b.min.z, intersectionPoint) <=
                     radiusAtIntersection))
                {
                    return true;
                }
            }

            if (DoesIntersectPlane(
                float3c.right,
                b.max.x,
                out intersectionPoint,
                out radiusAtIntersection
            ))
            {
                if (
                    (DistanceToPlane(float3c.up, b.max.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.down, b.min.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.forward, b.max.z, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.back, b.min.z, intersectionPoint) <=
                     radiusAtIntersection))
                {
                    return true;
                }
            }

            if (DoesIntersectPlane(
                float3c.forward,
                b.max.z,
                out intersectionPoint,
                out radiusAtIntersection
            ))
            {
                if (
                    (DistanceToPlane(float3c.up, b.max.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.down, b.min.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.left, b.min.x, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.right, b.max.x, intersectionPoint) <=
                     radiusAtIntersection))
                {
                    return true;
                }
            }

            if (DoesIntersectPlane(
                float3c.back,
                b.max.z,
                out intersectionPoint,
                out radiusAtIntersection
            ))
            {
                if (
                    (DistanceToPlane(float3c.up, b.max.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.down, b.min.y, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.left, b.max.x, intersectionPoint) <=
                     radiusAtIntersection) &&
                    (DistanceToPlane(float3c.right, b.max.x, intersectionPoint) <=
                     radiusAtIntersection))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsOutsideOf(Bounds b)
        {
            return !(ContainedBy(b) || Intersects(b));
        }

        /*public bool IntersectsRay(Ray r)
        {
            r.direction;
            double A = (vx * vx + vy * vy + vz * vz);
            double B = 2.0 * (px * vx + py * vy + pz * vz - vx * cx - vy * cy - vz * cz);
            double C = px * px - 2 * px * cx + cx * cx + py * py - 2 * py * cy + cy * cy +
                pz * pz - 2 * pz * cz    + cz * cz - radius * radius;
            double D = B * B - 4 * A * C;
            double t = -1.0;
            if (D >= 0)
            {
                double t1 = (-B - System.Math.Sqrt(D)) / (2.0 * A);
                double t2 = (-B + System.Math.Sqrt(D)) / (2.0 * A);
                if (t1 > t2) 
                    t = t1; 
                else 
                    t = t2;  // we choose the nearest t from the first point
            }
        }*/
    }
}
