#region

using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class TextureExtensions
    {
        #region Static Fields and Autoproperties

        private static Type type;

        #endregion

        private static Type spriteType =>
            type == null ? type = Type.GetType("UnityEditor.Sprites.SpriteUtility, UnityEditor") : type;

        public static void Copy(this Texture2D texture, Texture2D other)
        {
            using (_PRF_Multiply.Auto())
            {
                texture.ModifyPixels(pixel =>
                    {
                        return other.GetPixelBilinear(pixel.widthTime, pixel.heightTime);
                    }
                );
            }
        }

        public static Color GetAverageColor(this Texture2D texture, bool ignoreAlpha, bool ignoreBlack)
        {
            using (_PRF_GetAverageColor.Auto())
            {
                texture.SetReadable();

                var pixels = texture.GetPixels();

                var sum = Vector3.zero;
                var colors = 0f;

                for (var j = 0; j < pixels.Length; j++)
                {
                    var pixel = pixels[j];

                    if (ignoreAlpha && (pixel.a < .01f))
                    {
                        continue;
                    }

                    if (ignoreBlack && (pixel.r < .01f) && (pixel.g < .01f) && (pixel.b < .01f))
                    {
                        continue;
                    }

                    sum += new Vector3(pixel.r, pixel.g, pixel.b);
                    colors += 1f;
                }

                var average = sum / colors;

                return new Color(average.x, average.y, average.z, 1f);
            }
        }

        public static void IteratePixels(
            this Texture2D texture,
            Action<TexturePixel> iterationAction,
            bool continueOnError = false)
        {
            using (_PRF_IteratePixels.Auto())
            {
                texture.SetReadable();

                var pixels = texture.GetPixels();

                for (var y = 0; y < texture.height; y++) // bottom to top
                for (var x = 0; x < texture.width; x++)  // left to right
                {
                    var index = (y * texture.width) + x;
                    var pixel = pixels[index];

                    try
                    {
                        var set = new TexturePixel(pixel, x, y, index, texture, pixels);

                        iterationAction(set);
                    }
                    catch
                    {
                        if (continueOnError)
                        {
                            continue;
                        }

                        throw;
                    }
                }
            }
        }

        public static void ModifyPixels(
            this Texture2D texture,
            Func<TexturePixel, Color> iterationAction,
            bool continueOnError = false)
        {
            using (_PRF_ModifyPixels.Auto())
            {
                texture.SetReadable();

                var pixels = texture.GetPixels();

                for (var y = 0; y < texture.height; y++) // bottom to top
                for (var x = 0; x < texture.width; x++)  // left to right
                {
                    var index = (y * texture.width) + x;
                    var pixel = pixels[index];

                    try
                    {
                        var set = new TexturePixel(pixel, x, y, index, texture, pixels);

                        pixel = iterationAction(set);
                        pixels[index] = pixel;
                    }
                    catch
                    {
                        if (continueOnError)
                        {
                            continue;
                        }

                        throw;
                    }
                }

                texture.SetPixels(pixels);
                texture.Apply();
            }
        }

        public static void Multiply(this Texture2D texture, Color multiplyBy)
        {
            using (_PRF_Multiply.Auto())
            {
                texture.ModifyPixels(pixel => pixel.color * multiplyBy);
            }
        }

        public static void SetReadable(this Texture2D texture)
        {
            using (_PRF_SetReadable.Auto())
            {
#if UNITY_EDITOR
                var importer = texture.GetTextureImporter();

                if (importer == null)
                {
                    return;
                }

                if (importer.isReadable)
                {
                    return;
                }

                importer.isReadable = true;

                importer.SaveAndReimport();
#endif
            }
        }

        public static void SetReadable(this Texture2D texture, bool normalTexture)
        {
#if UNITY_EDITOR
            if (null == texture)
            {
                return;
            }

            var assetPath = AssetDatabaseManager.GetAssetPath(texture);
            var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (tImporter != null)
            {
                tImporter.textureType = normalTexture
                    ? TextureImporterType.NormalMap
                    : TextureImporterType.Default;

                if (tImporter.isReadable)
                {
                    return;
                }

                tImporter.isReadable = true;
                tImporter.SaveAndReimport();
            }
#endif
        }

        #region Profiling

        private const string _PRF_PFX = nameof(TextureExtensions) + ".";

        private static readonly ProfilerMarker _PRF_ModifyPixels =
            new ProfilerMarker(_PRF_PFX + nameof(ModifyPixels));

        private static readonly ProfilerMarker
            _PRF_Multiply = new ProfilerMarker(_PRF_PFX + nameof(Multiply));

        private static readonly ProfilerMarker _PRF_SetReadable =
            new ProfilerMarker(_PRF_PFX + nameof(SetReadable));

        private static readonly ProfilerMarker _PRF_GetAverageColor =
            new ProfilerMarker(_PRF_PFX + nameof(GetAverageColor));

        private static readonly ProfilerMarker _PRF_IteratePixels =
            new ProfilerMarker(_PRF_PFX + nameof(IteratePixels));

        #endregion

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_SetRgba32Format =
            new ProfilerMarker(_PRF_PFX + nameof(SetRgba32Format));

        public static void SetRgba32Format(this Texture2D texture)
        {
            using (_PRF_SetRgba32Format.Auto())
            {
                if (null == texture)
                {
                    return;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    var pts = tImporter.GetDefaultPlatformTextureSettings();
                    pts.format = TextureImporterFormat.RGBA32;
                    tImporter.SetPlatformTextureSettings(pts);
                    tImporter.SaveAndReimport();
                }
            }
        }

        private static readonly ProfilerMarker _PRF_SetTextureSGBA =
            new ProfilerMarker(_PRF_PFX + nameof(SetTextureSGBA));

        public static void SetTextureSGBA(this Texture2D texture, bool value)
        {
            using (_PRF_SetTextureSGBA.Auto())
            {
                if (null == texture)
                {
                    return;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    tImporter.sRGBTexture = value;
                    tImporter.SaveAndReimport();
                }
            }
        }

        private static readonly ProfilerMarker _PRF_GenerateOutline =
            new ProfilerMarker(_PRF_PFX + nameof(GenerateOutline));

        public static Vector2[][] GenerateOutline(
            this Texture texture,
            Rect rect,
            float detail,
            byte alphaTolerance,
            bool holeDetection)
        {
            using (_PRF_GenerateOutline.Auto())
            {
                var opaths = new Vector2[0][];
                object[] parameters = { texture, rect, detail, alphaTolerance, holeDetection, opaths };
                var method = spriteType.GetMethod("GenerateOutline", ReflectionExtensions.PrivateStatic);
                method.Invoke(null, parameters);
                var paths = (Vector2[][])parameters[5];

                return paths;
            }
        }

        private static readonly ProfilerMarker _PRF_GetTextureImporter =
            new ProfilerMarker(_PRF_PFX + nameof(GetTextureImporter));

        public static TextureImporter GetTextureImporter(this Texture2D texture)
        {
            using (_PRF_GetTextureImporter.Auto())
            {
                var path = AssetDatabaseManager.GetAssetPath(texture);
                return (TextureImporter)AssetImporter.GetAtPath(path);
            }
        }

        private static readonly ProfilerMarker _PRF_HasCrunchCompression =
            new ProfilerMarker(_PRF_PFX + nameof(HasCrunchCompression));

        public static bool HasCrunchCompression(this Texture2D texture)
        {
            using (_PRF_HasCrunchCompression.Auto())
            {
                if (null == texture)
                {
                    return false;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    return tImporter.crunchedCompression;
                }

                return false;
            }
        }

        private static readonly ProfilerMarker _PRF_HasRgbaFormat =
            new ProfilerMarker(_PRF_PFX + nameof(HasRgbaFormat));

        public static bool HasRgbaFormat(this Texture2D texture)
        {
            using (_PRF_HasRgbaFormat.Auto())
            {
                if (null == texture)
                {
                    return false;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
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

                return false;
            }
        }

        private static readonly ProfilerMarker _PRF_RemoveCrunchCompression =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveCrunchCompression));

        public static void RemoveCrunchCompression(this Texture2D texture)
        {
            using (_PRF_RemoveCrunchCompression.Auto())
            {
                if (null == texture)
                {
                    return;
                }

                var assetPath = AssetDatabaseManager.GetAssetPath(texture);
                var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (tImporter != null)
                {
                    tImporter.crunchedCompression = false;
                    tImporter.SaveAndReimport();
                }
            }
        }
#endif
    }
}
