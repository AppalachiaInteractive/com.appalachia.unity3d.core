using Appalachia.Core.Types.Enums;
using UnityEngine;

namespace Appalachia.Core.Extensions
{
    public static class RenderTextureExtensions
    {
        public static RenderTexture Recreate(
            this RenderTexture old,
            RenderQuality renderTextureQuality,
            RenderTextureFormat renderTextureFormat,
            FilterMode filterMode,
            int depth)
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

        public static int GetRenderQualityPixelResolution(this RenderQuality renderQuality)
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

        public static Texture2D Capture(this RenderTexture texture)
        {
            var width = texture.width;
            var height = texture.height;

            var active = RenderTexture.active;
            var temporary = RenderTexture.GetTemporary(
                width,
                height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Default,
                8
            );
            RenderTexture.active = temporary;
            GL.Clear(false, true, new Color(1f, 1f, 1f, 0.0f));
            Graphics.Blit(texture, temporary);
            var texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
            texture2D.filterMode = FilterMode.Point;

            var rect = new Rect(0, 0, width, height);

            texture2D.ReadPixels(rect, 0, 0);
            texture2D.Apply();
            RenderTexture.active = active;
            RenderTexture.ReleaseTemporary(temporary);
            return texture2D;
        }
    }
}
