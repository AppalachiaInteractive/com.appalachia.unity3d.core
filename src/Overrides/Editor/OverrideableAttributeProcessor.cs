#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides
{
#if UNITY_EDITOR
    [ResolverPriority(500)]
    public class OverrideableAttributeProcessor : OdinAttributeProcessor
    {
        public override void ProcessSelfAttributes(
            InspectorProperty property,
            List<Attribute> attributes)
        {
            if (property.Info.TypeOfValue == null)
            {
                return;
            }

            var match = typeof(Overridable<,>).IsAssignableFrom(property.Info.TypeOfValue);

            if (match)
            {
                for (var i = attributes.Count - 1; i >= 0; i--)
                {
                    if (ShouldPushToChildren(attributes[i]))
                    {
                        attributes.Remove(attributes[i]);
                    }
                }
            }
        }

        public override void ProcessChildMemberAttributes(
            InspectorProperty parentProperty,
            MemberInfo member,
            List<Attribute> attributes)
        {
            if (member.Name != "value")
            {
                return;
            }

            var thisType = member.GetReturnType();

            var atties = parentProperty.Info.GetMemberInfo()?.GetAttributes_CACHE();

            foreach (var attribute in atties)
            {
                if (ShouldPushToChildren(attribute))
                {
                    if (member.Name == "value")
                    {
                        attributes.Add(attribute);
                    }
                }
            }
        }

        private static bool ShouldPushToChildren(Attribute a)
        {
            if (a is OnValueChangedAttribute)
            {
                return true;
            }

            if (a is PropertyRangeAttribute)
            {
                return true;
            }

            if (a is RangeAttribute)
            {
                return true;
            }

            if (a is MinValueAttribute)
            {
                return true;
            }

            if (a is MinAttribute)
            {
                return true;
            }

            if (a is MaxValueAttribute)
            {
                return true;
            }

            if (a is MinMaxSliderAttribute)
            {
                return true;
            }

            if (a is ToggleLeftAttribute)
            {
                return true;
            }

            return false;
        }
    }
#endif
}
