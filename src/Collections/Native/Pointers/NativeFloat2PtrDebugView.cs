using Unity.Mathematics;

namespace Appalachia.Core.Collections.Native.Pointers
{
    /// <summary>
    ///     Provides a debugger view of <see cref="NativeFloat2Ptr" />.
    /// </summary>
    internal sealed class NativeFloat2PtrDebugView
    {
        /// <summary>
        ///     Create the debugger view
        /// </summary>
        /// <param name="ptr">
        ///     The object to provide a debugger view for
        /// </param>
        public NativeFloat2PtrDebugView(NativeFloat2Ptr ptr)
        {
            m_Ptr = ptr;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The object to provide a debugger view for
        /// </summary>
        private NativeFloat2Ptr m_Ptr;

        #endregion

        /// <summary>
        ///     Get the viewed object's value
        /// </summary>
        /// <value>
        ///     The viewed object's value
        /// </value>
        public float2 Value => m_Ptr.Value;
    }
}
