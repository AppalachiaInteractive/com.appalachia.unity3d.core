#region

using UnityEngine;

#endregion

namespace Appalachia.Core.Labels
{
    public static class LABELS
    {
        public const string LABEL_BASE_Tree = "Tree";
        public const string LABEL_BASE_Object = "Object";
        public const string LABEL_BASE_Assembly = "Assembly";
        public const string LABEL_BASE_Vegetation = "Vegetation";

        public const string LABEL_Grass = "Grass";
        public const string LABEL_Fern = "Fern";
        public const string LABEL_GroundCover = "GroundCover";
        public const string LABEL_Flowers = "Flowers";
        public const string LABEL_Plant = "Plant";
        public const string LABEL_Fungus = "Fungus";
        public const string LABEL_Crop = "Crop";
        public const string LABEL_Corn = "Corn";
        public const string LABEL_AquaticPlant = "AquaticPlant";
        public const string LABEL_Scatter = "Scatter";
        public const string LABEL_Boulder = "Boulder"; 
        public const string LABEL_Gravel = "Gravel"; 
        public const string LABEL_Stone = "Stone"; 
        public const string LABEL_Log = "Log"; 
        public const string LABEL_Branch = "Branch"; 
        public const string LABEL_Stump = "Stump"; 
        public const string LABEL_Trunk = "Trunk"; 
        public const string LABEL_Tree = "Tree"; 
        public const string LABEL_Leaves = "Leaves"; 
        public const string LABEL_Hillside = "Hillside"; 
        public const string LABEL_Ridge = "Ridge"; 
        public const string LABEL_Cliff = "Cliff"; 
        public const string LABEL_Roots = "Roots"; 


        public const string LABEL_VegetationVerySmall = "VegetationVerySmall";
        public const string LABEL_VegetationSmall = "VegetationSmall";
        public const string LABEL_VegetationMedium = "VegetationMedium";
        public const string LABEL_VegetationLarge = "VegetationLarge";
        public const string LABEL_VegetationVeryLarge = "VegetationVeryLarge";

        public const string LABEL_TreeSmall = "TreeSmall";
        public const string LABEL_TreeMedium = "TreeMedium";
        public const string LABEL_TreeLarge = "TreeLarge";

        public const string LABEL_ObjectVerySmall = "ObjectVerySmall";
        public const string LABEL_ObjectSmall = "ObjectSmall";
        public const string LABEL_ObjectMedium = "ObjectMedium";
        public const string LABEL_ObjectLarge = "ObjectLarge";
        public const string LABEL_ObjectHuge = "ObjectHuge";

        public const string LABEL_AssemblySmall = "AssemblySmall";
        public const string LABEL_AssemblyMedium = "AssemblyMedium";
        public const string LABEL_AssemblyLarge = "AssemblyLarge";
        public const string LABEL_AssemblyHuge = "AssemblyHuge";

        public static readonly LabelAssignmentCollection vegetations =
            new LabelAssignmentCollection(LABEL_BASE_Vegetation, 
                Vector3.up,
                new LabelAssignmentTerm(LABEL_VegetationVerySmall, 0.15f), 
                new LabelAssignmentTerm(LABEL_VegetationSmall, .3f), 
                new LabelAssignmentTerm(LABEL_VegetationMedium, 0.6f), 
                new LabelAssignmentTerm(LABEL_VegetationLarge, 1.0f),
                new LabelAssignmentTerm(LABEL_VegetationVeryLarge, 1024.0f)
            );

        public static readonly LabelAssignmentCollection trees = new LabelAssignmentCollection(
            LABEL_BASE_Tree, 
                Vector3.up,
            new LabelAssignmentTerm(LABEL_TreeSmall,  12.0f),
            new LabelAssignmentTerm(LABEL_TreeMedium, 24.0f),
            new LabelAssignmentTerm(LABEL_TreeLarge,  1024.0f)
        );

        public static readonly LabelAssignmentCollection objects = new LabelAssignmentCollection(
            LABEL_BASE_Object, 
            Vector3.one,
            new LabelAssignmentTerm(LABEL_ObjectVerySmall, 0.2f),
            new LabelAssignmentTerm(LABEL_ObjectSmall,     2.0f),
            new LabelAssignmentTerm(LABEL_ObjectMedium,    5.0f),
            new LabelAssignmentTerm(LABEL_ObjectLarge,     24.0f),
            new LabelAssignmentTerm(LABEL_ObjectHuge,      1024.0f)
        );

        public static readonly LabelAssignmentCollection assemblies = new LabelAssignmentCollection(
            LABEL_BASE_Assembly, 
            Vector3.one,
            new LabelAssignmentTerm(LABEL_AssemblySmall,  2.0f),
            new LabelAssignmentTerm(LABEL_AssemblyMedium, 5.0f),
            new LabelAssignmentTerm(LABEL_AssemblyLarge,  24.0f),
            new LabelAssignmentTerm(LABEL_AssemblyHuge,   1024.0f)
        );

    }
}
