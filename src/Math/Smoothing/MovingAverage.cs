using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public abstract class MovingAverage<T>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("windowSize")]
        [SerializeField]
        [ShowInInspector]
        [PropertyRange(1, 64)]
        [SmartLabel, LabelText("Samples")]
        public int maximumSampleCount = 16;

        private readonly Queue<T> _samples = new();
        private bool _hasDequeuedPreviously;
        private bool _hasPreviousSamples;

        [SerializeField]
        [ShowInInspector]
        [SmartLabel]
        private T _average;

        [SerializeField]
        [ShowInInspector]
        [SmartLabel, LabelText("Delta")]
        private T _averageDelta;

        private T _currentSum;
        private T _currentSumOfDelta;
        private T _lastDequeuedSample;
        private T _previousSample;

        #endregion

        public T PreviousSample => _previousSample;

        public T Average
        {
            get => _average;
            private set => _average = value;
        }

        public T AverageDelta
        {
            get => _averageDelta;
            private set => _averageDelta = value;
        }

        /// <summary>
        ///     Computes a new windowed average each time a new sample arrives
        /// </summary>
        /// <param name="newSample"></param>
        public void AddSample(T newSample)
        {
            using (_PRF_AddSample.Auto())
            {
                _samples.Enqueue(newSample);

                var willRemoveSample = _samples.Count > maximumSampleCount;
                T removedSample = default;

                if (willRemoveSample)
                {
                    removedSample = _samples.Dequeue();
                }

                CalculateNewAverage(newSample, willRemoveSample, removedSample);

                CalculateNewAverageDelta(newSample, willRemoveSample, removedSample);

                _previousSample = newSample;
            }
        }

        public void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                _currentSum = default;
                _average = default;
                _previousSample = default;
                _lastDequeuedSample = default;
                _currentSumOfDelta = default;
                _averageDelta = default;
                _hasDequeuedPreviously = false;
                _samples?.Clear();
            }
        }

        protected abstract T Add(T a, T b);

        protected abstract T Divide(T a, int divisor);

        protected abstract T Subtract(T a, T b);

        private void CalculateNewAverage(T newSample, bool didRemoveSample, T removedSample)
        {
            using (_PRF_CalculateNewAverage.Auto())
            {
                if (didRemoveSample)
                {
                    _currentSum = Subtract(_currentSum, removedSample);
                }

                _currentSum = Add(_currentSum, newSample);

                Average = Divide(_currentSum, _samples.Count);
            }
        }

        private void CalculateNewAverageDelta(T newSample, bool didRemoveSample, T removedSample)
        {
            using (_PRF_CalculateNewAverageDelta.Auto())
            {
                if (!_hasPreviousSamples)
                {
                    _hasPreviousSamples = true;
                    return;
                }

                var delta = Subtract(newSample, _previousSample);

                _currentSumOfDelta = Add(_currentSumOfDelta, delta);

                if (didRemoveSample)
                {
                    if (_hasDequeuedPreviously)
                    {
                        var lastDequeuedDelta = Subtract(removedSample, _lastDequeuedSample);

                        _currentSumOfDelta = Subtract(_currentSumOfDelta, lastDequeuedDelta);
                    }

                    _hasDequeuedPreviously = true;
                }

                AverageDelta = Divide(_currentSumOfDelta, _samples.Count);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(MovingAverage<T>) + ".";

        private static readonly ProfilerMarker _PRF_CalculateNewAverageDelta =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateNewAverageDelta));

        private static readonly ProfilerMarker _PRF_CalculateNewAverage =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateNewAverage));

        private static readonly ProfilerMarker _PRF_AddSample =
            new ProfilerMarker(_PRF_PFX + nameof(AddSample));

        private static readonly ProfilerMarker _PRF_Clear = new ProfilerMarker(_PRF_PFX + nameof(Clear));

        #endregion
    }
}
