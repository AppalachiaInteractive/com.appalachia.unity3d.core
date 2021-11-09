using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{ // We need this base class to be able to store a list of AppaVolumeParameter in collections as we
    // can't store AppaVolumeParameter<T> with variable T types in the same collection. As a result some
    // of the following is a bit hacky...

    [Serializable]
    [DebuggerDisplay(k_DebuggerDisplay)]
    public class AppaVolumeParameter<T> : AppaVolumeParameter, IEquatable<AppaVolumeParameter<T>>
    {
        protected const float k_DefaultInterpSwap = 0f;

        public AppaVolumeParameter() : this(default, false)
        {
        }

        protected AppaVolumeParameter(T value, bool overrideState)
        {
            m_Value = value;

            // ReSharper disable once VirtualMemberCallInConstructor
            this.overrideState = overrideState;
        }

        [SerializeField] protected T m_Value;

        public virtual T value
        {
            get => m_Value;
            set => m_Value = value;
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

        public bool Equals(AppaVolumeParameter<T> other)
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

            return Equals((AppaVolumeParameter<T>) obj);
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
            return $"{value} ({overrideState})";
        }

        internal override void Interp(AppaVolumeParameter from, AppaVolumeParameter to, float t)
        {
            // Note: this is relatively unsafe (assumes that from and to are both holding type T)
            Interp(from.GetValue<T>(), to.GetValue<T>(), t);
        }

        internal override void SetValue(AppaVolumeParameter parameter)
        {
            m_Value = parameter.GetValue<T>();
        }

        public static bool operator ==(AppaVolumeParameter<T> lhs, T rhs)
        {
            return (lhs != null) && (lhs.value != null) && lhs.value.Equals(rhs);
        }

        public static bool operator !=(AppaVolumeParameter<T> lhs, T rhs)
        {
            return !(lhs == rhs);
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
        public static implicit operator T(AppaVolumeParameter<T> prop)
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
    //  public sealed class MyEnumParameter : AppaVolumeParameter<MyEnum> { }
    //  public enum MyEnum { One, Two }
    //

    // Holds a min & a max values clamped in a range (MinMaxSlider in the editor)

    // 32-bit RGBA

    // Used as a container to store custom serialized classes/structs inside volume components
    [Serializable]
    public abstract class AppaVolumeParameter
    {
        public const string k_DebuggerDisplay = "{m_Value} ({m_OverrideState})";

        [SerializeField] protected bool m_OverrideState;

        public virtual bool overrideState
        {
            get => m_OverrideState;
            set => m_OverrideState = value;
        }

        internal abstract void Interp(AppaVolumeParameter from, AppaVolumeParameter to, float t);

        internal abstract void SetValue(AppaVolumeParameter parameter);

        // Called when the parent AppaVolumeComponent OnDisabled is called
        protected internal virtual void OnDisable()
        {
        }

        // This is used in case you need to access fields/properties that can't be accessed in the
        // constructor of a ScriptableObject (AppaVolumeParameter are generally declared and inited in
        // a AppaVolumeComponent which is a ScriptableObject). This will be called right after the
        // AppaVolumeComponent object has been constructed, thus allowing access to previously
        // "forbidden" fields/properties.
        protected internal virtual void OnEnable()
        {
        }

        public T GetValue<T>()
        {
            return ((AppaVolumeParameter<T>) this).value;
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
}
