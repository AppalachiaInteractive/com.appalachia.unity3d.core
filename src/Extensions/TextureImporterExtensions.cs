#region

using Appalachia.CI.Integration.Assets;

#endregion

namespace Appalachia.Core.Extensions
{
#if UNITY_EDITOR
    public static class TextureImporterExtensions
    {
        public static void WriteSettings(this UnityEditor.TextureImporter importer)
        {
            AssetDatabaseManager.WriteImportSettingsIfDirty(importer.assetPath);
        }
    }
#endif
}
