#region

using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Objects.Root;

#endregion

namespace Appalachia.Core.Objects.Scriptables
{
    public abstract class NestedAppalachiaObject<T> : AppalachiaObject<T>
        where T : NestedAppalachiaObject<T>
    {
        protected abstract string DefaultName { get; }

        // ReSharper disable once UnusedParameter.Global
        public abstract void InitializeFromParent(AppalachiaObject parent);

#if UNITY_EDITOR
        public static T CreateNested(AppalachiaObject parent, bool initialize = true)
        {
            var path = AssetDatabaseManager.GetAssetPath(parent);
            var subAssets = AssetDatabaseManager.LoadAllAssetsAtPath(path);

            T instance = null;

            for (var index = 0; index < subAssets.Length; index++)
            {
                var subAsset = subAssets[index];
                if (subAsset is T existing)
                {
                    instance = existing;
                    break;
                }
            }

            if (instance == null)
            {
                instance = CreateInstance<T>();
                instance.name = instance.DefaultName;
                AssetDatabaseManager.AddObjectToAsset(instance, parent);
            }

            instance.name = instance.DefaultName;

            if (initialize)
            {
                instance.InitializeFromParent(parent);
            }

            return instance;
        }
#endif
    }
}
