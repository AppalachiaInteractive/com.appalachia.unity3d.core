#if UNITY_EDITOR
using System;
using Appalachia.CI.Integration.Core;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaObject
    {
        public static ScriptableObject CreateNew(Type t, Type ownerType = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                return AppalachiaObjectFactory.CreateNewAsset(
                    t,
                    ownerType: ownerType,
                    overwriteExisting: false
                );
            }
        }

        public static ScriptableObject CreateNew(
            Type t,
            string name,
            ScriptableObject i = null,
            string dataFolder = null,
            Type ownerType = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                if (i == null)
                {
                    i = CreateInstance(t);
                }

                return AppalachiaObjectFactory.SaveInstanceToAsset(t, name, i, dataFolder, ownerType);
            }
        }

        public static T CreateNew<T>(Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_CreateNew.Auto())
            {
                return CreateNew(typeof(T), ownerType) as T;
            }
        }

        public static T CreateNew<T>(string name, T i = null, string dataFolder = null, Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_CreateNew.Auto())
            {
                return CreateNew(typeof(T), name, i, dataFolder, ownerType) as T;
            }
        }

        public static string CreateTargetName<T1, T2>()
        {
            return $"{typeof(T1).Name}{typeof(T2).Name}";
        }

        public static ScriptableObject LoadOrCreateNew(
            Type t,
            string name,
            string dataFolder = null,
            Type ownerType = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return AppalachiaObjectFactory.LoadExistingOrCreateNewAsset(t, name, dataFolder, ownerType);
            }
        }

        public static T LoadOrCreateNew<T>(string name, string dataFolder = null, Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return LoadOrCreateNew(typeof(T), name, dataFolder, ownerType) as T;
            }
        }

        public static void LoadOrCreateNewIfNull(
            Type t,
            ref ScriptableObject assignment,
            string name,
            string dataFolder = null,
            Type ownerType = null)
        {
            using (_PRF_LoadOrCreateNewIfNull.Auto())
            {
                if (assignment == null)
                {
                    assignment = AppalachiaObjectFactory.LoadExistingOrCreateNewAsset(
                        t,
                        name,
                        dataFolder,
                        ownerType
                    );
                }
            }
        }

        public static void LoadOrCreateNewIfNull<T>(
            ref T assignment,
            string name,
            string dataFolder = null,
            Type ownerType = null)
            where T : AppalachiaObject
        {
            using (_PRF_LoadOrCreateNewIfNull.Auto())
            {
                ScriptableObject obj = assignment;

                LoadOrCreateNewIfNull(typeof(T), ref obj, name, dataFolder, ownerType);

                assignment = obj as T;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_LoadOrCreateNew =
            new ProfilerMarker(_PRF_PFX + nameof(LoadOrCreateNew));

        private static readonly ProfilerMarker _PRF_CreateNew =
            new ProfilerMarker(_PRF_PFX + nameof(CreateNew));

        private static readonly ProfilerMarker _PRF_LoadOrCreateNewIfNull =
            new ProfilerMarker(_PRF_PFX + nameof(LoadOrCreateNewIfNull));

        #endregion
    }

    public abstract partial class AppalachiaObject<T>
    {
        public static T CreateNew(Type ownerType = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                return CreateNew(typeof(T), ownerType) as T;
            }
        }

        public static T CreateNew(string name, T i = null, string dataFolder = null, Type ownerType = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                return CreateNew(typeof(T), name, i, dataFolder, ownerType) as T;
            }
        }

        public static T LoadOrCreateNew(string name, string dataFolder = null, Type ownerType = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return LoadOrCreateNew(typeof(T), name, dataFolder, ownerType) as T;
            }
        }

        public static void LoadOrCreateNewIfNull(
            ref T assignment,
            string name,
            string dataFolder = null,
            Type ownerType = null)
        {
            using (_PRF_LoadOrCreateNewIfNull.Auto())
            {
                ScriptableObject obj = assignment;

                LoadOrCreateNewIfNull(typeof(T), ref obj, name, dataFolder, ownerType);

                assignment = obj as T;
            }
        }

        #region Profiling

        private static readonly string _PRF_PFX3 = typeof(T).Name + ".";

        private static readonly ProfilerMarker _PRF_CreateNew =
            new ProfilerMarker(_PRF_PFX3 + nameof(CreateNew));

        private static readonly ProfilerMarker _PRF_LoadOrCreateNew =
            new ProfilerMarker(_PRF_PFX3 + nameof(LoadOrCreateNew));

        private static readonly ProfilerMarker _PRF_LoadOrCreateNewIfNull =
            new ProfilerMarker(_PRF_PFX3 + nameof(LoadOrCreateNewIfNull));

        #endregion
    }
}

#endif
