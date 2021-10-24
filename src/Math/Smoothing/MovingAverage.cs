using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public abstract class MovingAverage<T>
    {
        [SerializeField]
        [ShowInInspector]
        [PropertyRange(1, 64)]
        [HorizontalGroup("BBB")]
        [SmartLabel]
        public int windowSize = 16;

        private readonly Queue<T> _samples = new();

        [SerializeField]
        [ShowInInspector]
        [HorizontalGroup("BBB")]
        [SmartLabel]
        private T _average;

        private T _sampleAccumulator;

        public T Average
        {
            get => _average;
            private set => _average = value;
        }

        protected abstract T Add(T a, T b);

        protected abstract T Divide(T a, int divisor);

        protected abstract T Subtract(T a, T b);

        /// <summary>
        ///     Computes a new windowed average each time a new sample arrives
        /// </summary>
        /// <param name="newSample"></param>
        public void ComputeAverage(T newSample)
        {
            _sampleAccumulator = Add(_sampleAccumulator, newSample);

            _samples.Enqueue(newSample);

            if (_samples.Count > windowSize)
            {
                _sampleAccumulator = Subtract(_sampleAccumulator, _samples.Dequeue());
            }

            Average = Divide(_sampleAccumulator, _samples.Count);
        }
    }
}
