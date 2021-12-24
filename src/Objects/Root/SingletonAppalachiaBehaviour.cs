#region

using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

// ReSharper disable StaticMemberInGenericType

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class SingletonAppalachiaBehaviour<T> : AppalachiaBehaviour<T>
        where T : SingletonAppalachiaBehaviour<T>
    {
        protected virtual bool DestroyObjectOfSubsequentInstances => false;

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                Context.Log.Info(nameof(Initialize), this);

                await initializer.Do(
                    this,
                    nameof(EnsureInstanceIsPrepared),
                    !IsInstanceAvailable,
                    () => EnsureInstanceIsPrepared(this as T)
                );
            }
        }

        private static void EnsureInstanceIsPrepared(T callingInstance)
        {
            using (_PRF_EnsureInstanceIsPrepared.Auto())
            {
                lock (InstanceWriteLock)
                {
                    var currentFieldInstance = instance;

                    var hasExistingInstance = currentFieldInstance != null;
                    var wasProvidedInstance = callingInstance != null;
                    var isCheckingFromMainInstance = callingInstance == currentFieldInstance;

                    if (hasExistingInstance && isCheckingFromMainInstance)
                    {
                        return;
                    }

                    if (hasExistingInstance && !wasProvidedInstance)
                    {
                        return;
                    }

                    if (!hasExistingInstance && wasProvidedInstance)
                    {
                        SetInstance(callingInstance);
                        return;
                    }

                    if (hasExistingInstance)
                    {
                        if (currentFieldInstance.gameObject.scene.isLoaded &&
                            !callingInstance.gameObject.scene.isLoaded)
                        {
                            if (callingInstance.DestroyObjectOfSubsequentInstances)
                            {
                                callingInstance.gameObject.DestroySafely();
                            }
                            else
                            {
                                callingInstance.DestroySafely();
                            }

                            SingletonContext.Error("Multiple instances!", currentFieldInstance);
                        }
                        else if (!currentFieldInstance.gameObject.scene.isLoaded &&
                                 callingInstance.gameObject.scene.isLoaded)
                        {
                            if (currentFieldInstance.DestroyObjectOfSubsequentInstances)
                            {
                                currentFieldInstance.gameObject.DestroySafely();
                            }
                            else
                            {
                                currentFieldInstance.DestroySafely();
                            }

                            SetInstance(callingInstance);

                            SingletonContext.Error("Multiple instances!", callingInstance);
                        }
                        else
                        {
                            SingletonContext.Error("Multiple instances!", currentFieldInstance);
                            SingletonContext.Error("Multiple instances!", callingInstance);
                        }

                        return;
                    }

                    SingletonContext.Info(
                        ZString.Format(
                            "No singleton behaviour of [{0}] is set.  Attempting to resolve...",
                            typeof(T).Name.FormatNameForLogging()
                        )
                    );

                    currentFieldInstance = instance;

                    if (currentFieldInstance == null)
                    {
                        SingletonContext.Info(
                            ZString.Format(
                                "Searching for existing [{0}] singleton via {1}.{2})!",
                                typeof(T).Name.FormatNameForLogging(),
                                nameof(Object),
                                nameof(FindObjectOfType)
                            )
                        );

                        var findResult = FindObjectOfType<T>(true);

                        if (findResult != null)
                        {
                            SetInstance(findResult);
                        }
                    }

                    currentFieldInstance = instance;

                    if (currentFieldInstance == null)
                    {
                        SingletonContext.Info(
                            ZString.Format(
                                "Searching for existing [{0}] singleton via {1}.{2})!",
                                typeof(T).Name.FormatNameForLogging(),
                                nameof(SceneManager),
                                nameof(SceneManager.GetSceneAt)
                            )
                        );

                        for (var i = 0; i < SceneManager.sceneCount; i++)
                        {
                            var scene = SceneManager.GetSceneAt(i);

                            if (!scene.isLoaded)
                            {
                                continue;
                            }

                            var activeSceneRoots = scene.GetRootGameObjects();

                            for (var sceneRootIndex = 0;
                                 sceneRootIndex < activeSceneRoots.Length;
                                 sceneRootIndex++)
                            {
                                var activeSceneRoot = activeSceneRoots[sceneRootIndex];
                                var sceneFindResult = activeSceneRoot.GetComponentInChildren<T>();

                                if (sceneFindResult != null)
                                {
                                    SetInstance(sceneFindResult);

                                    break;
                                }
                            }

                            currentFieldInstance = instance;

                            if (currentFieldInstance != null)
                            {
                                break;
                            }
                        }
                    }

                    currentFieldInstance = instance;

                    if (currentFieldInstance == null)
                    {
                        SingletonContext.Info(
                            ZString.Format(
                                "Searching for existing [{0}] singleton via {1}.{2})!",
                                typeof(T).Name.FormatNameForLogging(),
                                nameof(Resources),
                                nameof(Resources.FindObjectsOfTypeAll)
                            )
                        );

                        var objects = Resources.FindObjectsOfTypeAll<T>();

                        for (var index = 0; index < objects.Length; index++)
                        {
                            var obj = objects[index];
                            if (obj != null)
                            {
                                SetInstance(obj);
                                break;
                            }
                        }
                    }

                    currentFieldInstance = instance;

                    if (currentFieldInstance != null)
                    {
                        SingletonContext.Info("Found existing singleton behaviour!", currentFieldInstance);

                        currentFieldInstance.MarkAsModified();
                        return;
                    }

                    var go = new GameObject(typeof(T).GetSimpleReadableName());
                    var newInstance = go.AddComponent<T>();

                    SetInstance(newInstance);

                    currentFieldInstance = instance;

                    SingletonContext.Info("Creating singleton behaviour!", currentFieldInstance);

                    currentFieldInstance.MarkAsModified();
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SingletonAppalachiaBehaviour<T>) + ".";

        private static readonly ProfilerMarker _PRF_EnsureInstanceIsPrepared =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureInstanceIsPrepared));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
