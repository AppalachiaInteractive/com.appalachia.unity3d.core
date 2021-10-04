#region

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Extensions
{
    public static class TextureExtensions
    {
        private static Type type;

        private static Type spriteType =>
            type == null
                ? type = Type.GetType("UnityEditor.Sprites.SpriteUtility, UnityEditor")
                : type;

        public static Vector2[][] GenerateOutline(
            this Texture texture,
            Rect rect,
            float detail,
            byte alphaTolerance,
            bool holeDetection)
        {
            var opaths = new Vector2[0][];
            object[] parameters = {texture, rect, detail, alphaTolerance, holeDetection, opaths};
            var method = spriteType.GetMethod(
                "GenerateOutline",
                BindingFlags.Static | BindingFlags.NonPublic
            );
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

        public static TextureImporter GetTextureImporter(this Texture2D texture)
        {
            var path = AssetDatabase.GetAssetPath(texture);
            return (TextureImporter) AssetImporter.GetAtPath(path);
        }
    }
}
