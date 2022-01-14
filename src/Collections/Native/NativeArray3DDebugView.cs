namespace Appalachia.Core.Collections.Native
{
    internal sealed class NativeArray3DDebugView<T>
        where T : struct
    {
        public NativeArray3DDebugView(NativeArray3D<T> array)
        {
            _mArray = array;
        }

        #region Fields and Autoproperties

        private readonly NativeArray3D<T> _mArray;

        #endregion

        public T[,,] Items => _mArray.ToArray();
    }
}
