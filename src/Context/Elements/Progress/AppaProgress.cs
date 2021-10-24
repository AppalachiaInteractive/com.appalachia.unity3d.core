using System;

namespace Appalachia.Core.Context.Elements.Progress
{
    public struct AppaProgress : IEquatable<AppaProgress>, IComparable<AppaProgress>, IComparable
    {
        public const string ANALYZING = "Analyzing";
        public const string CHECKING = "Checking";
        public const string FINDING = "Finding";
        public const string INITIALIZING = "Initializing";
        public const string REGISTERING = "Registering";
        public const string RESETTING = "Resetting";
        public const string RETRIEVING = "Retrieving";
        public const string REVIEWING = "Reviewing";
        public const string SETTING = "Setting";
        public const string SORTING = "Sorting";
        public const string VALIDATING = "Validating";
        public const string WAITING = "Waiting";

        private AppaProgress(float progress)
        {
            message = null;
            this.progress = progress;
        }

        private AppaProgress(string message)
        {
            this.message = message;
            progress = -1f;
        }

        private AppaProgress(string message, float progress)
        {
            this.message = message;
            this.progress = progress;
        }

        public float progress;

        public string message;

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            return obj is AppaProgress other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(AppaProgress)}");
        }

        public int CompareTo(AppaProgress other)
        {
            return progress.CompareTo(other.progress);
        }

        public bool Equals(AppaProgress other)
        {
            return (message == other.message) && progress.Equals(other.progress);
        }

        public override bool Equals(object obj)
        {
            return obj is AppaProgress other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(message, progress);
        }

        public static implicit operator (string, float)(AppaProgress o)
        {
            return (o.message, o.progress);
        }

        public static implicit operator AppaProgress(string o)
        {
            return new(o);
        }

        public static implicit operator AppaProgress(float o)
        {
            return new(o);
        }

        public static implicit operator AppaProgress(Tuple<string, float> o)
        {
            return new(o.Item1, o.Item2);
        }

        public static implicit operator AppaProgress((string, float) o)
        {
            return new(o.Item1, o.Item2);
        }

        public static bool operator ==(AppaProgress left, AppaProgress right)
        {
            return left.Equals(right);
        }

        public static bool operator >(AppaProgress left, AppaProgress right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(AppaProgress left, AppaProgress right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator !=(AppaProgress left, AppaProgress right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(AppaProgress left, AppaProgress right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(AppaProgress left, AppaProgress right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static implicit operator float(AppaProgress o)
        {
            return o.progress;
        }

        public static implicit operator string(AppaProgress o)
        {
            return o.message ?? $"{o.progress:N3}";
        }

        public static implicit operator Tuple<string, float>(AppaProgress o)
        {
            return Tuple.Create(o.message, o.progress);
        }
    }
}
