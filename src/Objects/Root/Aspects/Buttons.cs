using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Assets;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject
    {
        [ButtonGroup(GROUP_WORKFLOW_PROD)]
        [PropertyOrder(-40000)]
        [ShowIf(nameof(ShowWorkflow))]
        public void Duplicate()
        {
            using (_PRF_Duplicate.Auto())
            {
                var path = AssetDatabaseManager.GenerateUniqueAssetPath(
                    AssetDatabaseManager.GetAssetPath(this)
                );
                var newInstance = Instantiate(this);
                AssetDatabaseManager.CreateAsset(newInstance, path);
                UnityEditor.Selection.activeObject = newInstance;
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        public void Ping()
        {
            using (_PRF_Ping.Auto())
            {
                UnityEditor.EditorGUIUtility.PingObject(this);
            }
        }

        [ButtonGroup(GROUP_BUTTONS)]
        [PropertyOrder(-1000)]
        public void SaveNow()
        {
            using (_PRF_SaveNow.Auto())
            {
                MarkAsModified();

                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    AssetDatabaseSaveManager.SaveAssetsNextFrame();
                }
            }
        }

        [ShowIfGroup(SHOW_WORKFLOW)]
        [FoldoutGroup(GROUP_WORKFLOW, Order = -50000)]
        [ButtonGroup(GROUP_WORKFLOW_PROD)]
        [PropertyOrder(-40000)]
        [ShowIf(nameof(ShowWorkflow))]
        public void Select()
        {
            using (_PRF_Select.Auto())
            {
                AssetDatabaseManager.SetSelection(this);
            }
        }

        [ButtonGroup(GROUP_BUTTONS), HideInInlineEditors]
        private void ReInitialize()
        {
            _initializer.Reset(this, DateTime.Now.ToString("O"));

            InitializeSynchronous();
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Ping = new ProfilerMarker(_PRF_PFX + nameof(Ping));
        private static readonly ProfilerMarker _PRF_Select = new ProfilerMarker(_PRF_PFX + nameof(Select));

        private static readonly ProfilerMarker _PRF_Duplicate =
            new ProfilerMarker(_PRF_PFX + nameof(Duplicate));

        private static readonly ProfilerMarker _PRF_SaveNow = new ProfilerMarker(_PRF_PFX + nameof(SaveNow));

        private static readonly ProfilerMarker _PRF_ReInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(ReInitialize));

        #endregion
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
    }

    public partial class AppalachiaBehaviour
    {
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
