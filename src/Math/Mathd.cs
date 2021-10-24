#region

using System;
using Appalachia.Core.Timing;

#endregion

namespace Appalachia.Core.Math
{
    public static class Mathd
    {
        // Degrees-to-radians conversion constant (RO).
        public const double Deg2Rad = (PI * 2F) / 360F;

        // A representation of positive infinity (RO).
        public const double Infinity = float.PositiveInfinity;

        // A representation of negative infinity (RO).
        public const double NegativeInfinity = float.NegativeInfinity;

        // The infamous ''3.14159265358979...'' value (RO).
        public const double PI = System.Math.PI;

        // Radians-to-degrees conversion constant (RO).
        public const double Rad2Deg = 1F / Deg2Rad;

        // Compares two doubleing point values if they are similar.
        public static bool Approximately(double a, double b)
        {
            // If a or b is zero, compare that the other is less or equal to epsilon.
            // If neither a or b are 0, then find an epsilon that is good for
            // comparing numbers at the maximum magnitude of a and b.
            // Floating points have about 7 significant digits, so
            // 1.000001f can be represented while 1.0000001f is rounded to zero,
            // thus we could use an epsilon of 0.000001f for comparing values close to 1.
            // We multiply this epsilon by the biggest magnitude of a and b.
            return Abs(b - a) < Max(0.000001f * Max(Abs(a), Abs(b)), double.Epsilon * 8);
        }

        // Returns the absolute value of /f/.
        public static double Abs(double d)
        {
            return System.Math.Abs(d);
        }

        // Returns the arc-cosine of /f/ - the angle in radians whose cosine is /f/.
        public static double Acos(double d)
        {
            return System.Math.Acos(d);
        }

        // Returns the arc-sine of /f/ - the angle in radians whose sine is /f/.
        public static double Asin(double d)
        {
            return System.Math.Asin(d);
        }

        // Returns the arc-tangent of /f/ - the angle in radians whose tangent is /f/.
        public static double Atan(double d)
        {
            return System.Math.Atan(d);
        }

        // Returns the angle in radians whose ::ref::Tan is @@y/x@@.
        public static double Atan2(double y, double x)
        {
            return System.Math.Atan2(y, x);
        }

        // Returns the smallest integer greater to or equal to /f/.
        public static double Ceil(double d)
        {
            return System.Math.Ceiling(d);
        }

        // Clamps a value between a minimum double and maximum double value.
        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        // Clamps value between 0 and 1 and returns value
        public static double Clamp01(double value)
        {
            if (value < 0F)
            {
                return 0F;
            }

            if (value > 1F)
            {
                return 1F;
            }

            return value;
        }

        // Returns the cosine of angle /f/ in radians.
        public static double Cos(double d)
        {
            return System.Math.Cos(d);
        }

        // Calculates the shortest difference between two given angles.
        public static double DeltaAngle(double current, double target)
        {
            var delta = Repeat(target - current, 360.0F);
            if (delta > 180.0F)
            {
                delta -= 360.0F;
            }

            return delta;
        }

        // Returns e raised to the specified power.
        public static double Exp(double power)
        {
            return System.Math.Exp(power);
        }

        // Returns the largest integer smaller to or equal to /f/.
        public static double Floor(double d)
        {
            return System.Math.Floor(d);
        }

        //*undocumented
        public static double Gamma(double value, double absmax, double gamma)
        {
            var negative = false;
            if (value < 0F)
            {
                negative = true;
            }

            var absval = Abs(value);
            if (absval > absmax)
            {
                return negative ? -absval : absval;
            }

            var result = Pow(absval / absmax, gamma) * absmax;
            return negative ? -result : result;
        }

        // Calculates the ::ref::Lerp parameter between of two values.
        public static double InverseLerp(double a, double b, double value)
        {
            if (System.Math.Abs(a - b) > float.Epsilon)
            {
                return Clamp01((value - a) / (b - a));
            }

            return 0.0f;
        }

        // Interpolates between /a/ and /b/ by /t/. /t/ is clamped between 0 and 1.
        public static double Lerp(double a, double b, double t)
        {
            return a + ((b - a) * Clamp01(t));
        }

        // Same as ::ref::Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
        public static double LerpAngle(double a, double b, double t)
        {
            var delta = Repeat(b - a, 360);
            if (delta > 180)
            {
                delta -= 360;
            }

            return a + (delta * Clamp01(t));
        }

        // Interpolates between /a/ and /b/ by /t/ without clamping the interpolant.
        public static double LerpUnclamped(double a, double b, double t)
        {
            return a + ((b - a) * t);
        }

        // Returns the logarithm of a specified number in a specified base.
        public static double Log(double d, double p)
        {
            return System.Math.Log(d, p);
        }

        // Returns the natural (base e) logarithm of a specified number.
        public static double Log(double d)
        {
            return System.Math.Log(d);
        }

        // Returns the base 10 logarithm of a specified number.
        public static double Log10(double d)
        {
            return System.Math.Log10(d);
        }

        /// *listonly*
        public static double Max(double a, double b)
        {
            return a > b ? a : b;
        }

        // Returns largest of two or more values.
        public static double Max(params double[] values)
        {
            var len = values.Length;
            if (len == 0)
            {
                return 0;
            }

            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] > m)
                {
                    m = values[i];
                }
            }

            return m;
        }

        /// *listonly*
        public static double Min(double a, double b)
        {
            return a < b ? a : b;
        }

        // Returns the smallest of two or more values.
        public static double Min(params double[] values)
        {
            var len = values.Length;
            if (len == 0)
            {
                return 0;
            }

            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] < m)
                {
                    m = values[i];
                }
            }

            return m;
        }

        // Moves a value /current/ towards /target/.
        public static double MoveTowards(double current, double target, double maxDelta)
        {
            if (Abs(target - current) <= maxDelta)
            {
                return target;
            }

            return current + (Sign(target - current) * maxDelta);
        }

        // Same as ::ref::MoveTowards but makes sure the values interpolate correctly when they wrap around 360 degrees.
        public static double MoveTowardsAngle(double current, double target, double maxDelta)
        {
            var deltaAngle = DeltaAngle(current, target);
            if ((-maxDelta < deltaAngle) && (deltaAngle < maxDelta))
            {
                return target;
            }

            target = current + deltaAngle;
            return MoveTowards(current, target, maxDelta);
        }

        // PingPongs the value t, so that it is never larger than length and never smaller than 0.
        public static double PingPong(double t, double length)
        {
            t = Repeat(t, length * 2F);
            return length - Abs(t - length);
        }

        // Returns /f/ raised to power /p/.
        public static double Pow(double d, double p)
        {
            return System.Math.Pow(d, p);
        }

        // Loops the value t, so that it is never larger than length and never smaller than 0.
        public static double Repeat(double t, double length)
        {
            return Clamp(t - (Floor(t / length) * length), 0.0f, length);
        }

        // Returns /f/ rounded to the nearest integer.
        public static double Round(double d)
        {
            return System.Math.Round(d);
        }

        // Returns the sign of /f/.
        public static double Sign(double d)
        {
            return d >= 0F ? 1F : -1F;
        }

        // Returns the sine of angle /f/ in radians.
        public static double Sin(double d)
        {
            return System.Math.Sin(d);
        }

        public static double SmoothDamp(
            double current,
            double target,
            ref double currentVelocity,
            double smoothTime,
            double maxSpeed)
        {
            var deltaTime = CoreClock.VisualDelta;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static double SmoothDamp(
            double current,
            double target,
            ref double currentVelocity,
            double smoothTime)
        {
            var deltaTime = CoreClock.VisualDelta;
            var maxSpeed = Infinity;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        // Gradually changes a value towards a desired goal over time.
        public static double SmoothDamp(
            double current,
            double target,
            ref double currentVelocity,
            double smoothTime,
            double maxSpeed,
            double deltaTime)
        {
            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = Max(0.0001F, smoothTime);
            var omega = 2F / smoothTime;

            var x = omega * deltaTime;
            var exp = 1F / (1F + x + (0.48F * x * x) + (0.235F * x * x * x));
            var change = current - target;
            var originalTo = target;

            // Clamp maximum speed
            var maxChange = maxSpeed * smoothTime;
            change = Clamp(change, -maxChange, maxChange);
            target = current - change;

            var temp = (currentVelocity + (omega * change)) * deltaTime;
            currentVelocity = (currentVelocity - (omega * temp)) * exp;
            var output = target + ((change + temp) * exp);

            // Prevent overshooting
            if (((originalTo - current) > 0.0F) == (output > originalTo))
            {
                output = originalTo;
                currentVelocity = (output - originalTo) / deltaTime;
            }

            return output;
        }

        public static double SmoothDampAngle(
            double current,
            double target,
            ref double currentVelocity,
            double smoothTime,
            double maxSpeed)
        {
            var deltaTime = CoreClock.VisualDelta;
            return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static double SmoothDampAngle(
            double current,
            double target,
            ref double currentVelocity,
            double smoothTime)
        {
            var deltaTime = CoreClock.VisualDelta;
            var maxSpeed = Infinity;
            return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        // Gradually changes an angle given in degrees towards a desired goal angle over time.
        public static double SmoothDampAngle(
            double current,
            double target,
            ref double currentVelocity,
            double smoothTime,
            double maxSpeed,
            double deltaTime)
        {
            target = current + DeltaAngle(current, target);
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        // Interpolates between /min/ and /max/ with smoothing at the limits.
        public static double SmoothStep(double from, double to, double t)
        {
            t = Clamp01(t);
            t = (-2.0F * t * t * t) + (3.0F * t * t);
            return (to * t) + (from * (1F - t));
        }

        // Returns square root of /f/.
        public static double Sqrt(double d)
        {
            return System.Math.Sqrt(d);
        }

        // Returns the tangent of angle /f/ in radians.
        public static double Tan(double d)
        {
            return System.Math.Tan(d);
        }

        // Returns the absolute value of /value/.
        public static int Abs(int value)
        {
            return System.Math.Abs(value);
        }

        // Returns the smallest integer greater to or equal to /f/.
        public static int CeilToInt(double d)
        {
            return (int) System.Math.Ceiling(d);
        }

        // Clamps value between min and max and returns value.
        // Set the position of the transform to be that of the time
        // but never less than 1 or more than 3
        //
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        // Returns the largest integer smaller to or equal to /f/.
        public static int FloorToInt(double d)
        {
            return (int) System.Math.Floor(d);
        }

        /// *listonly*
        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        // Returns the largest of two or more values.
        public static int Max(params int[] values)
        {
            var len = values.Length;
            if (len == 0)
            {
                return 0;
            }

            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] > m)
                {
                    m = values[i];
                }
            }

            return m;
        }

        /// *listonly*
        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        // Returns the smallest of two or more values.
        public static int Min(params int[] values)
        {
            var len = values.Length;
            if (len == 0)
            {
                return 0;
            }

            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] < m)
                {
                    m = values[i];
                }
            }

            return m;
        }

        // Returns /f/ rounded to the nearest integer.
        public static int RoundToInt(double d)
        {
            return (int) System.Math.Round(d);
        }

        internal static long RandomToLong(Random r)
        {
            var buffer = new byte[8];
            r.NextBytes(buffer);
            return (long) (BitConverter.ToUInt64(buffer, 0) & long.MaxValue);
        }
    }
}
