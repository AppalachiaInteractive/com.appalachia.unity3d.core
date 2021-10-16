
namespace Appalachia.Core.Preferences
{
    public abstract class PREF_BASE
    {
        protected PREF_BASE(string key, string grouping, string label, int order)
        {
            _key = key;
            _grouping = grouping;
            _label = label;
            _order = order;
            _niceLabel = UnityEditor.ObjectNames.NicifyVariableName(label);
        }
        
        protected bool _isAwake;
        protected readonly string _key;
        protected readonly string _grouping;
        protected readonly string _label;
        protected string _niceLabel;
        protected readonly int _order;
        protected bool _reset;
        
        public bool IsAwake => _isAwake;
        internal string Key => _key;
        internal string Grouping => _grouping;
        internal string Label => _label;

        public string NiceLabel
        {
            get => _niceLabel;
            set => _niceLabel = value;
        }

        internal int Order => _order;

    }
}
