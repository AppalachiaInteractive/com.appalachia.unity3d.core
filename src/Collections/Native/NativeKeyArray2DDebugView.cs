namespace Appalachia.Core.Collections.Native
{
    internal sealed class NativeKeyArray2DDebugView<TK, TV>
        where TK : struct
        where TV : struct
    {
        public NativeKeyArray2DDebugView(NativeKeyArray2D<TK, TV> array)
        {
            _mArray = array;
        }

        #region Fields and Autoproperties

        private readonly NativeKeyArray2D<TK, TV> _mArray;

        #endregion

        public TK[] Keys => _mArray.ToKeyArray();

        public TV[,] Items => _mArray.ToArray();
    }
}
