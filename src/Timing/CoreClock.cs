#region

using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Timing
{
    public static class CoreClock
    {
        private const string _PRF_PFX = nameof(CoreClock) + ".";

        private static double _worldAgeInSeconds;
        private static double _worldAgeInSecondsSetAt;

        private static readonly ProfilerMarker _PRF_CoreClock = new(_PRF_PFX + nameof(CoreClock));

        private static readonly ProfilerMarker _PRF_Tick = new(_PRF_PFX + nameof(Tick));

        public static double Now { get; private set; }
        public static double VisualDelta { get; private set; }
        public static double PhysicalDelta { get; private set; }
        public static double TimeScale { get; private set; }
        public static double TimeSinceLevelLoad { get; private set; }

        public static float NowF { get; private set; }
        public static float VisualDeltaF { get; private set; }
        public static float PhysicalDeltaF { get; private set; }
        public static float TimeScaleF { get; private set; }
        public static float TimeSinceLevelLoadF { get; private set; }

        public static double WorldAgeInSeconds =>
            _worldAgeInSeconds + _worldAgeInSecondsSetAt.TimeSince();

        public static void Tick()
        {
            using (_PRF_Tick.Auto())
            {
                Now = Time.time;
                VisualDelta = Time.deltaTime;
                PhysicalDelta = Time.fixedDeltaTime;
                TimeScale = Time.timeScale;
                TimeSinceLevelLoad = Time.timeSinceLevelLoad;
                NowF = Time.time;
                VisualDeltaF = Time.deltaTime;
                PhysicalDeltaF = Time.fixedDeltaTime;
                TimeScaleF = Time.timeScale;
                TimeSinceLevelLoadF = Time.timeSinceLevelLoad;
            }
        }

        public static float TimeSince(this float timeSince)
        {
            return (float) (Now - timeSince);
        }

        public static double TimeSince(this double timeSince)
        {
            return Now - timeSince;
        }

        public static void SetWorldAgeInSeconds(double worldAgeInSeconds)
        {
            _worldAgeInSeconds = worldAgeInSeconds;
            _worldAgeInSecondsSetAt = Now;
        }
    }
}
