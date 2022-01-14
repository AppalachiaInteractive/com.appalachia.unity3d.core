#region

using System;
using Appalachia.Core.Collections.Implementations.Lists;

#endregion

namespace Appalachia.Core.Collections.Special
{
    [Serializable]
    public abstract class IsDirtyCollection<T, TList> : AppaLookup<T, bool, TList, AppaList_bool>
        where T : IEquatable<T>
        where TList : AppaList<T>, new()
    {
        #region Fields and Autoproperties

        public bool defaultStateIsDirty = true;

        #endregion

        public void Clean(T check)
        {
            this[check] = false;
        }

        public void CleanAll()
        {
            for (var i = 0; i < Count; i++)
            {
                at[i] = false;
            }
        }

        public void DirtyAll()
        {
            for (var i = 0; i < Count; i++)
            {
                at[i] = true;
            }
        }

        public bool IsDirty(T check)
        {
            if (!ContainsKey(check))
            {
                AddOrUpdate(check, defaultStateIsDirty);
            }

            return this[check];
        }
    }
}
