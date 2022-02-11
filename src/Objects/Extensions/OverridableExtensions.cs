using System;
using Appalachia.Core.Objects.Models;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Extensions
{
    public static class OverridableExtensions
    {
        public static bool IsOverridable(this Type t)
        {
            using (_PRF_IsOverridable.Auto())
            {
                var overridableType = typeof(Overridable<,>);

                while ((t != null) &&
                       (t is { IsConstructedGenericType: true } ||
                        t.BaseType is { IsConstructedGenericType: true }))
                {
                    if (t.IsGenericType)
                    {
                        var genericTypeDefinition = t.GetGenericTypeDefinition();

                        if (genericTypeDefinition == overridableType)
                        {
                            return true;
                        }
                    }

                    t = t.BaseType;
                }

                return false;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(OverridableExtensions) + ".";

        private static readonly ProfilerMarker _PRF_IsOverridable =
            new ProfilerMarker(_PRF_PFX + nameof(IsOverridable));

        #endregion
    }
}
