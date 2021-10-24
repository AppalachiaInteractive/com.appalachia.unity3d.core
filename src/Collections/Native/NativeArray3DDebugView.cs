namespace Appalachia.Core.Collections.Native
{
    internal sealed class NativeArray3DDebugView<T>
        where T : struct
    {
        public NativeArray3DDebugView(NativeArray3D<T> array)
        {
            _mArray = array;
        }

        private readonly NativeArray3D<T> _mArray;

        public T[,,] Items => _mArray.ToArray();
    }
}
