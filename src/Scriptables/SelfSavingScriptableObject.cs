#region

using System;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class SelfSavingScriptableObject<T> : AppalachiaScriptableObject<T>
        where T : SelfSavingScriptableObject<T>
    {
#if UNITY_EDITOR
        private const string _PRF_PFX = nameof(SelfSavingScriptableObject<T>) + ".";
        private static readonly ProfilerMarker _PRF_CreateNew = new(_PRF_PFX + nameof(CreateNew));

        private static readonly ProfilerMarker _PRF_LoadOrCreateNew = new(_PRF_PFX + nameof(LoadOrCreateNew));

        private static readonly ProfilerMarker _PRF_Rename = new(_PRF_PFX + nameof(Rename));

        public static T CreateNew()
        {
            using (_PRF_CreateNew.Auto())
            {
                return ScriptableObjectFactory.CreateNew<T>();
            }
        }

        public static T LoadOrCreateNew(string name, string dataFolder = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return ScriptableObjectFactory.LoadOrCreateNew<T>(name, dataFolder);
            }
        }

        public static T LoadOrCreateNew(
            string name,
            bool prependType,
            bool appendType,
            string dataFolder = null)
        {
            using (_PRF_LoadOrCreateNew.Auto())
            {
                return ScriptableObjectFactory.LoadOrCreateNew<T>(name, prependType, appendType, dataFolder);
            }
        }

        public static T CreateNew(string name, T i, string dataFolder = null)
        {
            using (_PRF_CreateNew.Auto())
            {
                return ScriptableObjectFactory.CreateNew(name, i, dataFolder);
            }
        }

        public void Rename(string newName)
        {
            using (_PRF_Rename.Auto())
            {
                ScriptableObjectFactory.Rename(this as T, newName);
            }
        }
#endif
    }
}
