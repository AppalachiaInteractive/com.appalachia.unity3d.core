#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Types
{
    [Serializable]
    public sealed class IDIncrementer
    {
        public IDIncrementer(bool startsAt0)
        {
            _nextID = startsAt0 ? 0 : 1;
        }

        #region Fields and Autoproperties

        [SerializeField] private int _nextID;

        #endregion

        public int GetNextIdAndIncrement()
        {
            return _nextID++;
        }

        public void SetNextID(int max)
        {
            _nextID = max;
        }
    }
}
