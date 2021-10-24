namespace Appalachia.Core.Extensions
{
    public static class Angles
    {
        public static float ToRelative(float a)
        {
            if (a >= 180f)
            {
                a -= 360f;
            }

            return a;
        }

        public static float Unwind(float a)
        {
            while (a > 360f)
            {
                a -= 360f;
            }

            while (a < 0f)
            {
                a += 360f;
            }

            return a;
        }
    }
} // namespace Appalachia.Core.Runtime.Gameplay.Utilities
