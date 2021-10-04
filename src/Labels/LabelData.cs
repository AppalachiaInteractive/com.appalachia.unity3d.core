#region

using System;
using Appalachia.Editing.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Labels
{
    [Serializable]
    [InlineProperty]
    [HideLabel]
    [LabelWidth(0)]
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    public class LabelData
    {
        [ReadOnly]
        [SmartLabel]
        [SuffixLabel("$" + nameof(suffixLabel))]
        public string label;

        [HideInInspector] public int count;

        [HorizontalGroup("B", .25f)]
        [LabelText("Delete")]
        [SmartLabel(Postfix = true)]
        [OnValueChanged(nameof(OnDeleteChanged))]
        [DisableIf(nameof(_disableDelete))]
        public bool deleteLabel;

        [HorizontalGroup("B", .25f)]
        [LabelText("Apply")]
        [SmartLabel(Postfix = true)]
        [OnValueChanged(nameof(OnApplyChanged))]
        [DisableIf(nameof(_disableApply))]
        public bool applyLabel;

        [HorizontalGroup("B", .5f)]
        [LabelText("Swap")]
        [SmartLabel]
        [OnValueChanged(nameof(OnSwitchToChanged))]
        [DisableIf(nameof(_disableSwitchTo))]
        public string switchTo;

        private string suffixLabel => $"  {count} items";

        private bool _disableDelete => applyLabel || (switchTo != null);

        private bool _disableApply => deleteLabel || (switchTo != null);

        private bool _disableSwitchTo => deleteLabel || applyLabel;

        private void OnDeleteChanged()
        {
            if (deleteLabel)
            {
                applyLabel = false;
                switchTo = null;
            }
        }

        private void OnApplyChanged()
        {
            if (applyLabel)
            {
                deleteLabel = false;
                switchTo = null;
            }
        }

        private void OnSwitchToChanged()
        {
            if (switchTo != null)
            {
                deleteLabel = false;
                applyLabel = false;
            }
        }
    }
}
