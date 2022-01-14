using Appalachia.Core.Types.Enums;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Extensions
{
    public static class RenderTextureExtensions
    {
        public static int GetRenderQualityPixelResolution(this RenderQuality renderQuality)
        {
            using (_PRF_GetRenderQualityPixelResolution.Auto())
            {
                switch (renderQuality)
                {
                    case RenderQuality.XLow_32:
                        return 32;
                    case RenderQuality.VLow_64:
                        return 64;
                    case RenderQuality.Low_128:
                        return 128;
                    case RenderQuality.Mid_256:
                        return 256;
                    case RenderQuality.Mid_512:
                        return 512;
                    case RenderQuality.High_1024:
                        return 1024;
                    case RenderQuality.VHigh_2048:
                        return 2048;
                    case RenderQuality.XHigh_4096:
                        return 4096;
                    default:
                        return 1024;
                }
            }
        }

        public static RenderTexture Recreate(
            this RenderTexture old,
            RenderQuality renderTextureQuality,
            RenderTextureFormat renderTextureFormat,
            FilterMode filterMode,
            int depth)
        {
            using (_PRF_Recreate.Auto())
            {
                var textureResolution = GetRenderQualityPixelResolution(renderTextureQuality);

                var rt = new RenderTexture(
                    textureResolution,
                    textureResolution,
                    24,
                    renderTextureFormat,
                    RenderTextureReadWrite.Linear
                )
                {
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = filterMode,
                    depth = depth,
                    autoGenerateMips = false,
                    hideFlags = HideFlags.DontSave
                };

                if (old)
                {
                    Object.DestroyImmediate(old);
                }

                return rt;
            }
        }

        public static Texture2D ToTexture2D(this RenderTexture texture, bool linear = true)
        {
            using (_PRF_ToTexture2D.Auto())
            {
                var active = RenderTexture.active;
                RenderTexture temporary = null;
                Texture2D result = null;

                try
                {
                    var width = texture.width;
                    var height = texture.height;

                    temporary = RenderTexture.GetTemporary(
                        width,
                        height,
                        0,
                        RenderTextureFormat.ARGB32,
                        RenderTextureReadWrite.Default,
                        8
                    );

                    RenderTexture.active = temporary;
                    temporary.DiscardContents(true, true);
                    GL.Clear(true, true, new Color(1f, 1f, 1f, 0.0f));

                    var sRGBWrite = GL.sRGBWrite;

                    GL.sRGBWrite = (QualitySettings.activeColorSpace == ColorSpace.Linear) && !linear;

                    Graphics.Blit(texture, temporary);
                    GL.sRGBWrite = sRGBWrite;

                    result = new Texture2D(width, height, TextureFormat.ARGB32, false, true)
                    {
                        filterMode = FilterMode.Point
                    };

                    var rect = new Rect(0, 0, width, height);

                    result.ReadPixels(rect, 0, 0);
                    result.Apply();
                }
                finally
                {
                    RenderTexture.active = active;

                    if (temporary != null)
                    {
                        RenderTexture.ReleaseTemporary(temporary);
                    }
                }

                return result;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(RenderTextureExtensions) + ".";

        private static readonly ProfilerMarker _PRF_ToTexture2D =
            new ProfilerMarker(_PRF_PFX + nameof(ToTexture2D));

        private static readonly ProfilerMarker _PRF_GetRenderQualityPixelResolution =
            new ProfilerMarker(_PRF_PFX + nameof(GetRenderQualityPixelResolution));

        private static readonly ProfilerMarker _PRF_Recreate =
            new ProfilerMarker(_PRF_PFX + nameof(Recreate));

        private static readonly ProfilerMarker _PRF_CopyToTexture =
            new ProfilerMarker(_PRF_PFX + nameof(ToTexture2D));

        #endregion
    }
}
