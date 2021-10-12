using System;
using System.IO;
using Appalachia.CI.Integration.Extensions;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Assets
{
    public static partial class AssetDatabaseManager
    {
        public static Texture2D SaveTextureAssetToFile<T>(T owner, Texture2D texture)
            where T : MonoBehaviour
        {
            try
            {
                var fileName = texture.name;

                if (fileName.EndsWith(".png"))
                {
                    fileName = fileName.Replace(".png", string.Empty);
                    texture.name = fileName;
                }

                var savePathMetadata = GetSaveLocationForOwnedAsset<T, Texture2D>("x.png");

                var targetSavePath = Path.Combine(savePathMetadata.relativePath, $"{fileName}.png");

                var absolutePath = targetSavePath;

                if (absolutePath.StartsWith("Assets"))
                {
                    absolutePath = targetSavePath.ToAbsolutePath();
                }

                var directoryName = Path.GetDirectoryName(absolutePath);
                Directory.CreateDirectory(directoryName);

                var bytes = texture.EncodeToPNG();
                File.WriteAllBytes(absolutePath, bytes);

                AssetDatabase.ImportAsset(targetSavePath);

                texture = AssetDatabase.LoadAssetAtPath<Texture2D>(targetSavePath);

                var tImporter = AssetImporter.GetAtPath(targetSavePath) as TextureImporter;
                if (tImporter != null)
                {
                    tImporter.textureType = TextureImporterType.Default;

                    tImporter.wrapMode = TextureWrapMode.Clamp;
                    tImporter.sRGBTexture = false;
                    tImporter.alphaSource = TextureImporterAlphaSource.None;
                    tImporter.SaveAndReimport();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            return texture;
        }
    }
}
