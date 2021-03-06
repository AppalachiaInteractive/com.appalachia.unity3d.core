#region

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

#endregion

namespace Appalachia.Core.Collections.Native.Pointers
{
	/// <summary>
	///     A pointer to an double stored in native (i.e. unmanaged) memory
	/// </summary>
	[NativeContainer]
    [NativeContainerSupportsDeallocateOnJobCompletion]
    [DebuggerTypeProxy(typeof(NativeDoublePtrDebugView))]
    [DebuggerDisplay("Value = {" + nameof(Value) + "}")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NativeDoublePtr : IDisposable
    {
	    /// <summary>
	    ///     Allocate memory and set the initial value
	    /// </summary>
	    /// <param name="allocator">
	    ///     Allocator to allocate and deallocate with. Must be valid.
	    /// </param>
	    /// <param name="initialValue">
	    ///     Initial value of the allocated memory
	    /// </param>
	    public NativeDoublePtr(Allocator allocator, double initialValue = 0)
	    {
		    // Require a valid allocator
		    if (allocator <= Allocator.None)
		    {
			    throw new ArgumentException(
				    "Allocator must be Temp, TempJob or Persistent",
				    nameof(allocator)
			    );
		    }

		    // Allocate the memory for the value
		    m_Buffer = (double*)UnsafeUtility.Malloc(
			    sizeof(double),
			    UnsafeUtility.AlignOf<double>(),
			    allocator
		    );

		    // Store the allocator to use when deallocating
		    m_AllocatorLabelLabel = allocator;

		    // Create the dispose sentinel
#if ENABLE_UNITY_COLLECTIONS_CHECKS
#if UNITY_2018_3_OR_NEWER
		    DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0, allocator);
#else
			DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0);
#endif
#endif

		    // Set the initial value
		    *m_Buffer = initialValue;
	    }

	    #region Fields and Autoproperties

	    /// <summary>
	    ///     Allocator used to create the backing memory
	    ///     This field must be named this way to comply with
	    ///     [NativeContainerSupportsDeallocateOnJobCompletion]
	    /// </summary>
	    internal readonly Allocator m_AllocatorLabelLabel;

	    /// <summary>
	    ///     Pointer to the value in native memory. Must be named exactly this
	    ///     way to allow for [NativeContainerSupportsDeallocateOnJobCompletion]
	    /// </summary>
	    [NativeDisableUnsafePtrRestriction]
	    internal double* m_Buffer;

	    #endregion

	    /// <summary>
	    ///     Check if the underlying unmanaged memory has been created and not
	    ///     freed via a call to <see cref="Dispose" />.
	    ///     This operation has no access requirements.
	    ///     This operation is O(1).
	    /// </summary>
	    /// <value>
	    ///     Initially true when a non-default constructor is called but
	    ///     initially false when the default constructor is used. After
	    ///     <see cref="Dispose" /> is called, this becomes false. Note that
	    ///     calling <see cref="Dispose" /> on one copy of this object doesn't
	    ///     result in this becoming false for all copies if it was true before.
	    ///     This property should <i>not</i> be used to check whether the object
	    ///     is usable, only to check whether it was <i>ever</i> usable.
	    /// </value>
	    public bool IsCreated => m_Buffer != null;

	    /// <summary>
	    ///     Get or set the contained value
	    ///     This operation requires read access to the node for 'get' and write
	    ///     access to the node for 'set'.
	    /// </summary>
	    /// <value>
	    ///     The contained value
	    /// </value>
	    public double Value
	    {
		    get
		    {
			    CheckReadAccess();
			    return *m_Buffer;
		    }

		    [WriteAccessRequired]
		    set
		    {
			    CheckWriteAccess();
			    *m_Buffer = value;
		    }
	    }

	    /// <summary>
	    ///     Get a version of this object suitable for use in a ParallelFor job
	    /// </summary>
	    /// <returns>
	    ///     A version of this object suitable for use in a ParallelFor job
	    /// </returns>
	    public Parallel GetParallel()
	    {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
		    var parallel = new Parallel(m_Buffer, m_Safety);
		    AtomicSafetyHandle.UseSecondaryVersion(ref parallel.m_Safety);
#else
			Parallel parallel = new Parallel(m_Buffer);
#endif
		    return parallel;
	    }

	    /// <summary>
	    ///     Throw an exception if the object isn't readable
	    /// </summary>
	    [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	    private void CheckReadAccess()
	    {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
		    AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
	    }

	    /// <summary>
	    ///     Throw an exception if the object isn't writable
	    /// </summary>
	    [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	    private void CheckWriteAccess()
	    {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
		    AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
	    }

	    #region IDisposable Members

	    /// <summary>
	    ///     Release the object's unmanaged memory. Do not use it after this. Do
	    ///     not call <see cref="Dispose" /> on copies of the object either.
	    ///     This operation requires write access.
	    ///     This complexity of this operation is O(1) plus the allocator's
	    ///     deallocation complexity.
	    /// </summary>
	    [WriteAccessRequired]
	    public void Dispose()
	    {
		    CheckWriteAccess();

// Make sure we're not double-disposing
#if ENABLE_UNITY_COLLECTIONS_CHECKS
#if UNITY_2018_3_OR_NEWER
		    DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
#else
			DisposeSentinel.Dispose(m_Safety, ref m_DisposeSentinel);
#endif
#endif

		    UnsafeUtility.Free(m_Buffer, m_AllocatorLabelLabel);
		    m_Buffer = null;
	    }

	    #endregion

	    #region Nested type: Parallel

	    /// <summary>
	    ///     An atomic write-only version of the object suitable for use in a
	    ///     ParallelFor job
	    /// </summary>
	    [NativeContainer]
        [NativeContainerIsAtomicWriteOnly]
        public struct Parallel
        {
	        /// <summary>
	        ///     Pointer to the value in native memory
	        /// </summary>
	        [NativeDisableUnsafePtrRestriction]
            internal readonly double* m_Buffer;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
	        /// <summary>
	        ///     A handle to information about what operations can be safely
	        ///     performed on the object at any given time.
	        /// </summary>
	        internal AtomicSafetyHandle m_Safety;

	        /// <summary>
	        ///     Create a parallel version of the object
	        /// </summary>
	        /// <param name="value">
	        ///     Pointer to the value
	        /// </param>
	        /// <param name="safety">
	        ///     Atomic safety handle for the object
	        /// </param>
	        internal Parallel(double* value, AtomicSafetyHandle safety)
            {
                m_Buffer = value;
                m_Safety = safety;
            }
#else
			/// <summary>
			/// Create a parallel version of the object
			/// </summary>
			/// 
			/// <param name="value">
			/// Pointer to the value
			/// </param>
			internal Parallel(double* value)
			{
				m_Buffer = value;
			}
#endif

	        /// <summary>
	        ///     Increment the stored value
	        /// </summary>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Increment()
            {
                CheckWriteAccess();
                InterlockedEx.Increment(ref *m_Buffer);
            }

	        /// <summary>
	        ///     Decrement the stored value
	        /// </summary>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Decrement()
            {
                CheckWriteAccess();
                InterlockedEx.Decrement(ref *m_Buffer);
            }

	        /// <summary>
	        ///     Add to the stored value
	        /// </summary>
	        /// <param name="value">
	        ///     Value to add. Use negative values for subtraction.
	        /// </param>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Add(double value)
            {
                CheckWriteAccess();
                InterlockedEx.Add(ref *m_Buffer, value);
            }

	        /// <summary>
	        ///     Add to the stored value
	        /// </summary>
	        /// <param name="value">
	        ///     Value to add. Use negative values for subtraction.
	        /// </param>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Subtract(double value)
            {
                CheckWriteAccess();
                InterlockedEx.Subtract(ref *m_Buffer, value);
            }

	        /// <summary>
	        ///     Add to the stored value
	        /// </summary>
	        /// <param name="value">
	        ///     Value to add. Use negative values for subtraction.
	        /// </param>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Multiply(double value)
            {
                CheckWriteAccess();
                InterlockedEx.Multiply(ref *m_Buffer, value);
            }

	        /// <summary>
	        ///     Add to the stored value
	        /// </summary>
	        /// <param name="value">
	        ///     Value to add. Use negative values for subtraction.
	        /// </param>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Divide(double value)
            {
                CheckWriteAccess();
                InterlockedEx.Divide(ref *m_Buffer, value);
            }

	        /// <summary>
	        ///     Add to the stored value
	        /// </summary>
	        /// <param name="value">
	        ///     Value to add. Use negative values for subtraction.
	        /// </param>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Min(double value)
            {
                CheckWriteAccess();
                InterlockedEx.Min(ref *m_Buffer, value);
            }

	        /// <summary>
	        ///     Add to the stored value
	        /// </summary>
	        /// <param name="value">
	        ///     Value to add. Use negative values for subtraction.
	        /// </param>
	        /// <returns>
	        ///     This object
	        /// </returns>
	        [WriteAccessRequired]
            public void Max(double value)
            {
                CheckWriteAccess();
                InterlockedEx.Max(ref *m_Buffer, value);
            }

	        /// <summary>
	        ///     Throw an exception if the object isn't writable
	        /// </summary>
	        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
            private void CheckWriteAccess()
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
            }
        }

	    #endregion

        // These fields are all required when safety checks are enabled
#if ENABLE_UNITY_COLLECTIONS_CHECKS
	    /// <summary>
	    ///     A handle to information about what operations can be safely
	    ///     performed on the object at any given time.
	    /// </summary>
	    internal AtomicSafetyHandle m_Safety;

	    /// <summary>
	    ///     A handle that can be used to tell if the object has been disposed
	    ///     yet or not, which allows for error-checking double disposal.
	    /// </summary>
	    [NativeSetClassTypeToNullOnSchedule]
        private DisposeSentinel m_DisposeSentinel;
#endif
    }
}
