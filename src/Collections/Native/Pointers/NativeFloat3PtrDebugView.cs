using Unity.Mathematics;

namespace Appalachia.Core.Collections.Native.Pointers
{
    /// <summary>
    ///     Provides a debugger view of <see cref="NativeFloat3Ptr" />.
    /// </summary>
    internal sealed class NativeFloat3PtrDebugView
    {
        /// <summary>
        ///     Create the debugger view
        /// </summary>
        /// <param name="ptr">
        ///     The object to provide a debugger view for
        /// </param>
        public NativeFloat3PtrDebugView(NativeFloat3Ptr ptr)
        {
            m_Ptr = ptr;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The object to provide a debugger view for
        /// </summary>
        private NativeFloat3Ptr m_Ptr;

        #endregion

        /// <summary>
        ///     Get the viewed object's value
        /// </summary>
        /// <value>
        ///     The viewed object's value
        /// </value>
        public float3 Value => m_Ptr.Value;
    }
}
