using System.Collections.Generic;

namespace Appalachia.Core.Context.Elements.Progress
{
    public sealed class AppaProgressCounter
    {
        public AppaProgressCounter()
        {
            _segmentSizes = new Stack<float>();
            _segmentPrefixes = new Stack<string>();

            _segmentSizes.Push(100f);
            _segmentPrefixes.Push(string.Empty);
        }

        private float _currentRaw;

        private float _currentSegmented;
        private Stack<float> _segmentSizes;
        private Stack<string> _segmentPrefixes;

        public float Current => _currentSegmented;

        public void EndSegment()
        {
            _segmentSizes.Pop();
            _segmentPrefixes.Pop();
        }

        public AppaProgress Get(string message, float bump)
        {
            return (message, bump);
        }

        public AppaProgress Get(string message, ref float progress, float bump)
        {
            progress += bump;

            return (message, progress);
        }

        public AppaProgress GetIncrement(string message, ref float p, float sum, int steps)
        {
            Increment(ref p, sum, steps);
            return Get(message, p);
        }

        public AppaProgress GetIncrement<T>(
            string message,
            ref float p,
            float sum,
            IReadOnlyList<T> collection)
        {
            Increment(ref p, sum, collection);
            return Get(message, p);
        }

        public AppaProgress GetIncrement<T>(
            string message,
            ref float p,
            float sum,
            T[] collection)
        {
            Increment(ref p, sum, collection);
            return Get(message, p);
        }

        public void Increment(ref float p, float sum, int steps)
        {
            p += sum * (1f / steps);
        }

        public void Increment<T>(ref float p, float sum, IReadOnlyList<T> collection)
        {
            Increment(ref p, sum, collection.Count);
        }

        public void Increment<T>(ref float p, float sum, T[] collection)
        {
            Increment(ref p, sum, collection.Length);
        }

        public AppaProgressSegment NewSegment(float? segmentSize = null, string segmentPrefix = null)
        {
            if (!segmentSize.HasValue)
            {
                _segmentSizes.Push(_segmentSizes.Peek());
            }
            else
            {
                _segmentSizes.Push(segmentSize.Value);
            }

            if (segmentPrefix == null)
            {
                _segmentPrefixes.Push(_segmentPrefixes.Peek());
            }
            else
            {
                _segmentPrefixes.Push($"{_segmentPrefixes.Peek()} | {segmentPrefix}");
            }

            return new AppaProgressSegment(this);
        }

        internal void Interpret(ref AppaProgress p)
        {
            _currentRaw = p.progress;

            p.progress = p.progress * _segmentSizes.Peek();
            p.message = $"{_segmentPrefixes.Peek()} | {p.message}";

            _currentSegmented = p.progress;
        }
    }
}
