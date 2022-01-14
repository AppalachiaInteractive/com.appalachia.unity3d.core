#region

using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

#endregion

namespace Appalachia.Core.Collections.Native
{
    public static class IJobParallelFor2DExtensions
    {
        public static unsafe void Run2D<T>(this T jobData, int length0, int length1)
            where T : struct, IJobParallelFor2D
        {
            var parameters = new JobsUtility.JobScheduleParameters(
                UnsafeUtility.AddressOf(ref jobData),
                ParallelFor2DJobStruct<T>.Initialize(),
                new JobHandle(),
                ScheduleMode.Run
            );
            JobsUtility.ScheduleParallelFor(ref parameters, length0 * length1, length0 * length1);
        }

        public static JobHandle Schedule2D<T, TE>(
            this T jobData,
            NativeArray2D<TE> array,
            int innerloopBatchCount,
            JobHandle dependsOn = default)
            where T : struct, IJobParallelFor2D
            where TE : struct
        {
            return Schedule2D(jobData, array.Length0, array.Length1, innerloopBatchCount, dependsOn);
        }

        public static unsafe JobHandle Schedule2D<T>(
            this T jobData,
            int length0,
            int length1,
            int innerloopBatchCount,
            JobHandle dependsOn = default)
            where T : struct, IJobParallelFor2D
        {
            var parameters = new JobsUtility.JobScheduleParameters(
                UnsafeUtility.AddressOf(ref jobData),
                ParallelFor2DJobStruct<T>.Initialize(),
                dependsOn,
                ScheduleMode.Parallel
            );
            return JobsUtility.ScheduleParallelFor(ref parameters, length0 * length1, innerloopBatchCount);
        }

        #region Nested type: ParallelFor2DJobStruct

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ParallelFor2DJobStruct<T>
            where T : struct, IJobParallelFor2D
        {
            public delegate void ExecuteJobFunction(
                    ref T data,
                    IntPtr additionalPtr,
                    IntPtr bufferRangePatchData,
                    ref JobRanges ranges,
                    int jobIndex)

                //where T : struct, IJobParallelFor2D
                ;

            #region Static Fields and Autoproperties

            public static IntPtr jobReflectionData;

            #endregion

            public static unsafe void Execute(
                ref T jobData,
                IntPtr additionalPtr,
                IntPtr bufferRangePatchData,
                ref JobRanges ranges,
                int jobIndex)
            {
                label_5:
                int beginIndex;
                int endIndex;
                if (!JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out beginIndex, out endIndex))
                {
                    return;
                }

                JobsUtility.PatchBufferMinMaxRanges(
                    bufferRangePatchData,
                    UnsafeUtility.AddressOf(ref jobData),
                    beginIndex,
                    endIndex - beginIndex
                );
                var num = endIndex;
                for (var index = beginIndex; index < num; ++index)
                {
                    jobData.Execute(index);
                }

                goto label_5;
            }

            public static IntPtr Initialize()
            {
                if (jobReflectionData == IntPtr.Zero)
                {
                    jobReflectionData = JobsUtility.CreateJobReflectionData(
                        typeof(T),
                        new ExecuteJobFunction(Execute)
                    );
                }

                return jobReflectionData;
            }
        }

        #endregion
    }
}
