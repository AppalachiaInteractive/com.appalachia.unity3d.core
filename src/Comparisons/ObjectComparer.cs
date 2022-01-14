using System;
using System.Collections.Generic;
using System.Diagnostics;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Comparisons
{
    public class ObjectComparer<T> : IEqualityComparer<T>, IComparer<T>
        where T : Object
    {
        #region Static Fields and Autoproperties

        private static ObjectComparer<T> _instance;

        #endregion

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

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }

            if (x == null)
            {
                return 1;
            }

            if (y == null)
            {
                return -1;
            }

            return string.Compare(x.name, y.name, StringComparison.Ordinal);
        }

        #endregion

        #region IEqualityComparer<T> Members

        [DebuggerStepThrough]
        public bool Equals(T x, T y)
        {
            if ((x == null) && (y == null))
            {
                return true;
            }

            if ((x == null) || (y == null))
            {
                return false;
            }

            return x.Equals(y);
        }

        [DebuggerStepThrough]
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
