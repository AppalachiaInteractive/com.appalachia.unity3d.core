#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#endregion

namespace Appalachia.Core.Volumes.Parameters
{
    // We need this base class to be able to store a list of VolumeParameter in collections as we
    // can't store VolumeParameter<T> with variable T types in the same collection. As a result some
    // of the following is a bit hacky...
    [Serializable]
    public abstract class VolumeParameter
    {
        public const string k_DebuggerDisplay = "{m_Value} ({m_OverrideState})";

        [SerializeField] protected bool m_OverrideState;

        public virtual bool overrideState
        {
            get => m_OverrideState;
            set => m_OverrideState = value;
        }

        internal abstract void Interp(VolumeParameter from, VolumeParameter to, float t);

        public T GetValue<T>()
        {
            return ((VolumeParameter<T>) this).value;
        }

        internal abstract void SetValue(VolumeParameter parameter);

        // This is used in case you need to access fields/properties that can't be accessed in the
        // constructor of a ScriptableObject (VolumeParameter are generally declared and inited in
        // a VolumeComponent which is a ScriptableObject). This will be called right after the
        // VolumeComponent object has been constructed, thus allowing access to previously
        // "forbidden" fields/properties.
        protected internal virtual void OnEnable()
        {
        }

        // Called when the parent VolumeComponent OnDisabled is called
        protected internal virtual void OnDisable()
        {
        }

        public static bool IsObjectParameter(Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(ObjectParameter<>)))
            {
                return true;
            }

            return (type.BaseType != null) && IsObjectParameter(type.BaseType);
        }
    }

    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public class VolumeParameter<T> : VolumeParameter, IEquatable<VolumeParameter<T>>
    {
        protected const float k_DefaultInterpSwap = 0f;

        [SerializeField] protected T m_Value;

        public VolumeParameter() : this(default, false)
        {
        }

        protected VolumeParameter(T value, bool overrideState)
        {
            m_Value = value;

            // ReSharper disable once VirtualMemberCallInConstructor
            this.overrideState = overrideState;
        }

        public virtual T value
        {
            get => m_Value;
            set => m_Value = value;
        }

        public bool Equals(VolumeParameter<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(m_Value, other.m_Value);
        }

        internal override void Interp(VolumeParameter from, VolumeParameter to, float t)
        {
            // Note: this is relatively unsafe (assumes that from and to are both holding type T)
            Interp(from.GetValue<T>(), to.GetValue<T>(), t);
        }

        public virtual void Interp(T from, T to, float t)
        {
            // Default interpolation is naive
            m_Value = t > k_DefaultInterpSwap ? to : from;
        }

        public void Override(T x)
        {
            overrideState = true;
            m_Value = x;
        }

        internal override void SetValue(VolumeParameter parameter)
        {
            m_Value = parameter.GetValue<T>();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 23) + overrideState.GetHashCode();
                if (value != null)
                {
                    hash = (hash * 23) + value.GetHashCode();
                }

                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", value, overrideState);
        }

        public static bool operator ==(VolumeParameter<T> lhs, T rhs)
        {
            return (lhs != null) && (lhs.value != null) && lhs.value.Equals(rhs);
        }

        public static bool operator !=(VolumeParameter<T> lhs, T rhs)
        {
            return !(lhs == rhs);
        }

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

            return Equals((VolumeParameter<T>) obj);
        }

        //
        // Implicit conversion; assuming the following:
        //
        //   var myFloatProperty = new ParameterOverride<float> { value = 42f; };
        //
        // It allows for implicit casts:
        //
        //   float myFloat = myFloatProperty.value; // No implicit cast
        //   float myFloat = myFloatProperty;       // Implicit cast
        //
        // For safety reason this is one-way only.
        //
        public static implicit operator T(VolumeParameter<T> prop)
        {
            return prop.m_Value;
        }
    }

    //
    // The serialization system in Unity can't serialize generic types, the workaround is to extend
    // and flatten pre-defined generic types.
    // For enums it's recommended to make your own types on the spot, like so:
    //
    //  [Serializable]
    //  public sealed class MyEnumParameter : VolumeParameter<MyEnum> { }
    //  public enum MyEnum { One, Two }
    //

    // Holds a min & a max values clamped in a range (MinMaxSlider in the editor)

    // 32-bit RGBA

    // Used as a container to store custom serialized classes/structs inside volume components
}
