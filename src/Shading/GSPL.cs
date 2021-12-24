#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Appalachia.Core.Shading
{
    public static class GSPL
    {
        public static Dictionary<string, int> propertyIDsByName
        {
            get
            {
                if (_propertyIDsByName == null)
                {
                    _propertyIDsByName = new Dictionary<string, int>();
                }

                if (_hashedShaders == null)
                {
                    _hashedShaders = new HashSet<string>();
                }

                return _propertyIDsByName;
            }
        }

        private static Dictionary<string, int> _propertyIDsByName;
        private static HashSet<string> _hashedShaders;

        private static object _addLock = new();

        public static int Get(string property)
        {
            if (_addLock == null)
            {
                _addLock = new object();
            }

            if (!propertyIDsByName.ContainsKey(property))
            {
                lock (_addLock)
                {
                    if (!propertyIDsByName.ContainsKey(property))
                    {
                        propertyIDsByName.Add(property, Shader.PropertyToID(property));
                    }
                }
            }

            return propertyIDsByName[property];
        }

        public static void Include(Shader s)
        {
            if (s == null)
            {
                //Context.Log.Warn("Null shader can not be included in property lookup.");
                return;
            }

            if (_hashedShaders == null)
            {
                _hashedShaders = new HashSet<string>();
            }

            var lookup = propertyIDsByName;

            if (!_hashedShaders.Contains(s.name))
            {
                var propCount = s.GetPropertyCount();

                for (var j = 0; j < propCount; j++)
                {
                    var prop = s.GetPropertyName(j);

                    if (!lookup.ContainsKey(prop))
                    {
                        var propID = Shader.PropertyToID(prop);

                        lookup.Add(prop, propID);
                    }
                }

                _hashedShaders.Add(s.name);
            }
        }

        public static void Include(params Shader[] args)
        {
            foreach (var s in args)
            {
                Include(s);
            }
        }

        public static void Include(Material m)
        {
            Include(m.shader);
        }

        public static void Include(params Material[] args)
        {
            foreach (var m in args)
            {
                Include(m.shader);
            }
        }
    }
}
