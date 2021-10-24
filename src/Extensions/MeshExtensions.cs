#region

using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class MeshExtensions
    {
        public static float GetVolume(this IReadOnlyList<Renderer> renderers)
        {
            var meshes = renderers.Select(r => r.GetSharedMesh()).ToList();

            return GetVolume(meshes);
        }

        public static float GetVolume(this IReadOnlyList<Mesh> meshes)
        {
            var volumes = new float[meshes.Count];

            for (var index = 0; index < meshes.Count; index++)
            {
                var mesh = meshes[index];
                GetVolumeAndCenterOfMass(mesh, true, out volumes[index], true, out _);
            }

            var volume = 0.0f;

            for (var i = 0; i < volumes.Length; i++)
            {
                volume += volumes[i];
            }

            return volume;
        }

        public static float GetVolume(this Mesh[] meshes)
        {
            var volumes = new float[meshes.Length];

            for (var index = 0; index < meshes.Length; index++)
            {
                var mesh = meshes[index];
                GetVolumeAndCenterOfMass(mesh, true, out volumes[index], true, out _);
            }

            var volume = 0.0f;

            for (var i = 0; i < volumes.Length; i++)
            {
                volume += volumes[i];
            }

            return volume;
        }

        public static float GetVolume(this Mesh mesh)
        {
            GetVolumeAndCenterOfMass(mesh, true, out var volume, false, out _);

            return volume;
        }

        public static Vector3 GetCenterOfMass(this IReadOnlyList<Mesh> meshes)
        {
            GetVolumeAndCenterOfMass(meshes, out _, out var centerOfMass);

            return centerOfMass;
        }

        public static Vector3 GetCenterOfMass(this Mesh[] meshes)
        {
            GetVolumeAndCenterOfMass(meshes, out _, out var centerOfMass);

            return centerOfMass;
        }

        public static Vector3 GetCenterOfMass(this Mesh mesh)
        {
            GetVolumeAndCenterOfMass(mesh, false, out _, true, out var centerOfMass);

            return centerOfMass;
        }

        public static void GetVolumeAndCenterOfMass(
            this IReadOnlyList<Mesh> meshes,
            out float volume,
            out Vector3 centerOfMass)
        {
            var volumes = new float[meshes.Count];
            var centersOfMass = new Vector3[meshes.Count];

            for (var index = 0; index < meshes.Count; index++)
            {
                var mesh = meshes[index];
                GetVolumeAndCenterOfMass(mesh, true, out volumes[index], true, out centersOfMass[index]);
            }

            volume = 0.0f;

            for (var i = 0; i < volumes.Length; i++)
            {
                volume += volumes[i];
            }

            centerOfMass = Vector3.zero;

            for (var i = 0; i < volumes.Length; i++)
            {
                var percentage = volumes[i] / volume;

                centerOfMass += percentage * centersOfMass[i];
            }
        }

        public static void GetVolumeAndCenterOfMass(
            this Mesh[] meshes,
            out float volume,
            out Vector3 centerOfMass)
        {
            var volumes = new float[meshes.Length];
            var centersOfMass = new Vector3[meshes.Length];

            for (var index = 0; index < meshes.Length; index++)
            {
                var mesh = meshes[index];
                GetVolumeAndCenterOfMass(mesh, true, out volumes[index], true, out centersOfMass[index]);
            }

            volume = 0.0f;

            for (var i = 0; i < volumes.Length; i++)
            {
                volume += volumes[i];
            }

            centerOfMass = Vector3.zero;

            for (var i = 0; i < volumes.Length; i++)
            {
                var percentage = volumes[i] / volume;

                centerOfMass += percentage * centersOfMass[i];
            }
        }

        public static void GetVolumeAndCenterOfMass(
            this Mesh mesh,
            out float volume,
            out Vector3 centerOfMass)
        {
            GetVolumeAndCenterOfMass(mesh, true, out volume, true, out centerOfMass);
        }

        public static void GetVolumeAndCenterOfMass(
            Mesh mesh,
            bool calculateVolume,
            out float volume,
            bool calculateCenterOfMass,
            out Vector3 centerOfMass)
        {
            GetVolumeAndCenterOfMass(
                mesh.vertices,
                mesh.triangles,
                calculateVolume,
                out volume,
                calculateCenterOfMass,
                out centerOfMass
            );
        }

        /*public static void GetVolumeAndCenterOfMass(this MeshObject mesh, 
                                                    bool solid, out float volume, out Vector3 centerOfMass)
        {
            GetVolumeAndCenterOfMass(mesh, solid, true, out volume, true, out centerOfMass);
        }*/

        /*public static Vector3 GetCenterOfMass(this MeshObject mesh, 
                                              bool solid)
        {            
            GetVolumeAndCenterOfMass(mesh, solid,false, out _, true, out var centerOfMass);

            return centerOfMass;
        }
        
        public static float GetVolume(this MeshObject mesh, 
                                      bool solid)
        {
            GetVolumeAndCenterOfMass(mesh, solid,true, out var volume, false, out _);

            return volume;
        }
        
        public static void GetVolumeAndCenterOfMass(
            MeshObject mesh, 
            bool solid,
            bool calculateVolume,
            out float volume, 
            bool calculateCenterOfMass,
            out Vector3 centerOfMass)
        {
            GetVolumeAndCenterOfMass(
                solid ? mesh.solidTriangles : mesh.triangles,
                calculateVolume,
                out volume,
                calculateCenterOfMass,
                out centerOfMass
            );
        }*/

        public static void GetVolumeAndCenterOfMass(
            Vector3[] vertices,
            int[] triangles,
            bool calculateVolume,
            out float volume,
            bool calculateCenterOfMass,
            out Vector3 centerOfMass)
        {
            centerOfMass = Vector3.zero;
            volume = 0f;

            for (var t = 0; t < triangles.Length; t += 3)
            {
                var v0 = vertices[triangles[t + 0]];
                var v1 = vertices[triangles[t + 1]];
                var v2 = vertices[triangles[t + 2]];

                var v = SignedVolumeOfTriangle(v0, v1, v2);

                if (calculateCenterOfMass)
                {
                    centerOfMass += (v * (v0 + v1 + v2)) / 4f;
                }

                if (calculateVolume)
                {
                    volume += v;
                }
            }

            centerOfMass /= volume > 0 ? volume : 1f;
        }

        /*public static void GetVolumeAndCenterOfMass(
            IEnumerable<MeshTriangle> triangles,
            bool calculateVolume,
            out float volume, 
            bool calculateCenterOfMass,
            out Vector3 centerOfMass)
        {
            centerOfMass = Vector3.zero;
            volume = 0f;

            foreach (var triangle in triangles)
            {
                var v0 = triangle.x;
                var v1 = triangle.y;
                var v2 = triangle.z;

                var v = SignedVolumeOfTriangle(v0.position, v1.position, v2.position);

                if (calculateCenterOfMass) centerOfMass += (v * (v0.position + v1.position + v2.position)) / 4f;
                if (calculateVolume) volume += v;
            }

            centerOfMass /= volume > 0 ? volume : 1f;
        }*/
        /**/

        private static float SignedVolumeOfTriangle(float3 p1, float3 p2, float3 p3)
        {
            return math.dot(p1, math.cross(p2, p3)) / 6.0f;
        }
    }
}
