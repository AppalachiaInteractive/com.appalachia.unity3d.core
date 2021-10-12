#region

using System;
using Sirenix.OdinInspector;

#endregion

namespace Appalachia.Core.Attributes.Editing
{
    [IncludeMyAttributes]
    [InlineProperty]
    [HideLabel]
    [LabelWidth(0)]
    public class EmbedAttribute : Attribute
    {
    }
}
