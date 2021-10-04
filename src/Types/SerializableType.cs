using System;
using Appalachia.Utility.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Types
{
    [Serializable]
    public class SerializableType
    {
        [SerializeField] private string _typeString;

        [NonSerialized] private ValueDropdownList<short> _enumValues;
        [NonSerialized] private Type _type;

        public SerializableType(Type t)
        {
            type = t;
        }

        public Type type
        {
            get
            {
                if (_type != null)
                {
                    return _type;
                }

                if (_typeString != null)
                {
                    _type = Type.GetType(_typeString);
                }

                return _type;
            }
            set
            {
                _type = value;
                _typeString = value?.AssemblyQualifiedName;
            }
        }

        public ValueDropdownList<short> EnumValues
        {
            get
            {
                if (_enumValues == null)
                {
                    _enumValues = new ValueDropdownList<short>();

                    if (type.IsEnum)
                    {
                        var values = EnumValueManager.GetAllValues(type);

                        for (var i = 0; i < values.Length; i++)
                        {
                            var value = values[i];

                            _enumValues.Add(value.ToString(), (short) value);
                        }
                    }
                }

                return _enumValues;
            }
        }

        // allow SerializableType to implicitly be converted to and from System.Type
        public static implicit operator Type(SerializableType stype)
        {
            return stype.type;
        }

        public static implicit operator SerializableType(Type t)
        {
            return new(t);
        }

        // overload the == and != operators
        public static bool operator ==(SerializableType a, SerializableType b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) a == null) || ((object) b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.type == b.type;
        }

        public static bool operator !=(SerializableType a, SerializableType b)
        {
            return !(a == b);
        }

        // we don't need to overload operators between SerializableType and System.Type because we already enabled them to implicitly convert

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        // overload the .Equals method
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to SerializableType return false.
            var p = obj as SerializableType;
            if ((object) p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return type == p.type;
        }

        public bool Equals(SerializableType p)
        {
            // If parameter is null return false:
            if ((object) p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return type == p.type;
        }
    }
}
