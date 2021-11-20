using System;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Core.Scriptables
{
    [Serializable]
    public abstract class
        CategorizableAutonamedIdentifiableAppalachiaObject : AutonamedIdentifiableAppalachiaObject,
                                                             ICategorizable
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("category")]
        [SerializeField]
#if UNITY_EDITOR
        [FoldoutGroup(GROUP)]
        [SmartLabel]
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

        #endregion

        #region ICategorizable Members

        public string Category => _category;
        public string Category_ => $"{Category}_";

        #endregion

#if UNITY_EDITOR
        private bool _disableSelectCategory => string.IsNullOrWhiteSpace(Category);

        private void SelectCategory()
        {
            UnityEditor.Selection.objects = GetAllOfType(
                    GetType(),
                    i => ((CategorizableAppalachiaObject) i).Category == Category
                )
               .ToArray();
        }

        private void SelectUncategorized()
        {
            UnityEditor.Selection.objects = GetAllOfType(
                    GetType(),
                    i => string.IsNullOrWhiteSpace(((CategorizableAppalachiaObject) i).Category)
                )
               .ToArray();
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
