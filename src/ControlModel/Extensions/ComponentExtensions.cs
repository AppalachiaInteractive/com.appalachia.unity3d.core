using Appalachia.Core.ControlModel.Components.Contracts;
using Appalachia.Core.Objects.Root;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.ControlModel.Extensions
{
    public static class ComponentExtensions
    {
        public static void Disable<T>(this T component)
            where T : Component
        {
            using (_PRF_Disable.Auto())
            {
                if (component is not Behaviour b)
                {
                    return;
                }

                if (b.enabled)
                {
                    b.enabled = false;
                }
            }
        }

        public static void Enable<T>(this T component)
            where T : Component
        {
            using (_PRF_Enable.Auto())
            {
                if (component is not Behaviour b)
                {
                    return;
                }

                if (!b.enabled)
                {
                    b.enabled = true;
                }
            }
        }

        public static void Enable<TComponent, TComponentConfig>(this TComponent component, TComponentConfig config)
            where TComponent : Component
            where TComponentConfig : IAppaComponentConfig<TComponent, TComponentConfig>, IRemotelyEnabled, new()
        {
            using (_PRF_Enable.Auto())
            {
                if (component is Behaviour b)
                {
                    if (config.ShouldEnable)
                    {
                        b.enabled = true;
                    }
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ComponentExtensions) + ".";
        private static readonly ProfilerMarker _PRF_Enable = new ProfilerMarker(_PRF_PFX + nameof(Enable));
        private static readonly ProfilerMarker _PRF_Disable = new ProfilerMarker(_PRF_PFX + nameof(Disable));

        #endregion
    }
}
