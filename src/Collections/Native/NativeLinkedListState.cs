using System.Runtime.InteropServices;
using Unity.Collections;

namespace Appalachia.Core.Collections.Native
{
    /// <summary>
    ///     The state of a <see cref="NativeLinkedList{T}" />. Shared among instances
    ///     of the struct via a pointer to unmanaged memory. This has no type
    ///     parameters, so it can be used by all list types.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct NativeLinkedListState
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     Allocator used to create the backing arrays
        /// </summary>
        internal Allocator m_AllocatorLabel;

        /// <summary>
        ///     Number of nodes that can be contained
        /// </summary>
        internal int m_Capacity;

        /// <summary>
        ///     Index of the first node in the list or -1 if there are no nodes in
        ///     the list
        /// </summary>
        internal int m_HeadIndex;

        /// <summary>
        ///     Number of nodes contained
        /// </summary>
        internal int m_Length;

        /// <summary>
        ///     Index of the last node in the list or -1 if there are no nodes in
        ///     the list
        /// </summary>
        internal int m_TailIndex;

        /// <summary>
        ///     Version of enumerators that are valid for this list. This starts at
        ///     1 and increases by one with each change that invalidates the list's
        ///     enumerators.
        /// </summary>
        internal int m_Version;

        /// <summary>
        ///     Each node's next node index. Indices correspond with values.
        /// </summary>
        internal int* m_NextIndexes;

        /// <summary>
        ///     Each node's previous node index. Indices correspond with values.
        /// </summary>
        internal int* m_PrevIndexes;

        /// <summary>
        ///     Each node's value. Indices correspond with nextIndexes.
        /// </summary>
        internal void* m_Values;

        #endregion
    }
}
