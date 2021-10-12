using System;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Timing
{
    [Serializable]
    [InlineProperty]
    public struct Duration
    {
        [HorizontalGroup(.33f)]
        [PropertyRange(nameof(rangeMinimum), nameof(rangeMaximum))]
        [HideLabel]
        public float value;

        [HorizontalGroup(.33f)]
        [HideLabel]
        public TimeUnit unit;

        [HorizontalGroup(.33f)]
        [ToggleLeft]
        [LabelWidth(80f)]
        public bool inRealTime;

        public float InSeconds =>
            unit == TimeUnit.Milliseconds
                ? value / 1000f
                : unit == TimeUnit.Seconds
                    ? value
                    : unit == TimeUnit.Minutes
                        ? value * 60f
                        : unit == TimeUnit.Hours
                            ? value * 60f * 60f
                            : value * 60f * 60f * 24f;

        private float rangeMinimum =>
            unit == TimeUnit.Milliseconds
                ? 0f
                : unit == TimeUnit.Seconds
                    ? 0f
                    : unit == TimeUnit.Minutes
                        ? 0f
                        : unit == TimeUnit.Hours
                            ? 0f
                            : 0f;

        private float rangeMaximum =>
            unit == TimeUnit.Milliseconds
                ? 1000f
                : unit == TimeUnit.Seconds
                    ? 60f
                    : unit == TimeUnit.Minutes
                        ? 60f
                        : unit == TimeUnit.Hours
                            ? 24f
                            : 30f;

        public static Duration ONE_SECOND()
        {
            return new() {value = 1.0f, unit = TimeUnit.Seconds};
        }

        public static Duration ONE_MINUTE()
        {
            return new() {value = 1.0f, unit = TimeUnit.Minutes};
        }

        public static Duration ONE_HOUR()
        {
            return new() {value = 1.0f, unit = TimeUnit.Hours};
        }

        public static Duration ONE_DAY()
        {
            return new() {value = 1.0f, unit = TimeUnit.Days};
        }
    }
}
