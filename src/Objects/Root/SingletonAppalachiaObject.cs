#region

using System;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions.Debugging;
using Appalachia.Utility.Strings;
using Unity.Profiling;

#endregion

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class SingletonAppalachiaObject<T> : AppalachiaObject<T>
        where T : SingletonAppalachiaObject<T>
    {
        private static async AppaTask<T> FindInstanceInSingletonLookup()
        {
            var firstFound = await AppalachiaRepository.Find<T>();

            if (firstFound != null)
            {
                return firstFound;
            }

            APPADEBUG.BREAKPOINT(
                () => ZString.Format(
                    "Could not find singleton object of type [{0}].  Is it addressable?",
                    typeof(T).FormatForLogging()
                ),
                AppalachiaRepository
            );

            return null;
        }

        private static async AppaTask<T> GetSingletonInternal()
        {
            using (_PRF_InitializeSingletonUsage.Auto())
            {
                if (instance != null)
                {
                    return instance;
                }

                var foundInstance = await FindInstanceInSingletonLookup();

                if (foundInstance != null)
                {
                    return foundInstance;
                }

#if UNITY_EDITOR
                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    throw new NotSupportedException(
                        ZString.Format(
                            "Cannot invoke {0} during Play mode unless the asset is addressable.",
                            nameof(_PRF_InitializeSingletonUsage)
                        )
                    );
                }

                StaticContext.Log.Warn(
                    ZString.Format(
                        "Creating new singleton object [{0}]",
                        typeof(T).Name.FormatNameForLogging()
                    )
                );

                foundInstance = await CreateAndSaveSingleton();

                foundInstance.MarkAsModified();
#endif
                return foundInstance;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SingletonAppalachiaObject<T>) + ".";

        private static readonly ProfilerMarker _PRF_InitializeSingletonUsage =
            new(_PRF_PFX + nameof(GetSingletonInternal));

        #endregion
    }
}
