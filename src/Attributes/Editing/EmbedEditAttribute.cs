#region

using System;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    [IncludeMyAttributes]
    [InlineEditor(Expanded = true)]
    [HideLabel]
    [LabelWidth(0)]
    public class EmbedEditAttribute : Attribute
    {
    }
}
