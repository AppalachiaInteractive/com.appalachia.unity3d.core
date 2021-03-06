namespace Appalachia.Core.Collections.Native
{
    /// <summary>
    ///     One chunk of elements in the list
    /// </summary>
    internal unsafe struct NativeChunkedListChunk
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     Array of elements in this chunk. Length is always equal to
        ///     <see cref="NativeChunkedListState.m_ChunkLength" />.
        /// </summary>
        internal void* m_Values;

        #endregion
    }
}
