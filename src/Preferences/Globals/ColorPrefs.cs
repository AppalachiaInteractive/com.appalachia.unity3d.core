#region

using Appalachia.Utility.Colors;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences.Globals
{
    [UnityEditor.InitializeOnLoad]
    public sealed class ColorPrefs
    {
        private const string _B = "Appalachia/Colors/Buoyancy/";
        private const string _BF = "Appalachia/Colors/Buoyancy/Force";

        private const string _G = "Appalachia/Colors";

        private const string _GRAD = _G + "/Gradients";
        private const string _GSC = _G + "/Status Codes";

        private const string _GSM = _G + "/Solo & Mute";

        private const string _GZ = _G + "/Gizmos";

        private const string _GZ_DC = _GZ + "/Decomposed Collider";

        private const string _GZ_MB = _GZ + "/Mesh Burial";

        private const string _GZPRI = _GZ + "/Prefab Rendering Instance";
        private const string _GZPRI_I = _GZPRI + "/Interaction";
        private const string _GZPRI_IC = _GZPRI + "/Instance Code";
        private const string _GZPRI_P = _GZPRI + "/Physics";
        private const string _GZPRI_R = _GZPRI + "/Rendering";

        private const string _O = "Appalachia/Octree";
        private const string _OB = _O + "/Bounds";
        private const string _OP = _O + "/Point";
        private const string _OS = _O + "/Sphere";
        private const string _OV = _O + "/Voxel";
        private const string _QUALITY = _GRAD + "/Quality";
        private const string _SELECT_DENSITY = _SELECTORS + "/Densities";
        private const string _SELECT_GENERIC = _SELECTORS + "/Generic";
        private const string _SELECT_PHYSICS = _SELECTORS + "/Physics Materials";
        private const string _SELECT_WOOD = _SELECTORS + "/Wood Simulation";

        private const string _SELECTORS = "Appalachia/Selectors";

        private static readonly Gradient _default_Gradient = new()
        {
            alphaKeys = new[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f)},
            colorKeys = new[]
            {
                new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.white, 1.0f)
            }
        };

        public static Color ButtonFade10 => Colors.Gray10;
        public static Color ButtonFade20 => Colors.Gray20;
        public static Color ButtonFade30 => Colors.Gray30;
        public static Color ButtonFade40 => Colors.Gray40;
        public static Color ButtonFade50 => Colors.Gray50;
        public static Color ButtonFade60 => Colors.Gray60;
        public static Color ButtonFade70 => Colors.Gray70;
        public static Color ButtonFade75 => Colors.Gray75;
        public static Color ButtonFade80 => Colors.Gray80;
        public static Color ButtonFade85 => Colors.Gray85;
        public static Color ButtonFade90 => Colors.Gray90;

        public static Color ButtonFade95 => Colors.Gray95;

        public static ColorPrefs Instance => _instance ?? (_instance = new ColorPrefs());

        static ColorPrefs()
        {
            _instance = new ColorPrefs();
        }

        private static ColorPrefs _instance;

        public PREF<Color> BackgroundInfo = PREFS.REG(_GSC, nameof(BackgroundInfo), Colors.Turquoise);

        public PREF<Color> Buoyancy_airResisForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_airResisForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_CenterOfMass = PREFS.REG(
            _B,
            nameof(Buoyancy_CenterOfMass),
            Colors.IndianRed
        );

        public PREF<Color> Buoyancy_CumulativeForce = PREFS.REG(
            _B,
            nameof(Buoyancy_CumulativeForce),
            Colors.DodgerBlue3
        );

        public PREF<Color> Buoyancy_CumulativeTorque = PREFS.REG(
            _B,
            nameof(Buoyancy_CumulativeTorque),
            Colors.CadmiumOrange
        );

        public PREF<Color> Buoyancy_CumulativeToSurface = PREFS.REG(
            _B,
            nameof(Buoyancy_CumulativeToSurface),
            Colors.WhiteSmokeGray96
        );

        public PREF<Color> Buoyancy_cumulatvForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_cumulatvForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_ForcePositions = PREFS.REG(
            _B,
            nameof(Buoyancy_ForcePositions),
            Colors.Cyan2
        );

        public PREF<Color> Buoyancy_hydrostaForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_hydrostaForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_presrDrgForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_presrDrgForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_slammingForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_slammingForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_SubmersionPositions = PREFS.REG(
            _B,
            nameof(Buoyancy_SubmersionPositions),
            Colors.Cyan2
        );

        public PREF<Color> Buoyancy_viscosWRForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_viscosWRForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_VoxelBounds = PREFS.REG(
            _B,
            nameof(Buoyancy_VoxelBounds),
            Colors.CadmiumYellow
        );

        public PREF<Color> Buoyancy_VoxelBoundsSubdivisions = PREFS.REG(
            _B,
            nameof(Buoyancy_VoxelBoundsSubdivisions),
            Colors.CadmiumOrange
        );

        public PREF<Color> Buoyancy_Voxels = PREFS.REG(_B, nameof(Buoyancy_Voxels), Colors.Yellow1);

        public PREF<Color> Buoyancy_WaterLines = PREFS.REG(
            _B,
            nameof(Buoyancy_WaterLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_waveDrftForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_waveDrftForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> Buoyancy_windResiForceLines = PREFS.REG(
            _BF,
            nameof(Buoyancy_windResiForceLines),
            Colors.PowderBlue
        );

        public PREF<Color> DecomposedColliderInBounds = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderInBounds),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderLimitationColors = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderLimitationColors),
            Colors.CadmiumYellow
        );

        public PREF<Color> DecomposedColliderMesh = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderMesh),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderReviewBasic = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderReviewBasic),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderReviewExternal = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderReviewExternal),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderReviewFunction = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderReviewFunction),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderReviewLocked = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderReviewLocked),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderReviewMissing = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderReviewMissing),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderReviewModification = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderReviewModification),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderReviewNotSet = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderReviewNotSet),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderSelected = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSelected),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderSelectedIndex = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSelectedIndex),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderSelectorAssign = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSelectorAssign),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderSelectorAssignAll = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSelectorAssignAll),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderSelectorModel = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSelectorModel),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderSelectorSwap = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSelectorSwap),
            Colors.Cyan
        );

        public PREF<Color> DecomposedColliderSuccessThreshold = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSuccessThreshold),
            Colors.ForestGreen
        );

        public PREF<Color> DensitySelectorButton = PREFS.REG(
            _SELECT_DENSITY,
            nameof(DensitySelectorButton),
            Colors.Cyan
        );

        public PREF<Color> Disabled = PREFS.REG(_GSM, nameof(Disabled), Colors.Gray50);

        public PREF<Color> DisabledImportant = PREFS.REG(
            _GSC,
            nameof(DisabledImportant),
            Colors.CadmiumYellow
        );

        public PREF<Color> DisabledImportantDisabled = PREFS.REG(
            _GSC,
            nameof(DisabledImportantDisabled),
            Colors.CadmiumYellow
        );

        public PREF<Color> DisabledImportantSubdued = PREFS.REG(
            _GSC,
            nameof(DisabledImportantSubdued),
            Colors.CadmiumYellow.UpdateV(1.1f).UpdateS(.5f)
        );

        public PREF<Color> Enabled = PREFS.REG(_GSC, nameof(Enabled), Colors.ForestGreen);

        public PREF<Color> EnabledDisabled = PREFS.REG(_GSC, nameof(EnabledDisabled), Colors.ForestGreen);

        public PREF<Color> EnabledDisabledElsewhere = PREFS.REG(
            _GSC,
            nameof(EnabledDisabledElsewhere),
            Colors.PaleGreen
        );

        public PREF<Color> EnabledSubdued = PREFS.REG(
            _GSC,
            nameof(EnabledSubdued),
            Colors.ForestGreen.UpdateV(1.1f).UpdateS(.75f)
        );

        public PREF<Color> GenericSelectorButton = PREFS.REG(
            _SELECT_GENERIC,
            nameof(GenericSelectorButton),
            Colors.LightSteelBlue1
        );

        public PREF<Color> MeshBurialBounds = PREFS.REG(_GZ_MB, nameof(MeshBurialBounds), Colors.Cyan);

        public PREF<Color> MuteAny = PREFS.REG(_GSM, nameof(MuteAny), Colors.IndianRed1);

        public PREF<Color> MuteDisabled = PREFS.REG(
            _GSM,
            nameof(MuteDisabled),
            Colors.IndianRed1.UpdateS(.1f)
        );

        public PREF<Color> MuteEnabled = PREFS.REG(_GSM, nameof(MuteEnabled), Colors.IndianRed4);

        public PREF<Color> Octree_Voxel_BoundsColor = PREFS.REG(
            _OV,
            nameof(Octree_Voxel_BoundsColor),
            Colors.Cyan
        );

        public PREF<Color> Octree_Voxel_NodeColor = PREFS.REG(
            _OV,
            nameof(Octree_Voxel_NodeColor),
            Colors.Cyan
        );

        public PREF<Color> Pending = PREFS.REG(_GSC, nameof(Pending), Colors.DodgerBlue1);

        public PREF<Color> PhysicMaterialSelectorButton = PREFS.REG(
            _SELECT_PHYSICS,
            nameof(PhysicMaterialSelectorButton),
            Colors.Cyan
        );

        public PREF<Color> PRI_INT_Disabled = PREFS.REG(_GZPRI_I, nameof(PRI_INT_Disabled), Colors.Cyan);

        public PREF<Color> PRI_INT_Enabled = PREFS.REG(_GZPRI_I, nameof(PRI_INT_Enabled), Colors.Cyan);

        public PREF<Color> PRI_INT_NotSet = PREFS.REG(_GZPRI_I, nameof(PRI_INT_NotSet), Colors.Cyan);

        public PREF<Color> PRI_PHYS_Disabled = PREFS.REG(_GZPRI_P, nameof(PRI_PHYS_Disabled), Colors.Cyan);

        public PREF<Color> PRI_PHYS_Enabled = PREFS.REG(_GZPRI_P, nameof(PRI_PHYS_Enabled), Colors.Cyan);

        public PREF<Color> PRI_PHYS_NotSet = PREFS.REG(_GZPRI_P, nameof(PRI_PHYS_NotSet), Colors.Cyan);

        public PREF<Color> PRI_REND_Disabled = PREFS.REG(_GZPRI_R, nameof(PRI_REND_Disabled), Colors.Cyan);

        public PREF<Color> PRI_REND_GPU = PREFS.REG(_GZPRI_R,  nameof(PRI_REND_GPU),  Colors.Cyan);
        public PREF<Color> PRI_REND_Mesh = PREFS.REG(_GZPRI_R, nameof(PRI_REND_Mesh), Colors.Cyan);

        public PREF<Color> PRI_REND_NotSet = PREFS.REG(_GZPRI_R, nameof(PRI_REND_NotSet), Colors.Cyan);

        public PREF<Color> PRIIC_Delayed = PREFS.REG(_GZPRI_IC, nameof(PRIIC_Delayed), Colors.Cyan);

        public PREF<Color> PRIIC_ForceDisabled = PREFS.REG(
            _GZPRI_IC,
            nameof(PRIIC_ForceDisabled),
            Colors.Cyan
        );

        public PREF<Color> PRIIC_Gizmos = PREFS.REG(_GZPRI_IC, nameof(PRIIC_Gizmos), Colors.Cyan);
        public PREF<Color> PRIIC_Normal = PREFS.REG(_GZPRI_IC, nameof(PRIIC_Normal), Colors.Cyan);
        public PREF<Color> PRIIC_NotSet = PREFS.REG(_GZPRI_IC, nameof(PRIIC_NotSet), Colors.Cyan);

        public PREF<Color> PRIIC_OutsideOfMaximumChangeRadius = PREFS.REG(
            _GZPRI_IC,
            nameof(PRIIC_OutsideOfMaximumChangeRadius),
            Colors.Cyan
        );

        public PREF<Color> SoloAny = PREFS.REG(_GSM, nameof(SoloAny), Colors.DarkSeaGreen1);

        public PREF<Color> SoloDisabled = PREFS.REG(
            _GSM,
            nameof(SoloDisabled),
            Colors.DarkSeaGreen1.UpdateS(.1f)
        );

        public PREF<Color> SoloEnabled = PREFS.REG(_GSM, nameof(SoloEnabled), Colors.DarkSeaGreen4);

        public PREF<Color> SubdivisionBounds = PREFS.REG(_GZ_DC, nameof(SubdivisionBounds), Colors.Cyan);

        public PREF<Color> WoodSimulationDataSelectorButton = PREFS.REG(
            _SELECT_WOOD,
            nameof(WoodSimulationDataSelectorButton),
            Colors.Cyan
        );

        public PREF<float> DecomposedColliderAlpha = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderAlpha),
            1.0f,
            0.0f,
            1.0f
        );

        public PREF<float> DecomposedColliderInBoundsScale = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderInBoundsScale),
            .96f
        );

        public PREF<float> DecomposedColliderLabelBackgroundAlpha = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderLabelBackgroundAlpha),
            1.0f,
            0.0f,
            1.0f
        );

        public PREF<float> DecomposedColliderLabelForegroundAlpha = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderLabelForegroundAlpha),
            1.0f,
            0.0f,
            1.0f
        );

        public PREF<float> DecomposedColliderSelectedScale = PREFS.REG(
            _GZ_DC,
            nameof(DecomposedColliderSelectedScale),
            .98f
        );

        public PREF<float> DensitySelectorColorDrop = PREFS.REG(
            _SELECT_DENSITY,
            nameof(DensitySelectorColorDrop),
            .96f
        );

        public PREF<float> GenericSelectorColorDrop = PREFS.REG(
            _SELECT_GENERIC,
            nameof(GenericSelectorColorDrop),
            .96f
        );

        public PREF<float> Octree_Voxel_NodeScale = PREFS.REG(
            _OV,
            nameof(Octree_Voxel_NodeScale),
            .8f,
            .01f,
            1.1f
        );

        public PREF<float> PhysicMaterialSelectorColorDrop = PREFS.REG(
            _SELECT_PHYSICS,
            nameof(PhysicMaterialSelectorColorDrop),
            .96f
        );

        public PREF<float> WoodSimulationDataSelectorColorDrop = PREFS.REG(
            _SELECT_WOOD,
            nameof(WoodSimulationDataSelectorColorDrop),
            .96f
        );

        public PREF<Gradient> Quality_BadToGood = PREFS.REG(
            _QUALITY,
            nameof(Quality_BadToGood),
            _default_Gradient
        );

        public PREF<Gradient> Quality_NeutralToBad = PREFS.REG(
            _QUALITY,
            nameof(Quality_NeutralToBad),
            _default_Gradient
        );

        public PREF<Gradient> Quality_NeutralToGood = PREFS.REG(
            _QUALITY,
            nameof(Quality_NeutralToGood),
            _default_Gradient
        );
    }
}
