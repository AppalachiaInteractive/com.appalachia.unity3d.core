#region

using System;
using Appalachia.Core.Attributes;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Layers
{
    public static class LAYERS
    {
        public static LayerInfo _03 => Layers._LAYERS[03];
        public static LayerInfo _06 => Layers._LAYERS[06];
        public static LayerInfo _07 => Layers._LAYERS[07];
        public static LayerInfo _11 => Layers._LAYERS[11];
        public static LayerInfo _12 => Layers._LAYERS[12];
        public static LayerInfo _13 => Layers._LAYERS[13];
        public static LayerInfo _14 => Layers._LAYERS[14];
        public static LayerInfo _17 => Layers._LAYERS[17];
        public static LayerInfo _18 => Layers._LAYERS[18];
        public static LayerInfo _19 => Layers._LAYERS[19];
        public static LayerInfo _20 => Layers._LAYERS[20];
        public static LayerInfo _21 => Layers._LAYERS[21];
        public static LayerInfo _24 => Layers._LAYERS[24];
        public static LayerInfo _25 => Layers._LAYERS[25];
        public static LayerInfo _26 => Layers._LAYERS[26];
        public static LayerInfo AreaVolume => Layers._LAYERS[10];
        public static LayerInfo Default => Layers._LAYERS[00];
        public static LayerInfo Environment => Layers._LAYERS[16];
        public static LayerInfo IgnoreRaycast => Layers._LAYERS[02];
        public static LayerInfo Interaction => Layers._LAYERS[28];
#if UNITY_EDITOR
        public static LayerInfo LOCKED_EDITOR_ONLY => Layers._LAYERS[31];
#endif
        public static LayerInfo OcclusionBake => Layers._LAYERS[27];
        public static LayerInfo Player => Layers._LAYERS[08];
        public static LayerInfo PostProcessing => Layers._LAYERS[09];
        public static LayerInfo Scatter => Layers._LAYERS[23];
        public static LayerInfo Terrain => Layers._LAYERS[15];
        public static LayerInfo TouchBend => Layers._LAYERS[29];
        public static LayerInfo TransparentFX => Layers._LAYERS[01];
        public static LayerInfo TransparentFX_Generation => Layers._LAYERS[30];
        public static LayerInfo UI => Layers._LAYERS[05];
        public static LayerInfo Undergrowth => Layers._LAYERS[22];
        public static LayerInfo Water => Layers._LAYERS[04];
    }

    public static class Layers
    {
        private const string _PRF_PFX = nameof(Layers) + ".";

        private static LayerInfo[] __layers;

        private static readonly ProfilerMarker _PRF_InitializeLayers =
            new(_PRF_PFX + nameof(InitializeLayers));

        private static readonly ProfilerMarker _PRF_GetMask = new(_PRF_PFX + nameof(GetMask));

        public static int Animal => 17;
        public static int Borders => 12;

        public static int CAMERA_FAR => 25;
        public static int CAMERA_MID => 24;
        public static int CAMERA_NEAR => 23;

        //public static int Layer6               => 06;
        //public static int Layer7               => 07;
        public static int Character => 08;

        public static int CharacterRagdoll => 09;

        public static int Default => 00;

        //public static int Layer10              => 10;
        public static int Ground => 11;
        public static int Heat => 29;

        public static int IgnoreRaycast => 02;
        public static int InGround => 13;

        public static int Interactable => 18;
#if UNITY_EDITOR
        public static int LOCKED_EDITOR_ONLY => 31;
#endif
        public static int Management => 30;

        //public static int Layer19              => 19; 
        public static int Reticle => 21;
        public static int Rock => 15;

        //public static int Layer26 => 26;
        //public static int Layer27 => 27;
        public static int Sky => 28;
        public static int TouchReact => 22;
        public static int TransparentFX => 01;
        public static int Tree => 16;

        public static int UI => 05;
        public static int Vegetation => 14;

        //public static int Layer3               => 03;
        public static int Water => 04;

        public static LayerInfo[] _LAYERS
        {
            get
            {
                if (__layers == null)
                {
                    __layers = new LayerInfo[32];
                }

                if (__layers[31].Id == 0)
                {
                    InitializeLayers();
                }

                return __layers;
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
                __layers = new LayerInfo[32];

                for (var i = 0; i < 32; i++)
                {
                    __layers[i] = new LayerInfo(i);
                }
            }
        }
    }
}
