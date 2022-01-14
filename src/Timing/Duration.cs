using System;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Timing
{
    [Serializable]
    [InlineProperty]
    public struct Duration
    {
        #region Fields and Autoproperties

        [HorizontalGroup(.33f)]
        [ToggleLeft]
        [LabelWidth(80f)]
        public bool inRealTime;

        [HorizontalGroup(.33f)]
        [PropertyRange(nameof(rangeMinimum), nameof(rangeMaximum))]
        [HideLabel]
        public float value;

        [HorizontalGroup(.33f)]
        [HideLabel]
        public TimeUnit unit;

        #endregion

        public float InSeconds =>
            unit switch
            {
                TimeUnit.Milliseconds => value / 1000f,
                TimeUnit.Seconds      => value,
                TimeUnit.Minutes      => value * 60f,
                TimeUnit.Hours        => value * 60f * 60f,
                _                     => value * 60f * 60f * 24f
            };

        private float rangeMaximum =>
            unit switch
            {
                TimeUnit.Milliseconds => 1000f,
                TimeUnit.Seconds      => 60f,
                TimeUnit.Minutes      => 60f,
                TimeUnit.Hours        => 24f,
                _                     => 30f
            };

        private float rangeMinimum =>
            unit switch
            {
                TimeUnit.Milliseconds => 0f,
                TimeUnit.Seconds      => 0f,
                TimeUnit.Minutes      => 0f,
                TimeUnit.Hours        => 0f,
                _                     => 0f
            };

        public static Duration ONE_DAY()
        {
            return new() { value = 1.0f, unit = TimeUnit.Days };
        }

        public static Duration ONE_HOUR()
        {
            return new() { value = 1.0f, unit = TimeUnit.Hours };
        }

        public static Duration ONE_MINUTE()
        {
            return new() { value = 1.0f, unit = TimeUnit.Minutes };
        }

        public static Duration ONE_SECOND()
        {
            return new() { value = 1.0f, unit = TimeUnit.Seconds };
        }
    }
}
