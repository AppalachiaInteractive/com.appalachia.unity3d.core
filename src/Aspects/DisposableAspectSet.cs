#region

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Appalachia.CI.Integration;
using Appalachia.CI.Integration.FileSystem;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Aspects
{
    public abstract class DisposableAspectSet<T>
    {
        private const int _INITIAL_ASPECT_CAPACITY = 30;

        public static DummyDisposable _dummy = new();

        private static readonly ProfilerMarker _PRF_DisposableAspectSet =
            new("DisposableAspectSet.Constructor");

        private static readonly ProfilerMarker _PRF_DisposableAspectSet_Execute =
            new("DisposableAspectSet.Execute");

        private static readonly ProfilerMarker _PRF_DisposableAspectSet_Execute_Format =
            new("DisposableAspectSet.Execute.Format");

        private static readonly ProfilerMarker _PRF_DisposableAspectSet_Execute_Markers =
            new("DisposableAspectSet.Execute.Markers");

        private static readonly ProfilerMarker _PRF_DisposableAspectSet_Execute_Create =
            new("DisposableAspectSet.Execute.Create");

        protected Dictionary<int, T> _instances;
        protected string _typePrefix;

        protected DisposableAspectSet()
        {
            using (_PRF_DisposableAspectSet.Auto())
            {
                if (_dummy == null)
                {
                    _dummy = new DummyDisposable();
                }

                _instances = new Dictionary<int, T>(_INITIAL_ASPECT_CAPACITY);
            }
        }

        public T Execute(
            out bool dummy,
            string additive = null,
            bool ignore = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ignore)
            {
                dummy = true;
                return default;
            }

            T instance;

            using (_PRF_DisposableAspectSet_Execute.Auto())
            {
                using (_PRF_DisposableAspectSet_Execute_Format.Auto())
                {
                    if (_typePrefix == null)
                    {
                        _typePrefix = AppaPath.GetFileNameWithoutExtension(sourceFilePath);
                    }
                }

                bool found;

                using (_PRF_DisposableAspectSet_Execute_Markers.Auto())
                {
                    if (_instances == null)
                    {
                        _instances = new Dictionary<int, T>(_INITIAL_ASPECT_CAPACITY);
                    }

                    found = _instances.TryGetValue(sourceLineNumber, out instance);
                }

                if (!found)
                {
                    using (_PRF_DisposableAspectSet_Execute_Create.Auto())
                    {
                        instance = Create(_typePrefix, memberName, additive, sourceLineNumber);

                        _instances[sourceLineNumber] = instance;
                    }
                }
            }

            dummy = false;
            return instance;
        }

        protected abstract T Create(
            string typePrefix,
            string memberName,
            string additive,
            int sourceLineNumber);

        public abstract IDisposable Initiate(T instance);
    }
}
