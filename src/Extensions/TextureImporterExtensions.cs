#region

using Appalachia.CI.Integration.Assets;
using UnityEditor;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class TextureImporterExtensions
    {
        public static void WriteSettings(this TextureImporter importer)
        {
            AssetDatabaseManager.WriteImportSettingsIfDirty(importer.assetPath);
        }
    }
}
