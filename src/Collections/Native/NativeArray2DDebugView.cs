namespace Appalachia.Core.Collections.Native
{
    internal sealed class NativeArray2DDebugView<T>
        where T : struct
    {
        public NativeArray2DDebugView(NativeArray2D<T> array)
        {
            _mArray = array;
        }

        #region Fields and Autoproperties

        private readonly NativeArray2D<T> _mArray;

        #endregion

        public T[,] Items => _mArray.ToArray();
    }
}
