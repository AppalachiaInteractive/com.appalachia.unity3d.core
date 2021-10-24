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

        [SerializeField] private int _nextID;

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
