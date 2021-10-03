#region

using UnityEditor;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class TextureImporterExtensions
    {
        public static void WriteSettings(this TextureImporter importer)
        {
            AssetDatabase.WriteImportSettingsIfDirty(importer.assetPath);
        }
    }
}
