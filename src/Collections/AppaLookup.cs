#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Collections.Exceptions;
using Appalachia.Core.Collections.Interfaces;
using Appalachia.Core.Collections.NonSerialized;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Extensions.Debugging;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Collections
{
    [Serializable]
    [HideLabel, InlineProperty]
    public abstract class AppaLookup<TKey, TValue, TKeyList, TValueList> : AppaCollection,
        ISerializationCallbackReceiver,
        IAppaLookupState<TKey, TValue>,
        IAppaLookup<TKey, TValue, TValueList>,
        IAppaLookupSafeUpdates<TKey, TValue, TValueList>,
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>
        where TKeyList : AppaList<TKey>, new()
        where TValueList : AppaList<TValue>, new()
    {
        #region Constants and Static Readonly

        private const int _REMOVE_INDICES_CAPACITY = 32;

        private const string MARK_AS_MODIFIED_NOT_SET_MESSAGE =
            "Must set modification action for this collection!";

        #endregion

        protected AppaLookup()
        {
            using var scope = APPASERIALIZE.OnAfterDeserialize();
            INTERNAL_INITIALIZE(true);
        }

        protected AppaLookup(int capacity)
        {
            using var scope = APPASERIALIZE.OnAfterDeserialize();
            initializerCount = capacity;
            INTERNAL_INITIALIZE(true);
        }

        #region Fields and Autoproperties

        [SerializeField] protected int initializerCount = 64;

        protected virtual bool AlwaysRefreshDisplayData { get; } = false;
        protected virtual bool NoTracking { get; } = false;

        protected virtual bool ReplacesKeysAtStart { get; } = false;

        protected virtual bool ShouldDisplayTitle { get; } = false;

        [NonSerialized] private Action _markAsModifiedAction;

        [NonSerialized] private AppaList<int> _tempRemovedIndices;

        [NonSerialized] private bool _hasInitializedPreviously;

        [NonSerialized] private bool _initializedSuccessfully;

        [NonSerialized] private bool _isValueUnityObjectChecked;

        [NonSerialized] private bool isValueUnityObject;

        [NonSerialized] private Dictionary<TKey, int> _indices;

        [NonSerialized] private Dictionary<TKey, TValue> _lookup;

        private NonSerializedList<KVPDisplayWrapper> _displayWrappers;

        [SerializeField, HideInInspector]
        private Object _object;

        [SerializeField] private TKeyList keys;

        [SerializeField] private TValueList values;

        #endregion

        private static bool _allowRecreation
        {
            get
            {
                return
#if UNITY_EDITOR
                    IndexedCollectionsGlobals.CanRecreateBrokenCollections
#else
                false
#endif
                    ;
            }
        }

        private static bool _showDebug
        {
            get
            {
                return
#if UNITY_EDITOR
                    IndexedCollectionsGlobals.ShouldShowDebuggingFields
#else
                    false
#endif
                    ;
            }
        }

        protected virtual bool IsSerialized => true;

        public TKeyList keysAt
        {
            get => keys;
            set => keys = value;
        }

        protected Action markAsModifiedAction
        {
            get
            {
                if (!IsSerialized)
                {
                    return null;
                }

                if (APPASERIALIZE.InSerializationWindow)
                {
                    return null;
                }

                if ((_markAsModifiedAction == null) && (_object != null))
                {
                    _markAsModifiedAction = _object.MarkAsModified;
                }

                if (_markAsModifiedAction == null)
                {
                    Context.Log.Error(MARK_AS_MODIFIED_NOT_SET_MESSAGE, _object);
                    APPADEBUG.BREAKPOINT(
                        () => MARK_AS_MODIFIED_NOT_SET_MESSAGE,
                        _object,
                        () => _markAsModifiedAction == null
                    );
                }

                return _markAsModifiedAction;
            }
        }

        public bool All(Predicate<TValue> check)
        {
            using (_PRF_All.Auto())
            {
                for (var i = 0; i < values.Count; i++)
                {
                    if (!check(values[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool Any(Predicate<TValue> check)
        {
            using (_PRF_Any.Auto())
            {
                for (var i = 0; i < values.Count; i++)
                {
                    if (check(values[i]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public int CountKeys_NoAlloc(Predicate<TKey> pred)
        {
            return keys.Count_NoAlloc(pred);
        }

        public int CountValues_NoAlloc(Predicate<TValue> pred)
        {
            return values.Count_NoAlloc(pred);
        }

        public TValue First_NoAlloc(Predicate<TValue> check)
        {
            using (_PRF_First.Auto())
            {
                for (var i = 0; i < values.Count; i++)
                {
                    if (check(values[i]))
                    {
                        return values[i];
                    }
                }

                throw new InvalidOperationException("Not found.");
            }
        }

        public TValue FirstOrDefault_NoAlloc(Predicate<TValue> check)
        {
            using (_PRF_FirstOrDefault.Auto())
            {
                for (var i = 0; i < values.Count; i++)
                {
                    if (check(values[i]))
                    {
                        return values[i];
                    }
                }

                return default;
            }
        }

        public TValue FirstWithPreference_NoAlloc(Predicate<TValue> preferred, out bool foundAny)
        {
            using (_PRF_FirstWithPreference_NoAlloc.Auto())
            {
                TValue found = default;
                foundAny = false;

                for (var i = 0; i < values.Count; i++)
                {
                    var checking = values[i];

                    if (preferred(checking))
                    {
                        foundAny = true;
                        return checking;
                    }

                    if (i == 0)
                    {
                        foundAny = true;
                        found = checking;
                    }
                }

                return found;
            }
        }

        public TValue FirstWithPreference_NoAlloc(
            Predicate<TValue> preferred,
            Predicate<TValue> required,
            out bool foundAny)
        {
            using (_PRF_FirstWithPreference_NoAlloc.Auto())
            {
                TValue found = default;
                var foundRequired = false;
                foundAny = false;

                for (var i = 0; i < values.Count; i++)
                {
                    var checking = values[i];

                    if (preferred(checking))
                    {
                        foundAny = true;
                        return checking;
                    }

                    if (!foundRequired && required(checking))
                    {
                        foundAny = true;
                        found = checking;
                        foundRequired = true;
                    }
                }

                return found;
            }
        }

        public KVPDisplayWrapper GetKeyValuePair(int index)
        {
            using (_PRF_GetKeyValuePair.Auto())
            {
                if (_displayWrappers == null)
                {
                    _displayWrappers = new NonSerializedList<KVPDisplayWrapper>(Count);
                }

                _displayWrappers.Count = Count;

                var wrapper = _displayWrappers[index];

                INTERNAL_INITIALIZE();
                var key = keys[index];
                var value = values[index];

                if (wrapper == null)
                {
                    wrapper = new KVPDisplayWrapper(
                        ShouldDisplayTitle,
                        GetDisplayTitle(key, value),
                        GetDisplaySubtitle(key, value),
                        GetDisplayColor(key, value),
                        key,
                        value,
                        INTERNAL_UPDATE
                    );
                }
                else
                {
                    if (AlwaysRefreshDisplayData || !wrapper.Key.Equals(key))
                    {
                        wrapper.Update(
                            ShouldDisplayTitle,
                            GetDisplayTitle(key, value),
                            GetDisplaySubtitle(key, value),
                            GetDisplayColor(key, value),
                            key,
                            value,
                            INTERNAL_UPDATE
                        );
                    }
                }

                _displayWrappers[index] = wrapper;

                return wrapper;
            }
        }

        public TValue GetOrCreate(TKey key, Func<TValue> creator)
        {
            using (_PRF_GetOrCreate.Auto())
            {
                INTERNAL_INITIALIZE();

                if (INTERNAL_CONTAINS(key))
                {
                    return INTERNAL_GET(key);
                }

                var newValue = creator();
                INTERNAL_ADD(key, newValue);

                return newValue;
            }
        }

        public void Insert(int index, TKey key, TValue value)
        {
            using (_PRF_Insert.Auto())
            {
                INTERNAL_INITIALIZE();

                if (Equals(key, keys[index]))
                {
                    return;
                }

                keys.Insert(index, key);
                values.Insert(index, value);

                INTERNAL_REBUILD(false);
            }
        }

        public void RemoveNulls()
        {
            using (_PRF_RemoveNulls.Auto())
            {
                var rebuild = false;

                if (_tempRemovedIndices == null)
                {
                    _tempRemovedIndices = new NonSerializedList<int>(
                        _REMOVE_INDICES_CAPACITY,
                        noTracking: true
                    );
                }

                _tempRemovedIndices.ClearFast();

                if (values.RemoveNulls(_tempRemovedIndices))
                {
                    for (var i = 0; i < _tempRemovedIndices.Count; i++)
                    {
                        keys.RemoveAt(i);
                    }

                    rebuild = true;
                }

                if (rebuild)
                {
                    INTERNAL_REBUILD(false);
                }

                _tempRemovedIndices.ClearFast();
            }
        }

        /// <summary>Reverses the sequence of the elements in the entire index.</summary>
        public void Reverse()
        {
            using (_PRF_Reverse.Auto())
            {
                Reverse(0, Count);
            }
        }

        public void Reverse(int index, int length)
        {
            using (_PRF_Reverse.Auto())
            {
                keys.Reverse(index, length);
                values.Reverse(index, length);

                INTERNAL_REBUILD(false);
            }
        }

        public void SortByKey(IComparer<TKey> comparer)
        {
            using (_PRF_SortByKey.Auto())
            {
                SortByKey(0, Count, comparer);
            }
        }

        public void SortByKey(int index, int length, IComparer<TKey> comparer)
        {
            using (_PRF_SortByKey.Auto())
            {
                keys.Sort(values, index, length, comparer);

                INTERNAL_REBUILD(false);
            }
        }

        public void SortByKey(Comparison<TKey> comparison)
        {
            using (_PRF_SortByKey.Auto())
            {
                keys.Sort(values, comparison);

                INTERNAL_REBUILD(false);
            }
        }

        public void SortByValue(IComparer<TValue> comparer)
        {
            using (_PRF_SortByValue.Auto())
            {
                SortByValue(0, Count, comparer);
            }
        }

        public void SortByValue(int index, int length, IComparer<TValue> comparer)
        {
            using (_PRF_SortByValue.Auto())
            {
                values.Sort(keys, index, length, comparer);

                INTERNAL_REBUILD(false);
            }
        }

        public void SortByValue(Comparison<TValue> comparison)
        {
            using (_PRF_SortByValue.Auto())
            {
                values.Sort(keys, comparison);

                INTERNAL_REBUILD(false);
            }
        }

        public bool TryGetValueWithFallback(
            TKey key,
            out TValue value,
            Predicate<TValue> fallbackCheck,
            string logFallbackAttempt = null,
            string logFallbackFailure = null)
        {
            using (_PRF_TryGetValueWithFallback.Auto())
            {
                if (TryGetValue(key, out value))
                {
                    return true;
                }

                if (logFallbackAttempt != null)
                {
                    Context.Log.Warn(logFallbackAttempt);
                }

                value = FirstWithPreference_NoAlloc(fallbackCheck, out var foundFallback);

                if (foundFallback)
                {
                    return true;
                }

                if (logFallbackFailure != null)
                {
                    Context.Log.Warn(logFallbackFailure);
                }

                value = default;
                return false;
            }
        }

        // ReSharper disable once UnusedParameter.Global
        protected abstract Color GetDisplayColor(TKey key, TValue value);
        protected abstract string GetDisplaySubtitle(TKey key, TValue value);
        protected abstract string GetDisplayTitle(TKey key, TValue value);

        protected virtual void CheckReplacementAtStart(
            TKey key,
            TValue value,
            out TKey newKey,
            out TValue newValue)
        {
            newKey = key;
            newValue = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_ADD(TKey key, TValue value)
        {
            using (_PRF_INTERNAL_ADD.Auto())
            {
                _lookup.Add(key, value);

                _indices.Add(key, _indices.Count);
                values.Add(value);
                keys.Add(key);
                if (IsSerialized)
                {
                    markAsModifiedAction.Invoke();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_CHECK_FATAL()
        {
            using (_PRF_INTERNAL_CHECK_FATAL.Auto())
            {
                var valueCount = values.Count;
                var keyCount = keys.Count;

                var countMismatch = keyCount != valueCount;

                if (countMismatch)
                {
                    throw new IndexedKeyValueMismatchException(keyCount, valueCount);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_CLEAR()
        {
            using (_PRF_INTERNAL_CLEAR.Auto())
            {
                keys.Clear();
                values.Clear();
                _indices.Clear();
                _lookup.Clear();

                if (IsSerialized)
                {
                    markAsModifiedAction.Invoke();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool INTERNAL_CONTAINS(TKey key)
        {
            using (_PRF_INTERNAL_CONTAINS.Auto())
            {
                return _lookup.ContainsKey(key);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue INTERNAL_GET(TKey key)
        {
            using (_PRF_INTERNAL_GET.Auto())
            {
                if (INTERNAL_CONTAINS(key))
                {
                    return _lookup[key];
                }

                throw new KeyNotFoundException(
                    ZString.Format("Could not find key [{0}] in collection.", key)
                );
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TKey INTERNAL_GET_KEY_BY_INDEX(int index)
        {
            using (_PRF_INTERNAL_GET_KEY_BY_INDEX.Auto())
            {
                return keys[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue INTERNAL_GET_VALUE_BY_INDEX(int index)
        {
            using (_PRF_INTERNAL_GET_VALUE_BY_INDEX.Auto())
            {
                return values[index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_INITIALIZE(bool inConstructor = false)
        {
            using (_PRF_INTERNAL_INITIALIZE.Auto())
            {
                var needsFrameBasedCheck = ShouldExecuteFrameBasedCheck(out var frameCount);

                if (!needsFrameBasedCheck)
                {
                    return;
                }

                INTERNAL_INITIALIZE_EXECUTE(inConstructor);

                var keyCountsMatch = _lookup?.Count == keys?.Count;

                var hasLookupCountIssue = _hasInitializedPreviously && !keyCountsMatch;

                if (hasLookupCountIssue)
                {
                    APPADEBUG.BREAKPOINT(() => "Lookup key count is invalid!", _object);

                    if (_allowRecreation)
                    {
                        INTERNAL_RECREATE(inConstructor);
                        return;
                    }
                }

                var hasInitializationIssue = !_initializedSuccessfully && _hasInitializedPreviously;

                if (hasInitializationIssue)
                {
                    var message = "Did not initialize lookup successfully!";
                    APPADEBUG.BREAKPOINT(() => message, _object);

                    //throw new NotSupportedException(message);
                }

                if (!_initializedSuccessfully)
                {
                    var message = "Did not initialize lookup successfully!";
                    APPADEBUG.BREAKPOINT(() => message, _object);
                    throw new NotSupportedException(message);
                }

                LastFrameCheck = frameCount;
                _hasInitializedPreviously = true;
            }
        }

        private void INTERNAL_INITIALIZE_EXECUTE(bool inConstructor)
        {
            using (_PRF_INTERNAL_INITIALIZE_EXECUTE.Auto())
            {
                if (keys == null)
                {
                    keys = new TKeyList { Capacity = initializerCount };

                    if (!inConstructor)
                    {
                        if (APPASERIALIZE.InSerializationWindow)
                        {
                            if (_markAsModifiedAction != null)
                            {
                                if (IsSerialized)
                                {
                                    _markAsModifiedAction.Invoke();
                                }
                            }
                        }
                        else
                        {
                            if (IsSerialized)
                            {
                                markAsModifiedAction.Invoke();
                            }
                        }
                    }
                }

                if (values == null)
                {
                    values = new TValueList { Capacity = initializerCount };

                    if (!inConstructor)
                    {
                        if (APPASERIALIZE.InSerializationWindow)
                        {
                            if (_markAsModifiedAction != null)
                            {
                                if (IsSerialized)
                                {
                                    _markAsModifiedAction.Invoke();
                                }
                            }
                        }
                        else
                        {
                            if (IsSerialized)
                            {
                                markAsModifiedAction.Invoke();
                            }
                        }
                    }
                }

                if (_lookup == null)
                {
                    _lookup = new Dictionary<TKey, TValue>(initializerCount);

                    if (!inConstructor)
                    {
                        if (APPASERIALIZE.InSerializationWindow)
                        {
                            if (_markAsModifiedAction != null)
                            {
                                if (IsSerialized)
                                {
                                    _markAsModifiedAction.Invoke();
                                }
                            }
                        }
                        else
                        {
                            if (IsSerialized)
                            {
                                markAsModifiedAction.Invoke();
                            }
                        }
                    }
                }

                if (_indices == null)
                {
                    _indices = new Dictionary<TKey, int>(initializerCount);

                    if (!inConstructor)
                    {
                        if (APPASERIALIZE.InSerializationWindow)
                        {
                            if (_markAsModifiedAction != null)
                            {
                                if (IsSerialized)
                                {
                                    _markAsModifiedAction.Invoke();
                                }
                            }
                        }
                        else
                        {
                            if (IsSerialized)
                            {
                                markAsModifiedAction.Invoke();
                            }
                        }
                    }
                }

                if (!_isValueUnityObjectChecked)
                {
                    isValueUnityObject = typeof(Object).IsAssignableFrom(typeof(TValue));
                    _isValueUnityObjectChecked = true;
                }

                INTERNAL_CHECK_FATAL();

                if (INTERNAL_REQUIRES_REBUILD(inConstructor))
                {
                    INTERNAL_REBUILD(inConstructor);
                }

                INTERNAL_CHECK_FATAL();

                if (values.Count > initializerCount)
                {
                    initializerCount = values.Count;

                    if (!inConstructor)
                    {
                        if (APPASERIALIZE.InSerializationWindow)
                        {
                            if (_markAsModifiedAction != null)
                            {
                                if (IsSerialized)
                                {
                                    _markAsModifiedAction.Invoke();
                                }
                            }
                        }
                        else
                        {
                            if (IsSerialized)
                            {
                                markAsModifiedAction.Invoke();
                            }
                        }
                    }
                }

                _initializedSuccessfully = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_REBUILD(bool inConstructor)
        {
            using (_PRF_INTERNAL_REBUILD.Auto())
            {
                _lookup.Clear();
                _indices.Clear();

                var newIndex = 0;

                for (var i = 0; i < values.Count; i++)
                {
                    var newValue = values[i];
                    var key = keys[i];

                    if (key == null)
                    {
                        var message = ZString.Format(
                            "Null key cannot be added.  Index: {0} | Value: {1}",
                            i,
                            newValue
                        );

                        APPADEBUG.BREAKPOINT(() => message, _object);

                        throw new ArgumentException(message);
                    }

                    if (_indices.ContainsKey(key))
                    {
                        var previousIndex = _indices[key];

                        var message = ZString.Format(
                            "The key was already added.  Index: {0} | Existing Index: {1} | Key: {2}",
                            i,
                            previousIndex,
                            key
                        );

                        APPADEBUG.BREAKPOINT(() => message, _object);

                        //throw new ArgumentException(message);

                        var previousValue = values[previousIndex];

                        if ((newValue == null) && (previousValue != null))
                        {
                            continue;
                        }

                        if ((newValue != null) && (previousValue == null))
                        {
                            _indices[key] = newIndex;
                            _lookup[key] = newValue;
                        }
                    }
                    else
                    {
                        _indices.Add(key, newIndex);
                        _lookup.Add(key, newValue);

                        newIndex += 1;
                    }
                }

                if (!inConstructor)
                {
                    if (APPASERIALIZE.InSerializationWindow)
                    {
                        if (_markAsModifiedAction != null)
                        {
                            if (IsSerialized)
                            {
                                _markAsModifiedAction.Invoke();
                            }
                        }
                    }
                    else
                    {
                        if (IsSerialized)
                        {
                            markAsModifiedAction.Invoke();
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_RECREATE(bool inConstructor)
        {
            using (_PRF_INTERNAL_RECREATE.Auto())
            {
                _lookup ??= new Dictionary<TKey, TValue>();
                _indices ??= new Dictionary<TKey, int>();

                _lookup.Clear();
                _indices.Clear();

                var minSize = Mathf.Min(values.Count, keys.Count);

                var newKeysHash = new HashSet<TKey>();
                var newKeys = new List<TKey>();
                var newValues = new List<TValue>();

                for (var i = 0; i < minSize; i++)
                {
                    var value = values[i];
                    var key = keys[i];

                    if (key == null)
                    {
                        continue;
                    }

                    if (value == null)
                    {
                        continue;
                    }

                    if (newKeysHash.Contains(key))
                    {
                        continue;
                    }

                    newKeysHash.Add(key);

                    _lookup.Add(key, value);
                    _indices.Add(key, newKeys.Count);

                    newKeys.Add(key);
                    newValues.Add(value);
                }

                keys.Clear();
                values.Clear();

                keys.AddRange(newKeys);
                values.AddRange(newValues);

                keys.TrimExcess();
                values.TrimExcess();

                if (!inConstructor)
                {
                    if (APPASERIALIZE.InSerializationWindow)
                    {
                        if (_markAsModifiedAction != null)
                        {
                            if (IsSerialized)
                            {
                                _markAsModifiedAction.Invoke();
                            }
                        }
                    }
                    else
                    {
                        if (IsSerialized)
                        {
                            markAsModifiedAction.Invoke();
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TValue INTERNAL_REMOVE(int targetIndex)
        {
            using (_PRF_INTERNAL_REMOVE.Auto())
            {
                if ((targetIndex >= keys.Count) || (targetIndex < 0))
                {
                    throw new IndexOutOfRangeException(
                        ZString.Format(
                            "Index [{0}] is out of range for collection of length [{1}].",
                            targetIndex,
                            keys.Count
                        )
                    );
                }

                var deletingKey = keys[targetIndex];
                var deletingValue = values[targetIndex];

                _lookup.Remove(deletingKey);

                _indices.Remove(deletingKey);

                var lastIndex = keys.Count - 1;

                if (lastIndex != targetIndex)
                {
                    var lastKey = keys[lastIndex];
                    var lastValue = values[lastIndex];

                    keys[targetIndex] = lastKey;
                    values[targetIndex] = lastValue;

                    _indices[lastKey] = targetIndex;
                }

                keys.RemoveAt(lastIndex);
                values.RemoveAt(lastIndex);

                if (IsSerialized)
                {
                    markAsModifiedAction.Invoke();
                }

                return deletingValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool INTERNAL_REQUIRES_REBUILD(bool inConstructor)
        {
            using (_PRF_INTERNAL_REQUIRES_REBUILD.Auto())
            {
                var valueCount = values.Count;

                if (valueCount == 0)
                {
                    var modified = false;

                    if (keys.Count > 0)
                    {
                        keys.ClearFast();
                        modified = true;
                    }

                    if (_lookup.Count > 0)
                    {
                        _lookup.Clear();
                        modified = true;
                    }

                    if (_indices.Count > 0)
                    {
                        _indices.Clear();
                        modified = true;
                    }

                    if (modified && !inConstructor)
                    {
                        if (APPASERIALIZE.InSerializationWindow)
                        {
                            if (_markAsModifiedAction != null)
                            {
                                if (IsSerialized)
                                {
                                    _markAsModifiedAction.Invoke();
                                }
                            }
                        }
                        else
                        {
                            if (IsSerialized)
                            {
                                markAsModifiedAction.Invoke();
                            }
                        }
                    }

                    return false;
                }

                if (!APPASERIALIZE.InSerializationWindow)
                {
                    if (AppalachiaApplication.IsPlaying)
                    {
                        return false;
                    }
                }

                if (_tempRemovedIndices == null)
                {
                    _tempRemovedIndices = new NonSerializedList<int>(
                        _REMOVE_INDICES_CAPACITY,
                        noTracking: true
                    );
                }

                var mustRebuild = false;

                if (isValueUnityObject)
                {
                    _tempRemovedIndices.ClearFast();

                    if (values.RemoveNulls(_tempRemovedIndices))
                    {
                        for (var i = 0; i < _tempRemovedIndices.Count; i++)
                        {
                            keys.RemoveAt(i);
                        }

                        mustRebuild = true;
                    }
                }

                _tempRemovedIndices.ClearFast();

                if (keys.RemoveNulls(_tempRemovedIndices))
                {
                    for (var i = 0; i < _tempRemovedIndices.Count; i++)
                    {
                        values.RemoveAt(i);
                    }

                    mustRebuild = true;
                }

                if (mustRebuild)
                {
                    return true;
                }

                var lookupCount = _lookup.Count;
                var indexCount = _indices.Count;

                if ((lookupCount != valueCount) || (indexCount != valueCount))
                {
                    return true;
                }

                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool INTERNAL_TRY_GET(TKey key, out TValue value)
        {
            using (_PRF_INTERNAL_TRY_GET.Auto())
            {
                return _lookup.TryGetValue(key, out value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_UPDATE(TKey key, TValue value)
        {
            using (_PRF_INTERNAL_UPDATE.Auto())
            {
                var index = _indices[key];
                values[index] = value;

                _lookup[key] = value;

                if (IsSerialized)
                {
                    markAsModifiedAction.Invoke();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void INTERNAL_UPDATE_BY_INDEX(int index, TValue value)
        {
            using (_PRF_INTERNAL_UPDATE_BY_INDEX.Auto())
            {
                var key = keys[index];
                _lookup[key] = value;
                values[index] = value;
                if (IsSerialized)
                {
                    markAsModifiedAction.Invoke();
                }
            }
        }

        private bool ShouldExecuteFrameBasedCheck(out int frameCount)
        {
            frameCount = -1;

            if (!APPASERIALIZE.InSerializationWindow)
            {
                frameCount = Time.frameCount;
            }

            if (LastFrameCheck == frameCount)
            {
                return false;
            }

            return true;
        }

        #region IAppaLookup<TKey,TValue,TValueList> Members

        public void Add(TKey key, TValue value)
        {
            using (_PRF_Add.Auto())
            {
                INTERNAL_INITIALIZE();
                INTERNAL_ADD(key, value);
            }
        }

        public int Count
        {
            get
            {
                using (_PRF_Count.Auto())
                {
                    INTERNAL_INITIALIZE();

                    if (keys.Count != values.Count)
                    {
                        throw new KeyNotFoundException(
                            ZString.Format("Key Count: [{0}] Value Count: [{1}]", keys.Count, values.Count)
                        );
                    }

                    return values.Count;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                using (_PRF_Indexer.Auto())
                {
                    INTERNAL_INITIALIZE();
                    return INTERNAL_GET(key);
                }
            }
            set
            {
                using (_PRF_Indexer.Auto())
                {
                    INTERNAL_INITIALIZE();
                    INTERNAL_UPDATE(key, value);
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            using (_PRF_ContainsKey.Auto())
            {
                INTERNAL_INITIALIZE();
                return INTERNAL_CONTAINS(key);
            }
        }

        public TValueList at
        {
            get => values;
            set => values = value;
        }

        public void AddIfKeyNotPresent(TKey key, Func<TValue> value)
        {
            using (_PRF_AddIfKeyNotPresent.Auto())
            {
                INTERNAL_INITIALIZE();

                if (!INTERNAL_CONTAINS(key))
                {
                    INTERNAL_ADD(key, value());
                }
            }
        }

        public void AddIfKeyNotPresent(TKey key, TValue value)
        {
            using (_PRF_AddIfKeyNotPresent.Auto())
            {
                INTERNAL_INITIALIZE();

                if (!INTERNAL_CONTAINS(key))
                {
                    INTERNAL_ADD(key, value);
                }
            }
        }

        public void AddOrUpdateIf(TKey key, Func<TValue> valueRetriever, Predicate<TValue> updateIf)
        {
            using (_PRF_AddOrUpdateIf.Auto())
            {
                INTERNAL_INITIALIZE();

                if (!INTERNAL_CONTAINS(key))
                {
                    INTERNAL_ADD(key, valueRetriever());
                    return;
                }

                var v = _lookup[key];

                if (updateIf(v))
                {
                    INTERNAL_UPDATE(key, valueRetriever());
                }
            }
        }

        public void AddOrUpdateIf(TKey key, TValue value, Predicate<TValue> updateIf)
        {
            using (_PRF_AddOrUpdateIf.Auto())
            {
                INTERNAL_INITIALIZE();

                if (!INTERNAL_CONTAINS(key))
                {
                    INTERNAL_ADD(key, value);
                    return;
                }

                var v = _lookup[key];

                if (updateIf(v))
                {
                    INTERNAL_UPDATE(key, value);
                }
            }
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            using (_PRF_AddOrUpdate.Auto())
            {
                INTERNAL_INITIALIZE();

                if (!INTERNAL_CONTAINS(key))
                {
                    INTERNAL_ADD(key, value);
                }
                else
                {
                    INTERNAL_UPDATE(key, value);
                }
            }
        }

        public void AddOrUpdate(TKey key, Func<TValue> add, Func<TValue> update)
        {
            using (_PRF_AddOrUpdate.Auto())
            {
                INTERNAL_INITIALIZE();

                if (!INTERNAL_CONTAINS(key))
                {
                    INTERNAL_ADD(key, add());
                }
                else
                {
                    INTERNAL_UPDATE(key, update());
                }
            }
        }

        public void AddOrUpdate(KeyValuePair<TKey, TValue> pair)
        {
            using (_PRF_AddOrUpdate.Auto())
            {
                AddOrUpdate(pair.Key, pair.Value);
            }
        }

        public TValue RemoveByKey(TKey key)
        {
            using (_PRF_RemoveByKey.Auto())
            {
                INTERNAL_INITIALIZE();

                if (!INTERNAL_CONTAINS(key))
                {
                    return default;
                }

                var targetIndex = _indices[key];

                return INTERNAL_REMOVE(targetIndex);
            }
        }

        public TValue RemoveAt(int targetIndex)
        {
            using (_PRF_RemoveAt.Auto())
            {
                INTERNAL_INITIALIZE();

                return INTERNAL_REMOVE(targetIndex);
            }
        }

        public TValue Remove(KeyValuePair<TKey, TValue> pair)
        {
            using (_PRF_Remove.Auto())
            {
                return RemoveByKey(pair.Key);
            }
        }

        public void AddOrUpdateRange(IEnumerable<TValue> vals, Func<TValue, TKey> selector)
        {
            using (_PRF_AddOrUpdateRange.Auto())
            {
                INTERNAL_INITIALIZE();

                foreach (var val in vals)
                {
                    var key = selector(val);

                    if (!INTERNAL_CONTAINS(key))
                    {
                        INTERNAL_ADD(key, val);
                    }
                    else
                    {
                        INTERNAL_UPDATE(key, val);
                    }
                }
            }
        }

        public void Clear()
        {
            using (_PRF_Clear.Auto())
            {
                INTERNAL_INITIALIZE();
                INTERNAL_CLEAR();
            }
        }

        public TKey GetKeyByIndex(int i)
        {
            using (_PRF_GetKeyByIndex.Auto())
            {
                INTERNAL_INITIALIZE();

                return INTERNAL_GET_KEY_BY_INDEX(i);
            }
        }

        public TValue GetByIndex(int i)
        {
            using (_PRF_GetByIndex.Auto())
            {
                INTERNAL_INITIALIZE();

                return INTERNAL_GET_VALUE_BY_INDEX(i);
            }
        }

        public TValue Get(TKey key)
        {
            using (_PRF_Get.Auto())
            {
                INTERNAL_INITIALIZE();

                if (INTERNAL_CONTAINS(key))
                {
                    return INTERNAL_GET(key);
                }

                throw new KeyNotFoundException(
                    ZString.Format("Could not find key [{0}] in collection.", key)
                );
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            using (_PRF_TryGet.Auto())
            {
                INTERNAL_INITIALIZE();
                return INTERNAL_TRY_GET(key, out value);
            }
        }

        public void IfPresent(TKey key, Action present, Action notPresent)
        {
            using (_PRF_IfPresent.Auto())
            {
                INTERNAL_INITIALIZE();

                if (INTERNAL_CONTAINS(key))
                {
                    present();
                }
                else
                {
                    notPresent();
                }
            }
        }

        public int SumCounts(Func<TValue, int> counter)
        {
            using (_PRF_SumCounts.Auto())
            {
                INTERNAL_INITIALIZE();

                var sum = 0;

                var count = Count;

                for (var i = 0; i < count; i++)
                {
                    sum += counter(INTERNAL_GET_VALUE_BY_INDEX(i));
                }

                return sum;
            }
        }

        #endregion

        #region IAppaLookupState<TKey,TValue> Members

        public void SetObjectOwnership(Object owner)
        {
            _object = owner;
            _markAsModifiedAction = owner.MarkAsModified;
        }

        [field: NonSerialized] public int LastFrameCheck { get; private set; }

        public int InitializerCount
        {
            get => initializerCount;
            set => initializerCount = value;
        }

        public void SetObjectOwnership(Object owner, Action markAsModifiedAction)
        {
            _markAsModifiedAction = markAsModifiedAction;
            _object = owner;
        }

        public IReadOnlyList<TKey> Keys => keys;

        IReadOnlyList<TValue> IIndexedCollectionState<TValue>.Values => values;

        public IReadOnlyDictionary<TKey, TValue> Lookup => _lookup;

        public IReadOnlyDictionary<TKey, int> Indices => _indices;

        #endregion

        #region IDictionary<TKey,TValue> Members

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            using (_PRF_Remove.Auto())
            {
                INTERNAL_INITIALIZE();

                var x = RemoveByKey(item.Key);

                return x != null;
            }
        }

        public bool IsReadOnly => false;

        public bool Remove(TKey key)
        {
            using (_PRF_Remove.Auto())
            {
                INTERNAL_INITIALIZE();

                if (INTERNAL_CONTAINS(key))
                {
                    var targetIndex = _indices[key];

                    INTERNAL_REMOVE(targetIndex);
                    return true;
                }

                return false;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (_PRF_CopyTo.Auto())
            {
                INTERNAL_INITIALIZE();

                for (var i = arrayIndex; i < array.Length; i++)
                {
                    array[i] = new KeyValuePair<TKey, TValue>(keys[i - arrayIndex], values[i - arrayIndex]);
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            using (_PRF_Add.Auto())
            {
                INTERNAL_INITIALIZE();
                INTERNAL_ADD(item.Key, item.Value);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            using (_PRF_Contains.Auto())
            {
                INTERNAL_INITIALIZE();
                return INTERNAL_CONTAINS(item.Key) && INTERNAL_GET(item.Key).Equals(item.Value);
            }
        }

        public ICollection<TValue> Values => values;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => keys;

        public bool TryGetValue(TKey key, out TValue value)
        {
            using (_PRF_TryGetValue.Auto())
            {
                INTERNAL_INITIALIZE();

                return INTERNAL_TRY_GET(key, out value);
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            INTERNAL_INITIALIZE();
            return _lookup.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            INTERNAL_INITIALIZE();
            return values.GetEnumerator();
        }

        #endregion

        #region IReadOnlyDictionary<TKey,TValue> Members

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                INTERNAL_INITIALIZE();
                return keys;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                INTERNAL_INITIALIZE();
                return values;
            }
        }

        #endregion

        #region ISerializationCallbackReceiver Members

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            using (APPASERIALIZE.OnBeforeSerialize())
            using (_PRF_OnBeforeSerialize.Auto())
            {
                keys.TrimExcess();
                values.TrimExcess();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            using (APPASERIALIZE.OnAfterDeserialize())
            using (_PRF_OnAfterDeserialize.Auto())
            {
                if (ReplacesKeysAtStart)
                {
                    for (var i = 0; i < keys.Count; i++)
                    {
                        CheckReplacementAtStart(keys[i], values[i], out var newKey, out var newValue);
                        keys[i] = newKey;
                        values[i] = newValue;
                    }
                }

                INTERNAL_INITIALIZE();
            }
        }

        #endregion

        #region Nested type: KVPDisplayWrapper

        [HideReferenceObjectPicker]
        public class KVPDisplayWrapper : IEquatable<KVPDisplayWrapper>
        {
            #region Constants and Static Readonly

            private const string _colorPointer = nameof(_color);
            private const string _disableTitlePointer = nameof(_disableTitle);
            private const string _subtitlePointer = "$" + nameof(_subtitle);

            private const string _titlePointer = "$" + nameof(_title);

            #endregion

            public KVPDisplayWrapper(
                bool showTitle,
                string title,
                string subtitle,
                Color color,
                TKey key,
                TValue value,
                Action<TKey, TValue> updateAction)
            {
                using (_PRF_KVPDisplayWrapper.Auto())
                {
                    _disableTitle = !showTitle;
                    _title = title;
                    _subtitle = subtitle;
                    _color = color;
                    _pair = new KeyValuePair<TKey, TValue>(key, value);
                    _updateAction = updateAction;
                }
            }

            #region Fields and Autoproperties

            private Action<TKey, TValue> _updateAction;
            private bool _disableTitle;
            private Color _color;
            private KeyValuePair<TKey, TValue> _pair;
            private string _subtitle;
            private string _title;

            #endregion

            //[ShowInInspector]
            [ReadOnly] public TKey Key => _pair.Key;

            [ShowInInspector]
            [InlineProperty]
            [HideLabel]
            [LabelWidth(0)]
            [SmartTitle(
                _titlePointer,
                _subtitlePointer,
                _titlePointer,
                _subtitlePointer,
                hideIfMemberName: _disableTitlePointer,
                titleColor: _colorPointer
            )]
            public TValue Value
            {
                get => _pair.Value;
                set
                {
                    using (_PRF_Value.Auto())
                    {
                        _updateAction?.Invoke(Key, value);
                        _pair = new KeyValuePair<TKey, TValue>(Key, value);
                    }
                }
            }

            public void Update(
                bool showTitle,
                string title,
                string subtitle,
                Color color,
                TKey key,
                TValue value,
                Action<TKey, TValue> updateAction)
            {
                using (_PRF_Update.Auto())
                {
                    _disableTitle = !showTitle;
                    _title = title;
                    _subtitle = subtitle;
                    _color = color;
                    _pair = new KeyValuePair<TKey, TValue>(key, value);
                    _updateAction = updateAction;
                }
            }

            #region Profiling

            private const string _PRF_PFX = nameof(KVPDisplayWrapper) + ".";

            private static readonly ProfilerMarker _PRF_KVPDisplayWrapper =
                new(_PRF_PFX + nameof(KVPDisplayWrapper));

            private static readonly ProfilerMarker _PRF_Value = new(_PRF_PFX + nameof(Value));
            private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + nameof(Update));

            #endregion

            #region IEquatable

            [DebuggerStepThrough]
            public bool Equals(KVPDisplayWrapper other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return Equals(_updateAction, other._updateAction) &&
                       (_disableTitle == other._disableTitle) &&
                       (_title == other._title) &&
                       (_subtitle == other._subtitle) &&
                       _color.Equals(other._color) &&
                       _pair.Equals(other._pair);
            }

            [DebuggerStepThrough]
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((KVPDisplayWrapper)obj);
            }

            [DebuggerStepThrough]
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = _updateAction != null ? _updateAction.GetHashCode() : 0;
                    hashCode = (hashCode * 397) ^ _disableTitle.GetHashCode();
                    hashCode = (hashCode * 397) ^ (_title != null ? _title.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (_subtitle != null ? _subtitle.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ _color.GetHashCode();
                    hashCode = (hashCode * 397) ^ _pair.GetHashCode();
                    return hashCode;
                }
            }

            [DebuggerStepThrough]
            public static bool operator ==(KVPDisplayWrapper left, KVPDisplayWrapper right)
            {
                return Equals(left, right);
            }

            [DebuggerStepThrough]
            public static bool operator !=(KVPDisplayWrapper left, KVPDisplayWrapper right)
            {
                return !Equals(left, right);
            }

            #endregion
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AppaLookup<TKey, TValue, TKeyList, TValueList>) + ".";

        private static readonly ProfilerMarker _PRF_INTERNAL_RECREATE =
            new ProfilerMarker(_PRF_PFX + nameof(INTERNAL_RECREATE));

        private static readonly ProfilerMarker _PRF_Remove = new(_PRF_PFX + nameof(Remove));
        private static readonly ProfilerMarker _PRF_Add = new(_PRF_PFX + nameof(Add));

        private static readonly ProfilerMarker _PRF_GetOrCreate =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrCreate));

        private static readonly ProfilerMarker _PRF_CopyTo = new(_PRF_PFX + nameof(CopyTo));
        private static readonly ProfilerMarker _PRF_Contains = new(_PRF_PFX + nameof(Contains));
        private static readonly ProfilerMarker _PRF_TryGetValue = new(_PRF_PFX + nameof(TryGetValue));

        private static readonly ProfilerMarker _PRF_TryGetValueWithFallback =
            new(_PRF_PFX + nameof(TryGetValueWithFallback));

        private static readonly ProfilerMarker _PRF_Count = new(_PRF_PFX + nameof(Count));
        private static readonly ProfilerMarker _PRF_Indexer = new(_PRF_PFX + "Indexer");
        private static readonly ProfilerMarker _PRF_ContainsKey = new(_PRF_PFX + nameof(ContainsKey));

        private static readonly ProfilerMarker _PRF_AddIfKeyNotPresent =
            new(_PRF_PFX + nameof(AddIfKeyNotPresent));

        private static readonly ProfilerMarker _PRF_AddOrUpdateIf = new(_PRF_PFX + nameof(AddOrUpdateIf));
        private static readonly ProfilerMarker _PRF_AddOrUpdate = new(_PRF_PFX + nameof(AddOrUpdate));

        private static readonly ProfilerMarker _PRF_RemoveByKey = new(_PRF_PFX + nameof(RemoveByKey));
        private static readonly ProfilerMarker _PRF_RemoveAt = new(_PRF_PFX + nameof(RemoveAt));

        private static readonly ProfilerMarker _PRF_AddOrUpdateRange =
            new(_PRF_PFX + nameof(AddOrUpdateRange));

        private static readonly ProfilerMarker _PRF_Clear = new(_PRF_PFX + nameof(Clear));
        private static readonly ProfilerMarker _PRF_GetKeyByIndex = new(_PRF_PFX + nameof(GetKeyByIndex));
        private static readonly ProfilerMarker _PRF_GetByIndex = new(_PRF_PFX + nameof(GetByIndex));
        private static readonly ProfilerMarker _PRF_Get = new(_PRF_PFX + nameof(Get));
        private static readonly ProfilerMarker _PRF_TryGet = new(_PRF_PFX + nameof(TryGet));
        private static readonly ProfilerMarker _PRF_IfPresent = new(_PRF_PFX + nameof(IfPresent));
        private static readonly ProfilerMarker _PRF_SumCounts = new(_PRF_PFX + nameof(SumCounts));

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new(_PRF_PFX + nameof(ISerializationCallbackReceiver.OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new(_PRF_PFX + nameof(ISerializationCallbackReceiver.OnAfterDeserialize));

        private static readonly ProfilerMarker _PRF_RemoveNulls = new(_PRF_PFX + nameof(RemoveNulls));
        private static readonly ProfilerMarker _PRF_Any = new(_PRF_PFX + nameof(Any));
        private static readonly ProfilerMarker _PRF_All = new(_PRF_PFX + nameof(All));

        private static readonly ProfilerMarker _PRF_First = new(_PRF_PFX + nameof(First_NoAlloc));

        private static readonly ProfilerMarker _PRF_FirstOrDefault =
            new(_PRF_PFX + nameof(FirstOrDefault_NoAlloc));

        private static readonly ProfilerMarker _PRF_FirstWithPreference_NoAlloc =
            new(_PRF_PFX + nameof(FirstWithPreference_NoAlloc));

        private static readonly ProfilerMarker _PRF_INTERNAL_CLEAR = new(_PRF_PFX + nameof(INTERNAL_CLEAR));

        private static readonly ProfilerMarker _PRF_INTERNAL_TRY_GET =
            new(_PRF_PFX + nameof(INTERNAL_TRY_GET));

        private static readonly ProfilerMarker _PRF_INTERNAL_GET = new(_PRF_PFX + nameof(INTERNAL_GET));

        private static readonly ProfilerMarker _PRF_INTERNAL_GET_KEY_BY_INDEX =
            new(_PRF_PFX + nameof(INTERNAL_GET_KEY_BY_INDEX));

        private static readonly ProfilerMarker _PRF_INTERNAL_GET_VALUE_BY_INDEX =
            new(_PRF_PFX + nameof(INTERNAL_GET_VALUE_BY_INDEX));

        private static readonly ProfilerMarker _PRF_INTERNAL_CONTAINS =
            new(_PRF_PFX + nameof(INTERNAL_CONTAINS));

        private static readonly ProfilerMarker _PRF_INTERNAL_REMOVE = new(_PRF_PFX + nameof(INTERNAL_REMOVE));
        private static readonly ProfilerMarker _PRF_INTERNAL_ADD = new(_PRF_PFX + nameof(INTERNAL_ADD));

        private static readonly ProfilerMarker _PRF_INTERNAL_UPDATE_BY_INDEX =
            new(_PRF_PFX + nameof(INTERNAL_UPDATE_BY_INDEX));

        private static readonly ProfilerMarker _PRF_INTERNAL_UPDATE = new(_PRF_PFX + nameof(INTERNAL_UPDATE));

        private static readonly ProfilerMarker _PRF_INTERNAL_INITIALIZE =
            new(_PRF_PFX + nameof(INTERNAL_INITIALIZE));

        private static readonly ProfilerMarker _PRF_INTERNAL_INITIALIZE_EXECUTE =
            new(_PRF_PFX + nameof(INTERNAL_INITIALIZE_EXECUTE));

        private static readonly ProfilerMarker _PRF_INTERNAL_CHECK_FATAL =
            new(_PRF_PFX + nameof(INTERNAL_CHECK_FATAL));

        private static readonly ProfilerMarker _PRF_INTERNAL_REQUIRES_REBUILD =
            new(_PRF_PFX + nameof(INTERNAL_REQUIRES_REBUILD));

        private static readonly ProfilerMarker _PRF_INTERNAL_REBUILD =
            new(_PRF_PFX + nameof(INTERNAL_REBUILD));

        private static readonly ProfilerMarker _PRF_Reverse = new(_PRF_PFX + nameof(Reverse));
        private static readonly ProfilerMarker _PRF_SortByKey = new(_PRF_PFX + nameof(SortByKey));
        private static readonly ProfilerMarker _PRF_SortByValue = new(_PRF_PFX + nameof(SortByValue));
        private static readonly ProfilerMarker _PRF_Insert = new(_PRF_PFX + nameof(Insert));
        private static readonly ProfilerMarker _PRF_GetKeyValuePair = new(_PRF_PFX + nameof(GetKeyValuePair));

        #endregion

#if UNITY_EDITOR

        private static readonly ProfilerMarker _PRF_ClearFully =
            new ProfilerMarker(_PRF_PFX + nameof(ClearFully));

        [Button]
        private void ClearFully()
        {
            using (_PRF_ClearFully.Auto())
            {
                values.Clear();
                keys.Clear();
                markAsModifiedAction.Invoke();
            }
        }
#endif
    }
}
