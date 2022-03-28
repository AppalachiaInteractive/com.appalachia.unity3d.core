using System;
using Appalachia.Core.Objects.Models.Selectable;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Extensions
{
    public static class SelectableStateExtensions
    {
        private static readonly ProfilerMarker _PRF_IsSelectableState =
            new ProfilerMarker(_PRF_PFX + nameof(IsSelectableState));

        
        public static bool IsSelectableState(this Type t)
        {
            using (_PRF_IsSelectableState.Auto())
            {
                var type = typeof(SelectableStateParameter<,>);

                while ((t != null) &&
                       (t is { IsConstructedGenericType: true } ||
                        t.BaseType is { IsConstructedGenericType: true }))
                {
                    if (t.IsGenericType)
                    {
                        var genericTypeDefinition = t.GetGenericTypeDefinition();

                        if (genericTypeDefinition == type)
                        {
                            return true;
                        }
                    }

                    t = t.BaseType;
                }

                return false;
            }
        }
        
        public static SelectableState GetPrimaryState(this SelectableState state)
        {
            using (_PRF_GetPrimaryState.Auto())
            {
                if (state.HasFlag(SelectableState.Disabled))
                {
                    return SelectableState.Disabled;
                }

                if (state.HasFlag(SelectableState.Dragging))
                {
                    return SelectableState.Dragging;
                }

                if (state.HasFlag(SelectableState.Pressed))
                {
                    return SelectableState.Pressed;
                }

                if (state.HasFlag(SelectableState.Selected))
                {
                    return SelectableState.Selected;
                }

                if (state.HasFlag(SelectableState.Hovering))
                {
                    return SelectableState.Hovering;
                }

                return SelectableState.Normal;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SelectableStateExtensions) + ".";

        private static readonly ProfilerMarker _PRF_GetPrimaryState =
            new ProfilerMarker(_PRF_PFX + nameof(GetPrimaryState));

        #endregion
    }
}
