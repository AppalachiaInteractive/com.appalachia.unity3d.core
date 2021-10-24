#region

using Appalachia.Core.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Behaviours
{
    public abstract class SingletonMonoBehaviour<T> : AppalachiaMonoBehaviour
        where T : SingletonMonoBehaviour<T>
    {
        private const string _PRF_PFX = nameof(SingletonMonoBehaviour<T>) + ".";

        private static T __instance;
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));

        public static T instance => _instance;

        private static T _instance
        {
            get
            {
                if (__instance == null)
                {
                    __instance = FindObjectOfType<T>();
                }

                if (__instance == null)
                {
                    var go = new GameObject(typeof(T).GetSimpleReadableName());
                    __instance = go.AddComponent<T>();
                }

                return __instance;
            }
        }

        protected virtual void OnAwake()
        {
        }

        private void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                if ((_instance != null) && (_instance != this))
                {
#if UNITY_EDITOR
                    Selection.objects = new Object[] {_instance.gameObject};
#endif
                    this.DestroySafely();
                }
                else
                {
                    __instance = this as T;
                }

                OnAwake();
            }
        }
    }
}
