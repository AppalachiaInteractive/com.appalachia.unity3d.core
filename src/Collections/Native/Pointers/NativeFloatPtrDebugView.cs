namespace Appalachia.Core.Collections.Native.Pointers
{
    /// <summary>
    ///     Provides a debugger view of <see cref="NativeFloatPtr" />.
    /// </summary>
    internal sealed class NativeFloatPtrDebugView
    {
        /// <summary>
        ///     Create the debugger view
        /// </summary>
        /// <param name="ptr">
        ///     The object to provide a debugger view for
        /// </param>
        public NativeFloatPtrDebugView(NativeFloatPtr ptr)
        {
            m_Ptr = ptr;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The object to provide a debugger view for
        /// </summary>
        private NativeFloatPtr m_Ptr;

        #endregion

        /// <summary>
        ///     Get the viewed object's value
        /// </summary>
        /// <value>
        ///     The viewed object's value
        /// </value>
        public float Value => m_Ptr.Value;
    }
}
