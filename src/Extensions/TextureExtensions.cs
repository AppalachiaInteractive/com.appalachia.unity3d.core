#region

using System;
using System.IO;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static partial class TextureExtensions
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
                texture.ModifyPixels(
                    pixel => { return other.GetPixelBilinear(pixel.widthTime, pixel.heightTime); }
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

        public static Texture2D Resize(
            this Texture2D source,
            int width,
            int height,
            TextureFormat textureFormat = TextureFormat.ARGB32,
            RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGB32,
            bool linear = true)
        {
            using (_PRF_Resize.Auto())
            {
                var rt = new RenderTexture(
                    width,
                    height,
                    0,
                    renderTextureFormat,
                    linear ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB
                );

                var resultingResizedTexture = new Texture2D(width, height, textureFormat, true, linear);

                try
                {
                    rt.DiscardContents();

                    GL.sRGBWrite = (QualitySettings.activeColorSpace == ColorSpace.Linear) && !linear;

                    Graphics.Blit(source, rt);
                    GL.sRGBWrite = false;

                    RenderTexture.active = rt;

                    resultingResizedTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                    resultingResizedTexture.Apply(true);

                    RenderTexture.active = null;
                }
                finally
                {
                    rt.Release();
                    rt.DestroySafely();
                }

                return resultingResizedTexture;
            }
        }

        public static void WriteToJPGFile(this Texture2D texture, string outputFilePath)
        {
            using (_PRF_WriteToJPGFile.Auto())
            {
                var bytes = texture.EncodeToJPG();
                File.WriteAllBytes(outputFilePath, bytes);
            }
        }

        public static void WriteToPNGFile(this Texture2D texture, string outputFilePath)
        {
            using (_PRF_WriteToPNGFile.Auto())
            {
                var bytes = texture.EncodeToPNG();
                File.WriteAllBytes(outputFilePath, bytes);
            }
        }

        public static void WriteToTGAFile(this Texture2D texture, string outputFilePath)
        {
            using (_PRF_WriteToTGAFile.Auto())
            {
                var bytes = texture.EncodeToTGA();
                File.WriteAllBytes(outputFilePath, bytes);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(TextureExtensions) + ".";

        private static readonly ProfilerMarker _PRF_WriteToPNGFile =
            new ProfilerMarker(_PRF_PFX + nameof(WriteToPNGFile));

        private static readonly ProfilerMarker _PRF_WriteToJPGFile =
            new ProfilerMarker(_PRF_PFX + nameof(WriteToJPGFile));

        private static readonly ProfilerMarker _PRF_WriteToTGAFile =
            new ProfilerMarker(_PRF_PFX + nameof(WriteToTGAFile));

        private static readonly ProfilerMarker _PRF_Resize = new ProfilerMarker(_PRF_PFX + nameof(Resize));

        private static readonly ProfilerMarker _PRF_ModifyPixels =
            new ProfilerMarker(_PRF_PFX + nameof(ModifyPixels));

        private static readonly ProfilerMarker
            _PRF_Multiply = new ProfilerMarker(_PRF_PFX + nameof(Multiply));

        private static readonly ProfilerMarker _PRF_GetAverageColor =
            new ProfilerMarker(_PRF_PFX + nameof(GetAverageColor));

        private static readonly ProfilerMarker _PRF_IteratePixels =
            new ProfilerMarker(_PRF_PFX + nameof(IteratePixels));

        #endregion
    }
}
