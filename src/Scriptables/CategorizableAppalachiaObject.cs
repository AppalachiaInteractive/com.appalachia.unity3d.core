using System;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class CategorizableAppalachiaObject<T> : AppalachiaObject<T>, ICategorizable
        where T : CategorizableAppalachiaObject<T>
    {
        [FoldoutGroup("Metadata", false)]
        [FormerlySerializedAs("category")]
        [SerializeField]
        [SmartLabel]
#if UNITY_EDITOR
        [SmartInlineButton(nameof(Prefix),              "Prefix", false, false, null, nameof(_disablePrefix))]
        [SmartInlineButton(nameof(SelectUncategorized), "Select Uncat.", false)]
        [SmartInlineButton(
            nameof(SelectCategory),
            "Select Category",
            false,
            false,
            null,
            nameof(_disableSelectCategory)
        )]
#endif
#pragma warning disable 0649
        private string _category;
#pragma warning restore 0649

        public string Category => _category;
        public string Category_ => $"{Category}_";

#if UNITY_EDITOR
        private bool _disableSelectCategory => string.IsNullOrWhiteSpace(Category);

        private void SelectCategory()
        {
            Selection.objects = GetAllOfType(i => i.Category == Category).ToArray();
        }

        private void SelectUncategorized()
        {
            Selection.objects = GetAllOfType(i => string.IsNullOrWhiteSpace(i.Category)).ToArray();
        }

        private bool _disablePrefix =>
            (Category == null) || name.StartsWith(Category_, StringComparison.OrdinalIgnoreCase);

        private void Prefix()
        {
            if (!name.StartsWith(Category_, StringComparison.OrdinalIgnoreCase))
            {
                Rename($"{Category_.ToLower()}{name}");
            }
        }
#endif
    }
}
