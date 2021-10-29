using System.Diagnostics;

namespace Appalachia.Core.Preferences
{
    [DebuggerStepThrough]
    public abstract class PREF_BASE
    {
        protected PREF_BASE(string key, string grouping, string label, int order, string header)
        {
            _key = key;
            _grouping = grouping;
            _label = label;
            _order = order;
            _niceLabel = UnityEditor.ObjectNames.NicifyVariableName(label);
        }

        protected readonly int _order;
        protected readonly string _grouping;
        protected readonly string _key;
        protected readonly string _label;

        protected bool _isAwake;
        protected bool _reset;
        protected string _niceLabel;

        public bool IsAwake => _isAwake;

        public string NiceLabel
        {
            get => _niceLabel;
            set => _niceLabel = value;
        }

        internal int Order => _order;
        internal string Grouping => _grouping;
        internal string Key => _key;
        internal string Label => _label;
    }
}
