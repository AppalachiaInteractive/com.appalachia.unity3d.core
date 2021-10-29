#region

using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Reflection.Extensions;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class TextureExtensions
    {
        private static Type type;

        private static Type spriteType =>
            type == null ? type = Type.GetType("UnityEditor.Sprites.SpriteUtility, UnityEditor") : type;

        public static Vector2[][] GenerateOutline(
            this Texture texture,
            Rect rect,
            float detail,
            byte alphaTolerance,
            bool holeDetection)
        {
#if UNITY_EDITOR
            var opaths = new Vector2[0][];
            object[] parameters = {texture, rect, detail, alphaTolerance, holeDetection, opaths};
            var method = spriteType.GetMethod("GenerateOutline", ReflectionExtensions.PrivateStatic);
            method.Invoke(null, parameters);
            var paths = (Vector2[][]) parameters[5];

            return paths;
#endif
        }

        public static TextureImporter GetTextureImporter(this Texture2D texture)
        {
#if UNITY_EDITOR
            var path = AssetDatabaseManager.GetAssetPath(texture);
            return (TextureImporter) AssetImporter.GetAtPath(path);
#endif
        }

        public static bool HasCrunchCompression(Texture2D texture)
        {
#if UNITY_EDITOR
            if (null == texture)
            {
                return false;
            }

            var assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                return tImporter.crunchedCompression;
            }

#endif
            return false;
        }

        public static bool HasRgbaFormat(Texture2D texture)
        {
#if UNITY_EDITOR
            if (null == texture)
            {
                return false;
            }

            var assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                var pts = tImporter.GetDefaultPlatformTextureSettings();
                if ((pts.format == TextureImporterFormat.ARGB32) ||
                    (pts.format == TextureImporterFormat.RGBA32))
                {
                    return true;
                }

                return false;
            }
#endif
            return false;
        }

        public static void RemoveCrunchCompression(Texture2D texture)
        {
#if UNITY_EDITOR
            if (null == texture)
            {
                return;
            }

            var assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.crunchedCompression = false;
                tImporter.SaveAndReimport();
            }
#endif
        }

        public static void SetReadable(this Texture2D texture)
        {
#if UNITY_EDITOR
            var importer = texture.GetTextureImporter();

            if (importer.isReadable)
            {
                return;
            }

            importer.isReadable = true;

            importer.SaveAndReimport();
#endif
        }

        public static void SetReadable(Texture2D texture, bool normalTexture)
        {
#if UNITY_EDITOR
            if (null == texture)
            {
                return;
            }

            var assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.textureType =
                    normalTexture ? TextureImporterType.NormalMap : TextureImporterType.Default;

                if (tImporter.isReadable)
                {
                    return;
                }

                tImporter.isReadable = true;
                tImporter.SaveAndReimport();
            }
#endif
        }

        public static void SetRgba32Format(Texture2D texture)
        {
#if UNITY_EDITOR
            if (null == texture)
            {
                return;
            }

            var assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                var pts = tImporter.GetDefaultPlatformTextureSettings();
                pts.format = TextureImporterFormat.RGBA32;
                tImporter.SetPlatformTextureSettings(pts);
                tImporter.SaveAndReimport();
            }
#endif
        }

        public static void SetTextureSGBA(Texture2D texture, bool value)
        {
#if UNITY_EDITOR
            if (null == texture)
            {
                return;
            }

            var assetPath = AssetDatabase.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.sRGBTexture = value;
                tImporter.SaveAndReimport();
            }
#endif
        }
    }
}
