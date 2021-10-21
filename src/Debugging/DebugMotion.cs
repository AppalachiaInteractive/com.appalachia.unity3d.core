namespace Appalachia.Core.Debugging
{
    public enum DebugMotion
    {
        //Off = -1,

        PrimaryRoll = 10,  // VERTEX R
        PrimaryPivot = 11, // UV2 XYZ
        PrimaryBend = 12,  // UV2 W

        SecondaryRoll = 20,      // VERTEX G
        SecondaryPivot = 21,     // UV4 XYZ
        SecondaryBend = 22,      // UV4 W
        SecondaryDirection = 23, // UV3 XYZ

        TertiaryRoll = 30, // VERTEX B

        TypeDesignator = 40, // UV3 W

        Facing = 80,

        GustStrength_Object = 90,
        GustStrength_Vertex = 91,
        GustDirectionality = 92,

        AmbientOcclusion = 97, // UV0 Z
        Phase = 98,            // UV0 W
        Variation = 99         // VERTEX A

        // no more

        /*
        GlobalTint = 31,
        GlobalSize = 32,

        MaterialLighting = 61,
        MaterialInstancing = 62,
        */

        //shaderType = 63,
        //MaterialIssues = 64,
    }
}
