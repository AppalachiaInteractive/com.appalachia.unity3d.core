using Appalachia.Core.Objects.Root;
using UnityEngine;

namespace Appalachia.Core.Transitions
{
    /// <summary>This is the base class for all transition methods.</summary>
    public abstract class AppaMethod : AppalachiaBehaviour<AppaMethod>
    {
        public abstract void Register();

        /// <summary>This will take the input linear 0..1 value, and return a transformed version based on the specified easing function.</summary>
        public static float Smooth(AppaEase ease, float x)
        {
            switch (ease)
            {
                case AppaEase.Smooth:
                {
                    x = x * x * (3.0f - (2.0f * x));
                }
                    break;

                case AppaEase.Accelerate:
                {
                    x *= x;
                }
                    break;

                case AppaEase.Decelerate:
                {
                    x = 1.0f - x;
                    x *= x;
                    x = 1.0f - x;
                }
                    break;

                case AppaEase.Elastic:
                {
                    var angle = x * Mathf.PI * 4.0f;
                    var weightA = 1.0f - Mathf.Pow(x,        0.125f);
                    var weightB = 1.0f - Mathf.Pow(1.0f - x, 8.0f);

                    x = Mathf.LerpUnclamped(0.0f, 1.0f - (Mathf.Cos(angle) * weightA), weightB);
                }
                    break;

                case AppaEase.Back:
                {
                    x = 1.0f - x;
                    x = (x * x * x) - (x * Mathf.Sin(x * Mathf.PI));
                    x = 1.0f - x;
                }
                    break;

                case AppaEase.Bounce:
                {
                    if (x < (4f / 11f))
                    {
                        x = (121f / 16f) * x * x;
                    }
                    else if (x < (8f / 11f))
                    {
                        x = ((121f / 16f) * (x - (6f / 11f)) * (x - (6f / 11f))) + 0.75f;
                    }
                    else if (x < (10f / 11f))
                    {
                        x = ((121f / 16f) * (x - (9f / 11f)) * (x - (9f / 11f))) + (15f / 16f);
                    }
                    else
                    {
                        x = ((121f / 16f) * (x - (21f / 22f)) * (x - (21f / 22f))) + (63f / 64f);
                    }
                }
                    break;

                case AppaEase.SineIn:
                    return 1 - Mathf.Cos((x * Mathf.PI) / 2.0f);

                case AppaEase.SineOut:
                    return Mathf.Sin((x * Mathf.PI) / 2.0f);

                case AppaEase.SineInOut:
                    return -(Mathf.Cos(Mathf.PI * x) - 1.0f) / 2.0f;

                case AppaEase.QuadIn:
                    return SmoothQuad(x);

                case AppaEase.QuadOut:
                    return 1 - SmoothQuad(1 - x);

                case AppaEase.QuadInOut:
                    return x < 0.5f ? SmoothQuad(x * 2) / 2 : 1 - (SmoothQuad(2 - (x * 2)) / 2);

                case AppaEase.CubicIn:
                    return SmoothCubic(x);

                case AppaEase.CubicOut:
                    return 1 - SmoothCubic(1 - x);

                case AppaEase.CubicInOut:
                    return x < 0.5f ? SmoothCubic(x * 2) / 2 : 1 - (SmoothCubic(2 - (x * 2)) / 2);

                case AppaEase.QuartIn:
                    return SmoothQuart(x);

                case AppaEase.QuartOut:
                    return 1 - SmoothQuart(1 - x);

                case AppaEase.QuartInOut:
                    return x < 0.5f ? SmoothQuart(x * 2) / 2 : 1 - (SmoothQuart(2 - (x * 2)) / 2);

                case AppaEase.QuintIn:
                    return SmoothQuint(x);

                case AppaEase.QuintOut:
                    return 1 - SmoothQuint(1 - x);

                case AppaEase.QuintInOut:
                    return x < 0.5f ? SmoothQuint(x * 2) / 2 : 1 - (SmoothQuint(2 - (x * 2)) / 2);

                case AppaEase.ExpoIn:
                    return SmoothExpo(x);

                case AppaEase.ExpoOut:
                    return 1 - SmoothExpo(1 - x);

                case AppaEase.ExpoInOut:
                    return x < 0.5f ? SmoothExpo(x * 2) / 2 : 1 - (SmoothExpo(2 - (x * 2)) / 2);

                case AppaEase.CircIn:
                    return SmoothCirc(x);

                case AppaEase.CircOut:
                    return 1 - SmoothCirc(1 - x);

                case AppaEase.CircInOut:
                    return x < 0.5f ? SmoothCirc(x * 2) / 2 : 1 - (SmoothCirc(2 - (x * 2)) / 2);

                case AppaEase.BackIn:
                    return SmoothBack(x);

                case AppaEase.BackOut:
                    return 1 - SmoothBack(1 - x);

                case AppaEase.BackInOut:
                    return x < 0.5f ? SmoothBack(x * 2) / 2 : 1 - (SmoothBack(2 - (x * 2)) / 2);

                case AppaEase.ElasticIn:
                    return SmoothElastic(x);

                case AppaEase.ElasticOut:
                    return 1 - SmoothElastic(1 - x);

                case AppaEase.ElasticInOut:
                    return x < 0.5f ? SmoothElastic(x * 2) / 2 : 1 - (SmoothElastic(2 - (x * 2)) / 2);

                case AppaEase.BounceIn:
                    return 1 - SmoothBounce(1 - x);

                case AppaEase.BounceOut:
                    return SmoothBounce(x);

                case AppaEase.BounceInOut:
                    return x < 0.5f
                        ? 0.5f - (SmoothBounce(1 - (x * 2)) / 2)
                        : 0.5f + (SmoothBounce((x * 2) - 1) / 2);
            }

            return x;
        }

        [ContextMenu("Begin All Transitions")]
        public void BeginAllTransitions()
        {
            AppaTransition.CurrentAliases.Clear();

            AppaTransition.BeginAllTransitions(transform);
        }

        [ContextMenu("Begin This Transition")]
        public void BeginThisTransition()
        {
            AppaTransition.RequireSubmitted();

            AppaTransition.CurrentAliases.Clear();

            Register();

            AppaTransition.Submit();
        }

        private static float SmoothBack(float x)
        {
            return (2.70158f * x * x * x) - (1.70158f * x * x);
        }

        private static float SmoothBounce(float x)
        {
            if (x < (4f / 11f))
            {
                return (121f / 16f) * x * x;
            }

            if (x < (8f / 11f))
            {
                return ((121f / 16f) * (x - (6f / 11f)) * (x - (6f / 11f))) + 0.75f;
            }

            if (x < (10f / 11f))
            {
                return ((121f / 16f) * (x - (9f / 11f)) * (x - (9f / 11f))) + (15f / 16f);
            }

            return ((121f / 16f) * (x - (21f / 22f)) * (x - (21f / 22f))) + (63f / 64f);
        }

        private static float SmoothCirc(float x)
        {
            return 1.0f - Mathf.Sqrt(1.0f - Mathf.Pow(x, 2.0f));
        }

        private static float SmoothCubic(float x)
        {
            return x * x * x;
        }

        private static float SmoothElastic(float x)
        {
            return x == 0.0f
                ? 0.0f
                : x == 1.0f
                    ? 1.0f
                    : -Mathf.Pow(2.0f, (10.0f * x) - 10.0f) *
                      Mathf.Sin(((x * 10.0f) - 10.75f) * ((2.0f * Mathf.PI) / 3.0f));
        }

        private static float SmoothExpo(float x)
        {
            return x == 0.0f ? 0.0f : Mathf.Pow(2.0f, (10.0f * x) - 10.0f);
        }

        private static float SmoothQuad(float x)
        {
            return x * x;
        }

        private static float SmoothQuart(float x)
        {
            return x * x * x * x;
        }

        private static float SmoothQuint(float x)
        {
            return x * x * x * x * x;
        }
    }
}
