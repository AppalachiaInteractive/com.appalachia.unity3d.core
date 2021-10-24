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
        private static Type spriteType =>
            type == null ? type = Type.GetType("UnityEditor.Sprites.SpriteUtility, UnityEditor") : type;

        private static Type type;

        public static TextureImporter GetTextureImporter(this Texture2D texture)
        {
            var path = AssetDatabaseManager.GetAssetPath(texture);
            return (TextureImporter) AssetImporter.GetAtPath(path);
        }

        public static Vector2[][] GenerateOutline(
            this Texture texture,
            Rect rect,
            float detail,
            byte alphaTolerance,
            bool holeDetection)
        {
            var opaths = new Vector2[0][];
            object[] parameters = {texture, rect, detail, alphaTolerance, holeDetection, opaths};
            var method = spriteType.GetMethod("GenerateOutline", ReflectionExtensions.PrivateStatic);
            method.Invoke(null, parameters);
            var paths = (Vector2[][]) parameters[5];

            return paths;
        }

        public static void ToReadable(this Texture2D texture)
        {
            var importer = texture.GetTextureImporter();

            if (importer.isReadable)
            {
                return;
            }

            importer.isReadable = true;

            importer.SaveAndReimport();
        }
    }
}
