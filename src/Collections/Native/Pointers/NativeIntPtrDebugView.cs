namespace Appalachia.Core.Collections.Native.Pointers
{
    /// <summary>
    ///     Provides a debugger view of <see cref="NativeIntPtr" />.
    /// </summary>
    internal sealed class NativeIntPtrDebugView
    {
        /// <summary>
        ///     Create the debugger view
        /// </summary>
        /// <param name="ptr">
        ///     The object to provide a debugger view for
        /// </param>
        public NativeIntPtrDebugView(NativeIntPtr ptr)
        {
            m_Ptr = ptr;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The object to provide a debugger view for
        /// </summary>
        private NativeIntPtr m_Ptr;

        #endregion

        /// <summary>
        ///     Get the viewed object's value
        /// </summary>
        /// <value>
        ///     The viewed object's value
        /// </value>
        public int Value => m_Ptr.Value;
    }
}
