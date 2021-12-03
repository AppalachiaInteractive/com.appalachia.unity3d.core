using UnityEngine;

namespace Appalachia.Core.Extensions
{
    public  readonly struct TexturePixel
    {
        public TexturePixel(Color pixel, int x, int y, int index, Texture2D texture, Color[] pixels)
        {
            this.color = pixel;
            this.x = x;
            this.y = y;
            this.index = index;
            this.width = texture.width;
            this.height = texture.height;
            this.widthTime = x / (float)(texture.width - 1);
            this.heightTime = y / (float)(texture.height - 1);
            this.pixels = pixels;
        }
        
        #region Fields and Autoproperties

        public readonly Color color;
        public readonly Color[] pixels;
        public readonly float heightTime;
        public readonly float widthTime;
        public readonly int height;
        public readonly int index;
        public readonly int width;
        public readonly int x;
        public readonly int y;

        #endregion

        public bool IsBlack => (r < .001f) && (g < .001f) && (b < .001f);

        public bool IsTransparent => a < .001f;
        public bool IsWhite => (r > .999f) && (g > .999f) && (b > .999f);
        public float a => color.a;
        public float b => color.b;
        public float g => color.g;

        public float r => color.r;

        public Color GetAt(int x1, int y1)
        {
            var i = (y1 * width) + x1;

            return pixels[i];
        }
    }
}
