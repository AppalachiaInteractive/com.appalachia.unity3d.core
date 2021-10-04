#region

using System;
using Appalachia.Editing.Attributes;
using Appalachia.Editing.Preferences.Globals;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Labels
{
    [Serializable]
    public class LabelSearchTerm
    {
        [ToggleLeft]
        [LabelText("Use")]
        [SmartLabel]
        [HorizontalGroup("A", 20f /*, .1f*/)]
        [SerializeField]
        public bool enabled;

        [GUIColor(nameof(_labelColor))]
        [ValueDropdown(nameof(labelList))]
        [InlineProperty]
        [HideLabel]
        [LabelWidth(0)]
        [HorizontalGroup("A" /*, .1f*/)]
        [SerializeField]
        public string label;

        [HideInInspector]
        [NonSerialized]
        public bool found;

        private Color _labelColor =>
            enabled
                ? ColorPrefs.Instance.EnabledSubdued.v
                : ColorPrefs.Instance.DisabledImportantSubdued.v;

        private ValueDropdownList<string> labelList => LabelManager.labelDropdownList;

        public void Reset()
        {
            found = false;
        }
    }
}
