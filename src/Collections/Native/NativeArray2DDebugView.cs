namespace Appalachia.Core.Collections.Native
{
    internal sealed class NativeArray2DDebugView<T>
        where T : struct
    {
        public NativeArray2DDebugView(NativeArray2D<T> array)
        {
            _mArray = array;
        }

        private readonly NativeArray2D<T> _mArray;

        public T[,] Items => _mArray.ToArray();
    }
}
