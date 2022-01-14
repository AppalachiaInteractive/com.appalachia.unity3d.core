using System;
using Unity.Collections;

namespace Appalachia.Core.Collections.Native
{
    /// <summary>
    ///     Extensions to <see cref="IDisposable" /> to support
    ///     <see cref="SharedDisposable{TDisposable}" />.
    /// </summary>
    public static class INativeDisposableExtensions
    {
        /// <summary>
        ///     Allocate memory and save the disposable
        /// </summary>
        /// <param name="disposable">
        ///     Disposable that is being shared
        /// </param>
        /// <param name="allocator">
        ///     Allocator to allocate and deallocate with. Must be valid.
        /// </param>
        public static SharedDisposable<TDisposable> Share<TDisposable>(
            this TDisposable disposable,
            Allocator allocator)
            where TDisposable : IDisposable
        {
            return new(disposable, allocator);
        }
    }
}
