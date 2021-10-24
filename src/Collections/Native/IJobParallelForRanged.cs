#region

using Unity.Jobs.LowLevel.Unsafe;

#endregion

namespace Appalachia.Core.Collections.Native
{
    /// <summary>
    ///     A ParallelFor job type that executes on ranges of indices
    /// </summary>
    [JobProducerType(typeof(IJobParallelForRangedExtensions.ParallelForJobStruct<>))]
    public interface IJobParallelForRanged
    {
        /// <summary>
        ///     Execute on the given range of indices, inclusive of the start and
        ///     exclusive of the end
        /// </summary>
        /// <param name="startIndex">
        ///     First index to execute on
        /// </param>
        /// <param name="endIndex">
        ///     One greater than the last index to execute on
        /// </param>
        void Execute(int startIndex, int endIndex);
    }
}
