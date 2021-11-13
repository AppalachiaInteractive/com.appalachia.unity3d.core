#region

using Appalachia.CI.Integration.Assets;



#endregion

namespace Appalachia.Core.Scriptables
{
    public abstract class NestedAppalachiaObject<T, TP> : AppalachiaObject<T>
        where T : NestedAppalachiaObject<T, TP>
        where TP : AppalachiaObject<TP>
    {
        protected abstract string DefaultName { get; }
        public abstract void Initialize(TP parent);
#if UNITY_EDITOR
        public static T CreateNested(TP parent, bool initialize = true)
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
                instance.Initialize(parent);
            }

            return instance;
        }
#endif
    }
}
