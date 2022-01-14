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
        #region Constants and Static Readonly

        private const string VALUE_FIELD_NAME = "_value";

        #endregion

        public override void ProcessChildMemberAttributes(
            InspectorProperty parentProperty,
            MemberInfo member,
            List<Attribute> attributes)
        {
            var overridableFieldNames = new HashSet<string> { VALUE_FIELD_NAME };

            if (!overridableFieldNames.Contains(member.Name))
            {
                return;
            }

            var parentFieldInfo = parentProperty.Info.GetMemberInfo();

            var atties = parentFieldInfo?.GetAttributes_CACHE(true);

            if (atties == null)
            {
                return;
            }

            for (var index = 0; index < atties.Length; index++)
            {
                var attribute = atties[index];

                if (member.Name == VALUE_FIELD_NAME)
                {
                    if (ShouldPushToChildren(attribute))
                    {
                        attributes.Add(attribute);
                    }
                }
            }
        }

        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
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

        private static bool ShouldPushToChildren(Attribute a)
        {
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
