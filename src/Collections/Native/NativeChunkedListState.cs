using System.Runtime.InteropServices;
using Unity.Collections;

namespace Appalachia.Core.Collections.Native
{
    /// <summary>
    ///     The state of a <see cref="NativeChunkedList{T}" />. Shared among
    ///     instances of the struct via a pointer to unmanaged memory. This has no
    ///     type parameters, so it can be used by all list types.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct NativeChunkedListState
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     Allocator used to create <see cref="m_Chunks" /> and
        ///     <see cref="NativeChunkedListChunk.m_Values" />.
        /// </summary>
        internal Allocator m_AllocatorLabel;

        /// <summary>
        ///     Number of elements that can be contained
        /// </summary>
        internal int m_Capacity;

        /// <summary>
        ///     The number of elements in a chunk
        /// </summary>
        internal int m_ChunkLength;

        /// <summary>
        ///     Number of chunks that can be contained
        /// </summary>
        internal int m_ChunksCapacity;

        /// <summary>
        ///     Number of chunks contained
        /// </summary>
        internal int m_ChunksLength;

        /// <summary>
        ///     Number of elements contained
        /// </summary>
        internal int m_Length;

        /// <summary>
        ///     Array of chunks of elements. Length is <see cref="m_Capacity" />.
        /// </summary>
        internal NativeChunkedListChunk* m_Chunks;

        #endregion
    }
}
