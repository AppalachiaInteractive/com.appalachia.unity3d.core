using Appalachia.Core.Extensions;
using UnityEngine;

namespace Appalachia.Core.Data.ComponentEquality
{
    public static class ComponentEqualityExtensions
    {
        
        public static ColliderEqualityState[] ToEqualityStates(this Collider[] objects)
        {
            var output = new ColliderEqualityState[objects.Length];
            
            for (var i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];

                output[i] = ColliderEqualityState.CreateAndRecord(obj);
            }

            return output;
        }
        
        public static MeshEqualityState[] ToEqualityStates(this MeshRenderer[] objects)
        {
            var output = new MeshEqualityState[objects.Length];
            
            for (var i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];
                var sharedMesh = obj.GetSharedMesh();

                output[i] = MeshEqualityState.CreateAndRecord(sharedMesh);
            }

            return output;
        }
        
        public static MeshEqualityState[] ToEqualityStates(this Mesh[] objects)
        {
            var output = new MeshEqualityState[objects.Length];
            
            for (var i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];

                output[i] = MeshEqualityState.CreateAndRecord(obj);
            }

            return output;
        }
        
    }
}
