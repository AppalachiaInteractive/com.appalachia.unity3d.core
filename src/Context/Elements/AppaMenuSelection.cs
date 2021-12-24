using System;
using System.Collections.Generic;
using Appalachia.CI.Constants;
using UnityEngine;

namespace Appalachia.Core.Context.Elements
{
    [Serializable]
    public class AppaMenuSelection
    {
        [NonSerialized] private static AppaContext _context;

        private static AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(typeof(AppaMenuSelection));
                }

                return _context;
            }
        }
        public int currentIndex;
        public int currentVisibleIndex;
        public int lastVisibleCount;
        public int length;
        public int visibleCount;
        public Rect position;
        private List<bool> _visibility;

        private List<int> _indexToVisibleIndex;
        private List<int> _visibleIndexToIndex;

        public float scrollTime => currentVisibleIndex / (float) Mathf.Max(1, lastVisibleCount);

        public int GetIndex(int visibilityIndex)
        {
            EnsureCollectionSizes(
                ref _visibility,
                ref _indexToVisibleIndex,
                ref _visibleIndexToIndex,
                visibilityIndex
            );

            return _visibleIndexToIndex[visibilityIndex];
        }

        public int GetVisibilityIndex(int index)
        {
            EnsureCollectionSizes(ref _visibility, ref _indexToVisibleIndex, ref _visibleIndexToIndex, index);

            return _indexToVisibleIndex[index];
        }

        public bool IsSelected(int index)
        {
            EnsureCollectionSizes(ref _visibility, ref _indexToVisibleIndex, ref _visibleIndexToIndex, index);

            return currentIndex == index;
        }

        public bool IsVisible(int index)
        {
            EnsureCollectionSizes(ref _visibility, ref _indexToVisibleIndex, ref _visibleIndexToIndex, index);

            return _visibility[index];
        }

        public void RecordIndexInformation(int index, int visibleIndex, bool visible)
        {
            EnsureCollectionSizes(ref _visibility, ref _indexToVisibleIndex, ref _visibleIndexToIndex, index);

            _indexToVisibleIndex[index] = visibleIndex;

            if (visibleIndex >= 0)
            {
                _visibleIndexToIndex[visibleIndex] = index;
            }

            _visibility[index] = visible;

            if (visible)
            {
                visibleCount += 1;
            }
        }

        public void ResetVisibility()
        {
            /*EnsureCollectionSizes(ref _visibility, ref _indexToVisibleIndex, ref _visibleIndexToIndex, 1);

            for (var i = 0; i < _visibility.Count; i++)
            {
                _visibility[i] = false;
            }*/
            lastVisibleCount = visibleCount;
            visibleCount = 0;
        }

        public void SetLength(int length)
        {
            EnsureCollectionSizes(
                ref _visibility,
                ref _indexToVisibleIndex,
                ref _visibleIndexToIndex,
                length
            );

            this.length = length;
        }

        public void SetSelected(int index)
        {
            EnsureCollectionSizes(ref _visibility, ref _indexToVisibleIndex, ref _visibleIndexToIndex, index);

            var visibleIndex = GetVisibilityIndex(index);

            currentIndex = index;
            currentVisibleIndex = visibleIndex;
        }

        private static void EnsureCollectionSize<T>(ref List<T> collection, int requiredSize)
        {
            if (collection == null)
            {
                collection = new List<T>();
            }

            if (collection.Count < requiredSize)
            {
                var differenceSize = requiredSize - collection.Count;

                var newArray = new T[differenceSize];

                collection.AddRange(newArray);
            }
        }

        private static void EnsureCollectionSizes(
            ref List<bool> visibility,
            ref List<int> indexToVisibleIndex,
            ref List<int> visibleIndexToIndex,
            int index)
        {
            var requiredSize = index + 1;

            EnsureCollectionSize(ref visibility,          requiredSize);
            EnsureCollectionSize(ref indexToVisibleIndex, requiredSize);
            EnsureCollectionSize(ref visibleIndexToIndex, requiredSize);
        }
    }
}
