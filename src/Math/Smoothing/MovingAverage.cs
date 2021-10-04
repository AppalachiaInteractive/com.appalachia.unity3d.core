using System;
using System.Collections.Generic;
using Appalachia.Editing.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Math.Smoothing
{
    [Serializable]
    public abstract class MovingAverage<T>  
    {
        private readonly Queue<T> _samples = new Queue<T>();
        
        private T _sampleAccumulator;
        
        [SerializeField]
        [ShowInInspector]
        [PropertyRange(1, 64)]
        [HorizontalGroup("BBB"), SmartLabel]
        public int windowSize = 16;
        
        [SerializeField]
        [ShowInInspector]
        [HorizontalGroup("BBB"), SmartLabel]
        private T _average;

        public T Average
        {
            get => _average;
            private set => _average = value;
        }

        /// <summary>
        /// Computes a new windowed average each time a new sample arrives
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

        protected abstract T Add(T a, T b);

        protected abstract T Subtract(T a, T b);

        protected abstract T Divide(T a, int divisor);
    }
}
