#region

using System;

#endregion

namespace Appalachia.Core.Preferences
{
    public static class PREFS
    {
        public static PREF<TR> REG<TR>(
            string grouping,
            string label,
            TR dv,
            TR low = default,
            TR high = default,
            int order = 0,
            bool reset = false)
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
                out var fr4
            ))
            {
                return fr4 as PREF<TR>;
            }

            if (InternalRegistrationEnum<TR>(key, grouping, label, dv, order, reset, out var e))
            {
                return e;
            }

            throw new NotSupportedException();
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
            out PREF<TR> result)
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

                var instance = new PREF<TR>(key, grouping, label, dv, trLow, trHigh, order, reset);

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

        private static bool InternalRegistrationEnum<TR>(
            string key,
            string grouping,
            string label,
            object defaultValue,
            int order,
            bool reset,
            out PREF<TR> result)
        {
            var state = PREF_STATES.GetEnumState<TR>();

            if (defaultValue is TR dv)
            {
                if (state.Values.ContainsKey(key))
                {
                    result = state.Values[key];
                    return true;
                }

                var instance = new PREF<TR>(
                    key,
                    grouping,
                    label,
                    dv,
                    default,
                    default,
                    order,
                    reset
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
