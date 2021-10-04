#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Layers
{
    [Serializable]
    public struct LayerInfo : IEquatable<LayerInfo>, IEquatable<int>
    {
        public LayerInfo(int id)
        {
            Id = id;
            Name = LayerMask.LayerToName(Id);
            Mask = LayerMask.GetMask(Name);
        }

        public int Id { get; }
        public string Name { get; }
        public LayerMask Mask { get; }

        public void Assign(GameObject go)
        {
            go.layer = Id;
        }

#region IEquatable

        public bool Equals(LayerInfo other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is LayerInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(LayerInfo left, LayerInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LayerInfo left, LayerInfo right)
        {
            return !left.Equals(right);
        }

        public bool Equals(int other)
        {
            return Id == other;
        }

#endregion
    }
}
