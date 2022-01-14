using System;

namespace Appalachia.Core.Collections.Observable
{
    public sealed class ListChangedEventArgs<T> : EventArgs
    {
        public ListChangedEventArgs(int index, T item)
        {
            this.index = index;
            this.item = item;
        }

        #region Fields and Autoproperties

        public readonly int index;
        public readonly T item;

        #endregion
    }
}
