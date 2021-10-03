using System;
using System.Collections.Generic;

namespace Appalachia.Core.Comparers
{
    public class ObjectComparer<T> : IEqualityComparer<T>, IComparer<T>
    where T : UnityEngine.Object
    {
        private static ObjectComparer<T> _instance;

        public static ObjectComparer<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ObjectComparer<T>();
                }
                
                return _instance;
            }
        }

        public bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            
            if (x == null || y == null) return false;
            
            return x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public int Compare(T x, T y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
                                
            return string.Compare(x.name, y.name, StringComparison.Ordinal);
        }
    }
}
