/*
#region

using System;
using System.Runtime.CompilerServices;
using Appalachia.Core.Behaviours;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Caching
{
    public struct CacheCreator<T>
    {
        public CacheCreator(Func<T> retrieve)
        {
            this.retrieve = retrieve;
        }

        private readonly Func<T> retrieve;

        public Cached<T> ForMinutes(double minutes)
        {
            return Cached<T>.InvalidateByTime(retrieve, 1000 * 60 * minutes);
        }

        public Cached<T> ForSeconds(double seconds)
        {
            return Cached<T>.InvalidateByTime(retrieve, 1000 * seconds);
        }

        public Cached<T> ForMilliseconds(double milliseconds)
        {
            return Cached<T>.InvalidateByTime(retrieve, milliseconds);
        }

        public Cached<T> For(TimeSpan invalidationTime)
        {
            return Cached<T>.InvalidateByTime(retrieve, invalidationTime);
        }

        public Cached<T> ForFrames(int frameCount)
        {
            return Cached<T>.InvalidateByFrames(retrieve, frameCount);
        }

        public Cached<T> LimitUsages(int accessCountLimit)
        {
            return Cached<T>.InvalidateByAccessCount(retrieve, accessCountLimit);
        }

        public Cached<T> While(Predicate<T> condition)
        {
            return Cached<T>.InvalidateConditionally(retrieve, condition);
        }

        public Cached<T> UntilChange<TV>(Func<TV> externalValueRetriever)
        {
            return Cached<T>.InvalidateByExternalValueComparison(retrieve, externalValueRetriever);
        }

        public Cached<T> While<TV>(Func<TV> externalValueRetriever, Func<TV, TV, bool> comparison)
        {
            return Cached<T>.InvalidateByExternalValueComparison(retrieve, externalValueRetriever, comparison);
        }
    }

    public abstract class Cached : InternalBase<Cached>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CacheCreator<T> Cache<T>(Func<T> retrieve)
        {
            return new CacheCreator<T>(retrieve);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> Cache<T>(T initialValue)
        {
            return Cached<T>.NeverInvalidate(initialValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> NeverCache<T>(Func<T> retrieveCache)
        {
            return Cached<T>.NeverCache(retrieveCache);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByTime<T>(Func<T> retrieveCache, double milliseconds)
        {
            return Cached<T>.InvalidateByTime(retrieveCache, milliseconds);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByTime<T>(Func<T> retrieveCache, TimeSpan invalidationTime)
        {
            return Cached<T>.InvalidateByTime(retrieveCache, invalidationTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByFrames<T>(Func<T> retrieveCache, int frameCount)
        {
            return Cached<T>.InvalidateByFrames(retrieveCache, frameCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByAccessCount<T>(Func<T> retrieveCache, int accessLimit)
        {
            return Cached<T>.InvalidateByAccessCount(retrieveCache, accessLimit);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateWhen<T>(Func<T> retrieveCache, Predicate<T> condition)
        {
            return Cached<T>.InvalidateConditionally(retrieveCache, condition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateWhen<T, TV>(Func<T> retrieveCache, Func<TV> externalValueRetriever)
        {
            return Cached<T>.InvalidateByExternalValueComparison(retrieveCache, externalValueRetriever);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateWhen<T, TV>(Func<T> retrieveCache, Func<TV> externalValueRetriever, Func<TV, TV, bool> comparison)
        {
            return Cached<T>.InvalidateByExternalValueComparison(retrieveCache, externalValueRetriever, comparison);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> IInvalidateByFramesWhen<T>(Func<T> retrieveCache, int frameCount, Predicate<T> condition)
        {
            return Cached<T>.InvalidateFramesAndConditionally(retrieveCache, frameCount, condition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByFramesWhen<T, TV>(Func<T> retrieveCache, int frameCount, Func<TV> externalValueRetriever)
        {
            return Cached<T>.InvalidateFramesAndByExternalValueComparison(retrieveCache, frameCount, externalValueRetriever);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByFramesWhen<T, TV>(
            Func<T> retrieveCache,
            int frameCount,
            Func<TV> externalValueRetriever,
            Func<TV, TV, bool> comparison)
        {
            return Cached<T>.InvalidateFramesAndByExternalValueComparison(retrieveCache, frameCount, externalValueRetriever, comparison);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByTime<T>(T initial, Func<T> retrieveCache, double milliseconds)
        {
            return Cached<T>.InvalidateByTime(initial, retrieveCache, milliseconds);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByTime<T>(T initial, Func<T> retrieveCache, TimeSpan invalidationTime)
        {
            return Cached<T>.InvalidateByTime(initial, retrieveCache, invalidationTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByFrames<T>(T initial, Func<T> retrieveCache, int frameCount)
        {
            return Cached<T>.InvalidateByFrames(initial, retrieveCache, frameCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByAccessCount<T>(T initial, Func<T> retrieveCache, int accessLimit)
        {
            return Cached<T>.InvalidateByAccessCount(initial, retrieveCache, accessLimit);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateWhen<T>(T initial, Func<T> retrieveCache, Predicate<T> condition)
        {
            return Cached<T>.InvalidateConditionally(initial, retrieveCache, condition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateWhen<T, TV>(T initial, Func<T> retrieveCache, Func<TV> externalValueRetriever)
        {
            return Cached<T>.InvalidateByExternalValueComparison(initial, retrieveCache, externalValueRetriever);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateWhen<T, TV>(
            T initial,
            Func<T> retrieveCache,
            Func<TV> externalValueRetriever,
            Func<TV, TV, bool> comparison)
        {
            return Cached<T>.InvalidateByExternalValueComparison(initial, retrieveCache, externalValueRetriever, comparison);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> IInvalidateByFramesWhen<T>(T initial, Func<T> retrieveCache, int frameCount, Predicate<T> condition)
        {
            return Cached<T>.InvalidateFramesAndConditionally(initial, retrieveCache, frameCount, condition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByFramesWhen<T, TV>(T initial, Func<T> retrieveCache, int frameCount, Func<TV> externalValueRetriever)
        {
            return Cached<T>.InvalidateFramesAndByExternalValueComparison(initial, retrieveCache, frameCount, externalValueRetriever);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Cached<T> InvalidateByFramesWhen<T, TV>(
            T initial,
            Func<T> retrieveCache,
            int frameCount,
            Func<TV> externalValueRetriever,
            Func<TV, TV, bool> comparison)
        {
            return Cached<T>.InvalidateFramesAndByExternalValueComparison(initial, retrieveCache, frameCount, externalValueRetriever, comparison);
        }
    }

    public sealed class Cached<T> : Cached
    {
        
#region ProfilerMarkers

        private const string _PRF_PFX = nameof(Cached<T>) + ".";
        private static readonly ProfilerMarker _PRF_Get = new ProfilerMarker(_PRF_PFX + nameof(Get));
        private static readonly ProfilerMarker _PRF_GetBase = new ProfilerMarker(_PRF_PFX + nameof(GetBase));
        private static readonly ProfilerMarker _PRF_Get_RetrievalFunction = new ProfilerMarker(_PRF_PFX + nameof(Get) + ".RetrievalFunction");
        private static readonly ProfilerMarker _PRF_Invalidate = new ProfilerMarker(_PRF_PFX + nameof(Invalidate));
        private static readonly ProfilerMarker _PRF_InvalidateByAccessCount = new ProfilerMarker(_PRF_PFX + nameof(InvalidateByAccessCount));
        private static readonly ProfilerMarker _PRF_InvalidateByExternalValueComparison = new ProfilerMarker(_PRF_PFX + nameof(InvalidateByExternalValueComparison));
        private static readonly ProfilerMarker _PRF_InvalidateByFrames = new ProfilerMarker(_PRF_PFX + nameof(InvalidateByFrames));
        private static readonly ProfilerMarker _PRF_InvalidateByTime = new ProfilerMarker(_PRF_PFX + nameof(InvalidateByTime));
        private static readonly ProfilerMarker _PRF_InvalidateConditionally = new ProfilerMarker(_PRF_PFX + nameof(InvalidateConditionally));
        private static readonly ProfilerMarker _PRF_InvalidateFramesAndByExternalValueComparison = new ProfilerMarker(_PRF_PFX + nameof(InvalidateFramesAndByExternalValueComparison));
        private static readonly ProfilerMarker _PRF_InvalidateFramesAndConditionally = new ProfilerMarker(_PRF_PFX + nameof(InvalidateFramesAndConditionally));
        private static readonly ProfilerMarker _PRF_NeverCache = new ProfilerMarker(_PRF_PFX + nameof(_PRF_NeverCache));
        private static readonly ProfilerMarker _PRF_NeverInvalidate = new ProfilerMarker(_PRF_PFX + nameof(NeverInvalidate));
        private static readonly ProfilerMarker _PRF_RecordCacheInvalidationData = new ProfilerMarker(_PRF_PFX + nameof(RecordCacheInvalidationData));
        private static readonly ProfilerMarker _PRF_RecordCacheValidData = new ProfilerMarker(_PRF_PFX + nameof(RecordCacheValidData));
        private static readonly ProfilerMarker _PRF_Set = new ProfilerMarker(_PRF_PFX + nameof(Set));
        private static readonly ProfilerMarker _PRF_ShouldInvalidate = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate));
        private static readonly ProfilerMarker _PRF_op_Implicit = new ProfilerMarker(_PRF_PFX + "op_Implicit");

#endregion
        
        [NonSerialized] private CacheInvalidationStrategy _invalidationStrategy;
        [NonSerialized] private double _thresholdMilliseconds;
        [NonSerialized] private DateTime _lastCacheInvalidationTime;

        [NonSerialized] private int _thresholdFrames;
        [NonSerialized] private int _lastCacheInvalidationFrames;

        [NonSerialized] private int _accessCountThreshold;
        [NonSerialized] private int _currentAccessCount;

        [NonSerialized] private Predicate<T> _invalidateCache;

        [NonSerialized] private Func<object> _externalValueRetriever;
        [NonSerialized] private Func<object, object, bool> _doesValueMatchExternal;
        [NonSerialized] private object _lastExternalValue;

        [NonSerialized] private Func<T> _retrieveCacheFunction;

        [NonSerialized] private T _cached;

        private Cached()
        {
        }

        private static readonly ProfilerMarker _PRF_ShouldInvalidate_Time = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate) + ".Time");
        private static readonly ProfilerMarker _PRF_ShouldInvalidate_Frames = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate) + ".Frames");
        private static readonly ProfilerMarker _PRF_ShouldInvalidate_Conditional = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate) + ".Conditional");
        private static readonly ProfilerMarker _PRF_ShouldInvalidate_AccessCount = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate) + ".AccessCount");
        private static readonly ProfilerMarker _PRF_ShouldInvalidate_ExternalValueComparison = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate) + ".ExternalValueComparison");
        private static readonly ProfilerMarker _PRF_ShouldInvalidate_FramesAndConditional = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate) + ".FramesAndConditional");
        private static readonly ProfilerMarker _PRF_ShouldInvalidate_FramesAndExternalValueComparison = new ProfilerMarker(_PRF_PFX + nameof(ShouldInvalidate) + ".FramesAndExternalValueComparison");
        
        private bool ShouldInvalidate()
        {
            using (_PRF_ShouldInvalidate.Auto())
            {
                switch (_invalidationStrategy)
                {
                    case CacheInvalidationStrategy.NeverInvalidate:
                        return false;
                    case CacheInvalidationStrategy.NeverCache:
                        return true;
                    case CacheInvalidationStrategy.Time:
                        using (_PRF_ShouldInvalidate_Time.Auto())
                        {
                            return (DateTime.UtcNow - _lastCacheInvalidationTime).TotalMilliseconds > _thresholdMilliseconds;
                        }
                    case CacheInvalidationStrategy.Frames:
                        using (_PRF_ShouldInvalidate_Frames.Auto())
                        {
                            return (CoreClock.Instance.FrameCount - _lastCacheInvalidationFrames) > _thresholdFrames;
                        }
                    case CacheInvalidationStrategy.Conditional:
                        using (_PRF_ShouldInvalidate_Conditional.Auto())
                        {
                            return _invalidateCache(_cached);
                        }
                    case CacheInvalidationStrategy.AccessCount:
                        using (_PRF_ShouldInvalidate_AccessCount.Auto())
                        {
                            return _currentAccessCount > _accessCountThreshold;
                        }
                    case CacheInvalidationStrategy.ExternalValueComparison:
                        using (_PRF_ShouldInvalidate_ExternalValueComparison.Auto())
                        {
                            return !_doesValueMatchExternal(_lastExternalValue, _externalValueRetriever());
                        }
                    case CacheInvalidationStrategy.FramesAndConditional:
                        using (_PRF_ShouldInvalidate_FramesAndConditional.Auto())
                        {
                            return ((CoreClock.Instance.FrameCount - _lastCacheInvalidationFrames) > _thresholdFrames) && _invalidateCache(_cached);
                        }
                    case CacheInvalidationStrategy.FramesAndExternalValueComparison:
                        using (_PRF_ShouldInvalidate_FramesAndExternalValueComparison.Auto())
                        {
                            return ((CoreClock.Instance.FrameCount - _lastCacheInvalidationFrames) > _thresholdFrames) &&
                                   !_doesValueMatchExternal(_lastExternalValue, _externalValueRetriever());
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void RecordCacheValidData()
        {
            using (_PRF_RecordCacheValidData.Auto())
            {
                switch (_invalidationStrategy)
                {
                    case CacheInvalidationStrategy.NeverInvalidate:
                        break;
                    case CacheInvalidationStrategy.NeverCache:
                        break;
                    case CacheInvalidationStrategy.Time:
                        break;
                    case CacheInvalidationStrategy.Frames:
                        break;
                    case CacheInvalidationStrategy.Conditional:
                        break;
                    case CacheInvalidationStrategy.AccessCount:
                        _currentAccessCount += 1;
                        break;
                    case CacheInvalidationStrategy.ExternalValueComparison:
                        break;
                    case CacheInvalidationStrategy.FramesAndConditional:
                        break;
                    case CacheInvalidationStrategy.FramesAndExternalValueComparison:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static readonly ProfilerMarker _PRF_RecordCacheInvalidationData_Time = new ProfilerMarker(_PRF_PFX + nameof(RecordCacheInvalidationData) + ".Time" );
        private static readonly ProfilerMarker _PRF_RecordCacheInvalidationData_Frames = new ProfilerMarker(_PRF_PFX + nameof(RecordCacheInvalidationData) + ".Frames");
        private static readonly ProfilerMarker _PRF_RecordCacheInvalidationData_ExternalValueComparison = new ProfilerMarker(_PRF_PFX + nameof(RecordCacheInvalidationData) + ".ExternalValueComparison");
        private static readonly ProfilerMarker _PRF_RecordCacheInvalidationData_FramesAndConditional = new ProfilerMarker(_PRF_PFX + nameof(RecordCacheInvalidationData) + ".FramesAndConditional");
        private static readonly ProfilerMarker _PRF_RecordCacheInvalidationData_FramesAndExternalValueComparison = new ProfilerMarker(_PRF_PFX + nameof(RecordCacheInvalidationData) + ".FramesAndExternalValueComparison");
        private void RecordCacheInvalidationData()
        {
            using (_PRF_RecordCacheInvalidationData.Auto())
            {
                switch (_invalidationStrategy)
                {
                    case CacheInvalidationStrategy.NeverInvalidate:
                        break;
                    case CacheInvalidationStrategy.NeverCache:
                        break;
                    case CacheInvalidationStrategy.Time:
                        using (_PRF_RecordCacheInvalidationData_Time.Auto())
                        {
                            _lastCacheInvalidationTime = DateTime.UtcNow;
                        }
                        break;
                    case CacheInvalidationStrategy.Frames:
                        using (_PRF_RecordCacheInvalidationData_Frames.Auto())
                        {
                            _lastCacheInvalidationFrames = CoreClock.Instance.FrameCount;
                        }
                        break;
                    case CacheInvalidationStrategy.Conditional:
                        break;
                    case CacheInvalidationStrategy.AccessCount:
                        _currentAccessCount = 0;
                        break;
                    case CacheInvalidationStrategy.ExternalValueComparison:
                        using (_PRF_RecordCacheInvalidationData_ExternalValueComparison.Auto())
                        {
                            _lastExternalValue = _externalValueRetriever();
                        }
                        break;
                    case CacheInvalidationStrategy.FramesAndConditional:
                        using (_PRF_RecordCacheInvalidationData_FramesAndConditional.Auto())
                        {
                            _lastCacheInvalidationFrames = CoreClock.Instance.FrameCount;
                        }
                        break;
                    case CacheInvalidationStrategy.FramesAndExternalValueComparison:
                        using (_PRF_RecordCacheInvalidationData_FramesAndExternalValueComparison.Auto())
                        {
                            _lastCacheInvalidationFrames = CoreClock.Instance.FrameCount;
                            _lastExternalValue = _externalValueRetriever();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Set(T newValue)
        {
            using (_PRF_Set.Auto())
            {
                _cached = newValue;
                RecordCacheInvalidationData();
            }
        }

        public T Get()
        {
            using (_PRF_Get.Auto())
            {
                if (ShouldInvalidate())
                {
                    using (_PRF_Get_RetrievalFunction.Auto())
                    {
                        _cached = _retrieveCacheFunction();
                    }

                    RecordCacheInvalidationData();
                }
                else
                {
                    RecordCacheValidData();
                }

                return _cached;
            }
        }

        public void Invalidate()
        {
            using (_PRF_Invalidate.Auto())
            {
                _cached = _retrieveCacheFunction();

                RecordCacheInvalidationData();
            }
        }

        private static Cached<T> GetBase(CacheInvalidationStrategy strategy, T initial)
        {
            using (_PRF_GetBase.Auto())
            {
                var instance = new Cached<T>
                {
                    _cached = initial,
                    _invalidationStrategy = strategy,
                    _thresholdMilliseconds = default,
                    _lastCacheInvalidationTime = default,
                    _thresholdFrames = default,
                    _lastCacheInvalidationFrames = default,
                    _accessCountThreshold = default,
                    _currentAccessCount = default,
                    _invalidateCache = default,
                    _externalValueRetriever = default,
                    _lastExternalValue = default,
                    _retrieveCacheFunction = default
                };

                return instance;
            }
        }

        private static Cached<T> GetBase(CacheInvalidationStrategy strategy)
        {
            using (_PRF_GetBase.Auto())
            {
                var instance = new Cached<T>
                {
                    _invalidationStrategy = strategy,
                    _thresholdMilliseconds = default,
                    _lastCacheInvalidationTime = default,
                    _thresholdFrames = default,
                    _lastCacheInvalidationFrames = default,
                    _accessCountThreshold = default,
                    _currentAccessCount = default,
                    _invalidateCache = default,
                    _externalValueRetriever = default,
                    _lastExternalValue = default,
                    _retrieveCacheFunction = default
                };

                return instance;
            }
        }

        public static Cached<T> NeverInvalidate(T initialValue)
        {
            using (_PRF_NeverInvalidate.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.NeverInvalidate);
                instance._cached = initialValue;

                return instance;
            }
        }

        public static Cached<T> NeverCache(Func<T> retrieveCache)
        {
            using (_PRF_NeverCache.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.NeverInvalidate);
                instance._retrieveCacheFunction = retrieveCache;

                return instance;
            }
        }

        public static Cached<T> InvalidateByTime(Func<T> retrieveCache, double milliseconds)
        {
            using (_PRF_InvalidateByTime.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Time);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdMilliseconds = milliseconds;

                return instance;
            }
        }

        public static Cached<T> InvalidateByTime(Func<T> retrieveCache, TimeSpan invalidationTime)
        {
            using (_PRF_InvalidateByTime.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Time);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdMilliseconds = invalidationTime.TotalMilliseconds;

                return instance;
            }
        }

        public static Cached<T> InvalidateByFrames(Func<T> retrieveCache, int frameCount)
        {
            using (_PRF_InvalidateByFrames.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Frames);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdFrames = frameCount;

                return instance;
            }
        }

        public static Cached<T> InvalidateByAccessCount(Func<T> retrieveCache, int accessLimit)
        {
            using (_PRF_InvalidateByAccessCount.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.AccessCount);
                instance._retrieveCacheFunction = retrieveCache;
                instance._accessCountThreshold = accessLimit;

                return instance;
            }
        }

        public static Cached<T> InvalidateConditionally(Func<T> retrieveCache, Predicate<T> condition)
        {
            using (_PRF_InvalidateConditionally.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Conditional);
                instance._retrieveCacheFunction = retrieveCache;
                instance._invalidateCache = condition;

                return instance;
            }
        }

        
        public static Cached<T> InvalidateByExternalValueComparison<TV>(Func<T> retrieveCache, Func<TV> externalValueRetriever)
        {
            using (_PRF_InvalidateByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.ExternalValueComparison);
                instance._retrieveCacheFunction = retrieveCache;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => o?.Equals(o1) ?? false;

                return instance;
            }
        }

        public static Cached<T> InvalidateByExternalValueComparison<TV>(
            Func<T> retrieveCache,
            Func<TV> externalValueRetriever,
            Func<TV, TV, bool> comparison)
        {
            using (_PRF_InvalidateByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.ExternalValueComparison);
                instance._retrieveCacheFunction = retrieveCache;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => comparison((TV) o, (TV) o1);

                return instance;
            }
        }

        public static Cached<T> InvalidateFramesAndConditionally(Func<T> retrieveCache, int frameCount, Predicate<T> condition)
        {
            using (_PRF_InvalidateFramesAndConditionally.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.FramesAndConditional);
                instance._thresholdFrames = frameCount;
                instance._retrieveCacheFunction = retrieveCache;
                instance._invalidateCache = condition;

                return instance;
            }
        }

        public static Cached<T> InvalidateFramesAndByExternalValueComparison<TV>(
            Func<T> retrieveCache,
            int frameCount,
            Func<TV> externalValueRetriever)
        {
            using (_PRF_InvalidateFramesAndByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.FramesAndExternalValueComparison);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdFrames = frameCount;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => o?.Equals(o1) ?? false;

                return instance;
            }
        }

        public static Cached<T> InvalidateFramesAndByExternalValueComparison<TV>(
            Func<T> retrieveCache,
            int frameCount,
            Func<TV> externalValueRetriever,
            Func<TV, TV, bool> comparison)
        {
            using (_PRF_InvalidateFramesAndByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.FramesAndExternalValueComparison);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdFrames = frameCount;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => comparison((TV) o, (TV) o1);

                return instance;
            }
        }

        public static Cached<T> InvalidateByTime(T initial, Func<T> retrieveCache, double milliseconds)
        {
            using (_PRF_InvalidateByTime.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Time, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdMilliseconds = milliseconds;

                return instance;
            }
        }

        public static Cached<T> InvalidateByTime(T initial, Func<T> retrieveCache, TimeSpan invalidationTime)
        {
            using (_PRF_InvalidateByTime.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Time, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdMilliseconds = invalidationTime.TotalMilliseconds;

                return instance;
            }
        }

        public static Cached<T> InvalidateByFrames(T initial, Func<T> retrieveCache, int frameCount)
        {
            using (_PRF_InvalidateByFrames.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Frames, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdFrames = frameCount;

                return instance;
            }
        }

        public static Cached<T> InvalidateByAccessCount(T initial, Func<T> retrieveCache, int accessLimit)
        {
            using (_PRF_InvalidateByAccessCount.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.AccessCount, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._accessCountThreshold = accessLimit;

                return instance;
            }
        }

        public static Cached<T> InvalidateConditionally(T initial, Func<T> retrieveCache, Predicate<T> condition)
        {
            using (_PRF_InvalidateConditionally.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.Conditional, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._invalidateCache = condition;

                return instance;
            }
        }

        public static Cached<T> InvalidateByExternalValueComparison<TV>(T initial, Func<T> retrieveCache, Func<TV> externalValueRetriever)
        {
            using (_PRF_InvalidateByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.ExternalValueComparison, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => o?.Equals(o1) ?? false;

                return instance;
            }
        }

        public static Cached<T> InvalidateByExternalValueComparison<TV>(
            T initial,
            Func<T> retrieveCache,
            Func<TV> externalValueRetriever,
            Func<TV, TV, bool> comparison)
        {
            using (_PRF_InvalidateByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.ExternalValueComparison, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => comparison((TV) o, (TV) o1);

                return instance;
            }
        }

        public static Cached<T> InvalidateFramesAndConditionally(T initial, Func<T> retrieveCache, int frameCount, Predicate<T> condition)
        {
            using (_PRF_InvalidateFramesAndConditionally.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.FramesAndConditional, initial);
                instance._thresholdFrames = frameCount;
                instance._retrieveCacheFunction = retrieveCache;
                instance._invalidateCache = condition;

                return instance;
            }
        }

        public static Cached<T> InvalidateFramesAndByExternalValueComparison<TV>(
            T initial,
            Func<T> retrieveCache,
            int frameCount,
            Func<TV> externalValueRetriever)
        {
            using (_PRF_InvalidateFramesAndByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.FramesAndExternalValueComparison, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdFrames = frameCount;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => o?.Equals(o1) ?? false;

                return instance;
            }
        }

        public static Cached<T> InvalidateFramesAndByExternalValueComparison<TV>(
            T initial,
            Func<T> retrieveCache,
            int frameCount,
            Func<TV> externalValueRetriever,
            Func<TV, TV, bool> comparison)
        {
            using (_PRF_InvalidateFramesAndByExternalValueComparison.Auto())
            {
                var instance = GetBase(CacheInvalidationStrategy.FramesAndExternalValueComparison, initial);
                instance._retrieveCacheFunction = retrieveCache;
                instance._thresholdFrames = frameCount;
                instance._externalValueRetriever = () => externalValueRetriever();
                instance._doesValueMatchExternal = (o, o1) => comparison((TV) o, (TV) o1);

                return instance;
            }
        }

        [DebuggerStepThrough] public static implicit operator T(Cached<T> cached)
        {
            using (_PRF_op_Implicit.Auto())
            {
                return cached.Get();
            }
        }
    }
}
*/


