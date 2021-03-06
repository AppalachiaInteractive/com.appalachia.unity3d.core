using System;

namespace Appalachia.Core.Collections.Native
{
    /// <summary>
    ///     Provides a debugger view of <see cref="SharedDisposable{T}" />.
    /// </summary>
    /// <typeparam name="TDisposable">
    ///     Type of disposable that is shared.
    /// </typeparam>
    internal sealed class SharedDisposableDebugView<TDisposable>
        where TDisposable : IDisposable
    {
        /// <summary>
        ///     Create the debugger view
        /// </summary>
        /// <param name="ptr">
        ///     The object to provide a debugger view for
        /// </param>
        public SharedDisposableDebugView(SharedDisposable<TDisposable> ptr)
        {
            m_Ptr = ptr;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     The object to provide a debugger view for
        /// </summary>
        private SharedDisposable<TDisposable> m_Ptr;

        #endregion

        /// <summary>
        ///     Get the viewed object's disposable
        /// </summary>
        /// <value>
        ///     The viewed object's disposable
        /// </value>
        public TDisposable Disposable => m_Ptr.Value;
    }
}
