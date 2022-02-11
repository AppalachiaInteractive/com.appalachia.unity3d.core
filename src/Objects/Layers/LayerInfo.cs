#region

using System;
using System.Diagnostics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Layers
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

        #region Fields and Autoproperties

        public int Id { get; }
        public LayerMask Mask { get; }
        public string Name { get; }

        #endregion

        public void Assign(GameObject go)
        {
            go.layer = Id;
        }

        #region IEquatable

        [DebuggerStepThrough]
        public bool Equals(LayerInfo other)
        {
            return Id == other.Id;
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is LayerInfo other && Equals(other);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return Id;
        }

        [DebuggerStepThrough]
        public static bool operator ==(LayerInfo left, LayerInfo right)
        {
            return left.Equals(right);
        }

        [DebuggerStepThrough]
        public static bool operator !=(LayerInfo left, LayerInfo right)
        {
            return !left.Equals(right);
        }

        [DebuggerStepThrough]
        public bool Equals(int other)
        {
            return Id == other;
        }

        #endregion
    }
}
