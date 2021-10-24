namespace Appalachia.Core.Collections.Native.Pointers
{
    /// <summary>
    ///     Provides a debugger view of <see cref="NativeLongPtr" />.
    /// </summary>
    internal sealed class NativeLongPtrDebugView
    {
        /// <summary>
        ///     Create the debugger view
        /// </summary>
        /// <param name="ptr">
        ///     The object to provide a debugger view for
        /// </param>
        public NativeLongPtrDebugView(NativeLongPtr ptr)
        {
            m_Ptr = ptr;
        }

        /// <summary>
        ///     The object to provide a debugger view for
        /// </summary>
        private NativeLongPtr m_Ptr;

        /// <summary>
        ///     Get the viewed object's value
        /// </summary>
        /// <value>
        ///     The viewed object's value
        /// </value>
        public long Value => m_Ptr.Value;
    }
}
