#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes;
using Unity.Profiling;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

#endregion

namespace Appalachia.Core.Layers
{
    public static class Layers
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(Layers) + ".";

        private static readonly ProfilerMarker _PRF_InitializeLayers =
            new(_PRF_PFX + nameof(InitializeLayers));

        private static readonly ProfilerMarker _PRF_GetMask = new(_PRF_PFX + nameof(GetMask));

        #endregion

        private const int MAX_LAYERS = 31;

        private const int MAX_TAGS = 10000;
        
        private static List<LayerInfo> _layerInfos;
        private static List<string> _layerNames;

        public static IReadOnlyList<LayerInfo> ByID
        {
            get
            {
                if (_layerInfos == null)
                {
                    _layerInfos = new List<LayerInfo>(32);
                }

                if (_layerInfos[31].Id == 0)
                {
                    InitializeLayers();
                }

                return _layerInfos;
            }
        }

        /// <summary>
        ///     <para>Given a set of layer names as defined by either a Builtin or a User Layer in the, returns the equivalent layer mask for all of them.</para>
        /// </summary>
        /// <param name="layers">List of layer names to convert to a layer mask.</param>
        /// <returns>
        ///     <para>The layer mask created from the layerNames.</para>
        /// </returns>
        public static int GetMask(params LayerInfo[] layers)
        {
            using (_PRF_GetMask.Auto())
            {
                if (layers == null)
                {
                    throw new ArgumentNullException(nameof(layers));
                }

                var num = 0;
                foreach (var layer in layers)
                {
                    if (layer.Id != -1)
                    {
                        num |= 1 << layer.Id;
                    }
                }

                return num;
            }
        }

        [ExecuteOnEnable]
        public static void InitializeLayers()
        {
            using (_PRF_InitializeLayers.Auto())
            {
                if (_layerNames == null)
                {
                    _layerNames = new List<string>(32);
                }
                
                _layerNames.Add(nameof(IDs.Default));
                _layerNames.Add(nameof(IDs.TransparentFX));
                _layerNames.Add("Ignore Raycast");
                _layerNames.Add(nameof(IDs.Terrain));
                _layerNames.Add(nameof(IDs.Water));
                _layerNames.Add(nameof(IDs.UI));
                _layerNames.Add(nameof(IDs.HUD));
                _layerNames.Add(""/*nameof(IDs.Layer7)*/);
                _layerNames.Add(nameof(IDs.Character));
                _layerNames.Add(nameof(IDs.CharacterRagdoll));
                _layerNames.Add(""/*nameof(IDs.Layer10)*/);
                _layerNames.Add(nameof(IDs.Ground));
                _layerNames.Add(nameof(IDs.Borders));
                _layerNames.Add(nameof(IDs.InGround));
                _layerNames.Add(nameof(IDs.Vegetation));
                _layerNames.Add(nameof(IDs.Rock));
                _layerNames.Add(nameof(IDs.Tree));
                _layerNames.Add(nameof(IDs.Animal));
                _layerNames.Add(nameof(IDs.Interactable));
                _layerNames.Add(""/*nameof(IDs.Layer19)*/); 
                _layerNames.Add(""/*nameof(IDs.Layer20)*/); 
                _layerNames.Add(""/*nameof(IDs.Layer21)*/); 
                _layerNames.Add(nameof(IDs.TouchReact));
                _layerNames.Add(nameof(IDs.CAMERA_NEAR));
                _layerNames.Add(nameof(IDs.CAMERA_MID));
                _layerNames.Add(nameof(IDs.CAMERA_FAR));
                _layerNames.Add(""/*nameof(IDs.Layer26)*/);
                _layerNames.Add(""/*nameof(IDs.Layer27)*/);
                _layerNames.Add(nameof(IDs.Sky));
                _layerNames.Add(nameof(IDs.Heat));
                _layerNames.Add(nameof(IDs.Management));
#if UNITY_EDITOR
                _layerNames.Add(nameof(IDs.LOCKED_EDITOR_ONLY));
#endif
                
                if (_layerInfos == null)
                {
                    _layerInfos = new List<LayerInfo>(32);
                }
                
                for (var i = 0; i < 32; i++)
                {
                    _layerInfos[i] = new LayerInfo(i);                    
                }

#if UNITY_EDITOR
                CheckLayers();
#endif
            }
        }

        public static class IDs
        {
            #region Unity Layers

            public const int Default = 00;
            public const int TransparentFX = 01;
            public const int IgnoreRaycast = 02;
            public const int Water = 04;
            public const int UI = 05;

            #endregion

            public const int Terrain = 03;
            public const int HUD = 06;
            //public const int Layer7 = 07;
            public const int Character = 08;
            public const int CharacterRagdoll = 09;
            //public const int Layer10 = 10;
            public const int Ground = 11;
            public const int Borders = 12;
            public const int InGround = 13;
            public const int Vegetation = 14;
            public const int Rock = 15;
            public const int Tree = 16;
            public const int Animal = 17;
            public const int Interactable = 18;
            //public const int Layer19 = 19;
            //public const int Layer20 = 20;
            //public const int Layer21 = 21;
            public const int TouchReact = 22;
            public const int CAMERA_NEAR = 23;
            public const int CAMERA_MID = 24;
            public const int CAMERA_FAR = 25;
            //public const int Layer26 = 26;
            //public const int Layer27 = 27;
            public const int Sky = 28;
            public const int Heat = 29;
            public const int Management = 30;
#if UNITY_EDITOR
            public const int LOCKED_EDITOR_ONLY = 31;
#endif
        }

        public static class ByName
        {
            #region Unity Layers

            public static LayerInfo Default => ByID[IDs.Default];
            public static LayerInfo TransparentFX => ByID[IDs.TransparentFX];
            public static LayerInfo IgnoreRaycast => ByID[IDs.IgnoreRaycast];
            public static LayerInfo Water => ByID[IDs.Water];
            public static LayerInfo UI => ByID[IDs.UI];

            #endregion

            public static LayerInfo Terrain => ByID[IDs.Terrain];
            public static LayerInfo HUD => ByID[IDs.HUD];
            //public static LayerInfo Layer6 => ByID[IDs.Layer6];
            //public static LayerInfo Layer7 => ByID[IDs.Layer7];
            public static LayerInfo Character => ByID[IDs.Character];
            public static LayerInfo CharacterRagdoll => ByID[IDs.CharacterRagdoll];
            //public static LayerInfo Layer10 => ByID[IDs.Layer10];
            public static LayerInfo Ground => ByID[IDs.Ground];
            public static LayerInfo Borders => ByID[IDs.Borders];
            public static LayerInfo InGround => ByID[IDs.InGround];
            public static LayerInfo Vegetation => ByID[IDs.Vegetation];
            public static LayerInfo Rock => ByID[IDs.Rock];
            public static LayerInfo Tree => ByID[IDs.Tree];
            public static LayerInfo Animal => ByID[IDs.Animal];
            public static LayerInfo Interactable => ByID[IDs.Interactable];
            //public static LayerInfo Layer19 => ByID[IDs.Layer19];
            //public static LayerInfo Layer20 => ByID[IDs.Layer20];
            //public static LayerInfo Layer21 => ByID[IDs.Layer21];
            public static LayerInfo TouchReact => ByID[IDs.TouchReact];
            public static LayerInfo CAMERA_NEAR => ByID[IDs.CAMERA_NEAR];
            public static LayerInfo CAMERA_MID => ByID[IDs.CAMERA_MID];
            public static LayerInfo CAMERA_FAR => ByID[IDs.CAMERA_FAR];
            //public static LayerInfo Layer26 => ByID[IDs.Layer26];
            //public static LayerInfo Layer27 => ByID[IDs.Layer27];
            public static LayerInfo Sky => ByID[IDs.Sky];
            public static LayerInfo Heat => ByID[IDs.Heat];
            public static LayerInfo Management => ByID[IDs.Management];

#if UNITY_EDITOR
            public static LayerInfo LOCKED_EDITOR_ONLY => ByID[IDs.LOCKED_EDITOR_ONLY];
#endif
        }

#if UNITY_EDITOR

        private static void CheckLayers()
        {
            // Open tag manager
            var tagManager = new SerializedObject(
                AssetDatabaseManager.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
            );

            // Layers Property
            var layersProp = tagManager.FindProperty("layers");

            SerializedProperty sp;

            // Start at layer 9th index -> 8 (zero based) => first 8 reserved for unity / greyed out
            for (int i = 8, j = MAX_LAYERS; i < j; i++)
            {
                var layerInfo = _layerInfos[i];
                var targetName = _layerNames[i];

                sp = layersProp.GetArrayElementAtIndex(i);

                if (sp.stringValue != targetName)
                {
                    Debug.LogWarning($"Layer [{i}] should be named [{targetName}]");

                    // Save settings
                    // tagManager.ApplyModifiedProperties();
                    // return true;
                }
            }
        }
#endif
    }
}
