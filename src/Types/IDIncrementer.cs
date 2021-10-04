#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Types
{
    [Serializable]
    public sealed class IDIncrementer
    {
        [SerializeField] private int _nextID;

        public IDIncrementer(bool startsAt0)
        {
            _nextID = startsAt0 ? 0 : 1;
        }

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
