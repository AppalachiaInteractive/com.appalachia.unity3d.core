#region

using System;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Preferences
{
    public static class PREFS
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(PREFS) + ".";

        private static readonly ProfilerMarker _PRF_REG = new(_PRF_PFX + nameof(REG));

        private static readonly ProfilerMarker _PRF_InternalRegistration =
            new(_PRF_PFX + nameof(InternalRegistration));

        private static readonly ProfilerMarker _PRF_InternalRegistrationEnum =
            new(_PRF_PFX + nameof(InternalRegistrationEnum));

        #endregion

        public static PREF<TR> REG<TR>(
            string grouping,
            string label,
            TR dv,
            TR low = default,
            TR high = default,
            int order = 0,
            bool reset = false,
            string header = null)
        {
            using (_PRF_REG.Auto())
            {
                var splits = label.Split('_');
                label = splits[splits.Length - 1];
                grouping = grouping.Trim('/').Trim();

                var key =
                    $"{grouping.ToLower().Replace(" ", string.Empty).Trim()}.{label.ToLower().Replace(" ", string.Empty).Trim()}";

                PREF_STATES._keys.Add(key);
                PREF_STATES._groupings.Add(grouping);

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._bools,
                    header,
                    out var br
                ))
                {
                    return br as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._ints,
                    header,
                    out var ir
                ))
                {
                    return ir as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._strings,
                    header,
                    out var sr
                ))
                {
                    return sr as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._bounds,
                    header,
                    out var bor
                ))
                {
                    return bor as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._colors,
                    header,
                    out var cr
                ))
                {
                    return cr as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._gradients,
                    header,
                    out var gr
                ))
                {
                    return gr as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._quaternions,
                    header,
                    out var qr
                ))
                {
                    return qr as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._doubles,
                    header,
                    out var fd
                ))
                {
                    return fd as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._floats,
                    header,
                    out var fr
                ))
                {
                    return fr as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._float2s,
                    header,
                    out var fr2
                ))
                {
                    return fr2 as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._float3s,
                    header,
                    out var fr3
                ))
                {
                    return fr3 as PREF<TR>;
                }

                if (InternalRegistration(
                    key,
                    grouping,
                    label,
                    dv,
                    low,
                    high,
                    order,
                    reset,
                    PREF_STATES._float4s,
                    header,
                    out var fr4
                ))
                {
                    return fr4 as PREF<TR>;
                }

                if (InternalRegistrationEnum<TR>(key, grouping, label, dv, order, reset, header, out var e))
                {
                    return e;
                }

                throw new NotSupportedException();
            }
        }

        private static bool InternalRegistration<TR>(
            string key,
            string grouping,
            string label,
            object defaultValue,
            object low,
            object high,
            int order,
            bool reset,
            PREF_STATE<TR> cached,
            string header,
            out PREF<TR> result)
        {
            using (_PRF_InternalRegistration.Auto())
            {
                if (defaultValue is TR dv)
                {
                    if (cached.Values.ContainsKey(key))
                    {
                        result = cached.Values[key];
                        return true;
                    }

                    var trLow = (TR) Convert.ChangeType(low,   typeof(TR));
                    var trHigh = (TR) Convert.ChangeType(high, typeof(TR));

                    var api = PREF_STATES.Get<TR>().API;
                    var instance = new PREF<TR>(
                        key,
                        grouping,
                        label,
                        dv,
                        trLow,
                        trHigh,
                        order,
                        reset,
                        api,
                        header
                    );

                    if (PREF_STATES._safeToAwaken)
                    {
                        instance.WakeUp();
                    }

                    cached.Add(key, instance);
                    result = instance;
                    return true;
                }

                result = null;
                return false;
            }
        }

        private static bool InternalRegistrationEnum<TR>(
            string key,
            string grouping,
            string label,
            object defaultValue,
            int order,
            bool reset,
            string header,
            out PREF<TR> result)
        {
            using (_PRF_InternalRegistrationEnum.Auto())
            {
                if (!typeof(TR).IsEnum)
                {
                    result = null;
                    return false;
                }

                var underlyingType = Enum.GetUnderlyingType(typeof(TR));

                if ((underlyingType == typeof(ulong)) ||
                    (underlyingType == typeof(uint)) ||
                    (underlyingType == typeof(ushort)))
                {
                    throw new NotSupportedException(
                        "Preferences does not support the use of enums with unsigned underlying types."
                    );
                }

                PREF_STATE<TR> state;

                if (typeof(TR).HasAttribute<FlagsAttribute>())
                {
                    state = PREF_STATES.GetFlagState<TR>();
                }
                else
                {
                    state = PREF_STATES.GetEnumState<TR>();
                }

                if (defaultValue is TR dv)
                {
                    if (state.Values.ContainsKey(key))
                    {
                        result = state.Values[key];
                        return true;
                    }

                    var api = PREF_STATES.Get<TR>().API;
                    var instance = new PREF<TR>(
                        key,
                        grouping,
                        label,
                        dv,
                        default,
                        default,
                        order,
                        reset,
                        api,
                        header
                    );

                    if (PREF_STATES._safeToAwaken)
                    {
                        instance.WakeUp();
                    }

                    state.Add(key, instance);
                    result = instance;
                    return true;
                }

                result = null;
                return false;
            }
        }
    }
}
