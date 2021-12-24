#if UNITY_EDITOR

#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Addressables;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Collections.Implementations.Lists;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Reflection.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine.AddressableAssets;

#endregion

namespace Appalachia.Core.Objects.Root
{
    public sealed partial class AppalachiaRepository
    {
        public void ScanExternal()
        {
            using (_PRF_ScanExternal.Auto())
            {
                UpdateSingletonList();
            }
        }

        [Button]
        public void UpdateSingletonList()
        {
            using (_PRF_Scan.Auto())
            {
                _singletons ??= new AssetReferenceList();
                _singletonTypes ??= new stringList();

                if (AppalachiaApplication.IsPlaying)
                {
                    return;
                }

                _singletons.Clear();
                _singletonTypes.Clear();

                var singletonTypeHash = new HashSet<Type>();

                var genericType = typeof(SingletonAppalachiaObject<>);

                var allAssetPaths = AssetDatabaseManager.GetAllAssetPaths();

                foreach (var path in allAssetPaths)
                {
                    if (!AppaFile.Exists(path))
                    {
                        continue;
                    }

                    if (AppaDirectory.Exists(path))
                    {
                        continue;
                    }

                    if (!path.EndsWith(".asset"))
                    {
                        continue;
                    }

                    var allObjectsAtPath = AssetDatabaseManager.LoadAllAssetsAtPath(path);

                    for (var i = 0; i < allObjectsAtPath.Length; i++)
                    {
                        var objectAtPath = allObjectsAtPath[i];

                        if (objectAtPath == null)
                        {
                            continue;
                        }

                        var typeAtPath = objectAtPath.GetType();

                        if (typeAtPath.InheritsFrom(genericType))
                        {
                            if (objectAtPath.name != typeAtPath.Name)
                            {
                                objectAtPath.name = typeAtPath.Name;

                                var directory = AppaPath.GetDirectoryName(path);
                                var extension = AppaPath.GetExtension(path);

                                var newName = AppaPath.Combine(
                                    directory,
                                    ZString.Format("{0}{1}", typeAtPath.Name, extension)
                                );

                                AppaFile.Move(path, newName);
                            }

                            if (singletonTypeHash.Contains(typeAtPath))
                            {
                                Context.Log.Error("Duplicate types!", objectAtPath);
                                continue;
                            }

                            if (!objectAtPath.IsAddressable(out var info))
                            {
                                objectAtPath.SetAddressableGroup();

                                if (objectAtPath.IsAddressable(out var info2))
                                {
                                    singletonTypeHash.Add(typeAtPath);
                                    _singletons.Add(new AssetReference(info2.Guid));
                                    _singletonTypes.Add(typeAtPath.FullName);
                                }
                            }
                            else
                            {
                                singletonTypeHash.Add(typeAtPath);
                                _singletons.Add(new AssetReference(info.Guid));
                                _singletonTypes.Add(typeAtPath.FullName);
                            }
                        }
                    }
                }

                AssetDatabaseManager.Refresh();

                MarkAsModified();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Scan = new(_PRF_PFX + nameof(UpdateSingletonList));

        private static readonly ProfilerMarker _PRF_Scan_Search =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".Search");

        private static readonly ProfilerMarker _PRF_Scan_AssemblyLookup =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".AssemblyLookup");

        private static readonly ProfilerMarker _PRF_Scan_CheckExcluded =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".CheckExcluded");

        private static readonly ProfilerMarker _PRF_Scan_LoadInstance =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".LoadInstance");

        private static readonly ProfilerMarker _PRF_Scan_AddToList =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".AddToList");

        private static readonly ProfilerMarker _PRF_Scan_SetInstance =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".SetInstance");

        private static readonly ProfilerMarker _PRF_Scan_SetDirty =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".SetDirty");

        private static readonly ProfilerMarker _PRF_Scan_AssemblyCheck =
            new(_PRF_PFX + nameof(UpdateSingletonList) + ".AssemblyCheck");

        private static readonly ProfilerMarker _PRF_ScanExternal = new(_PRF_PFX + nameof(ScanExternal));

        #endregion
    }
}

#endif
