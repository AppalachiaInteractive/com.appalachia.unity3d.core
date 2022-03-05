#if UNITY_EDITOR

#region

using System;
using System.Collections.Generic;
using System.Reflection;
using Appalachia.Core.Objects.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Overrides
{
    [ResolverPriority(500)]
    public class OverrideableAttributeProcessor : OdinAttributeProcessor
    {
        #region Constants and Static Readonly

        private const string VALUE_FIELD_NAME = "_value";

        #endregion

        #region Static Fields and Autoproperties

        private static HashSet<string> _overridableFieldNames;

        #endregion

        /// <inheritdoc />
        public override void ProcessChildMemberAttributes(
            InspectorProperty parentProperty,
            MemberInfo member,
            List<Attribute> attributes)
        {
            using (_PRF_ProcessChildMemberAttributes.Auto())
            {
                _overridableFieldNames ??= new HashSet<string> { VALUE_FIELD_NAME };

                if (_overridableFieldNames.Contains(member.Name))
                {
                    var parentFieldInfo = parentProperty.Info.GetMemberInfo();

                    var parentAttributes = parentFieldInfo?.GetAttributes_CACHE(true);

                    if (parentAttributes == null)
                    {
                        return;
                    }

                    AddAttributeToChild(member, parentAttributes, attributes);
                }
                else
                {
                    if (member.MemberType is not (MemberTypes.Field or MemberTypes.Property))
                    {
                        return;
                    }

                    var isOverridable = member.ReflectedType.IsOverridable();

                    if (isOverridable)
                    {
                        foreach (var childProperty in parentProperty.Children)
                        {
                            if (childProperty.Name == member.Name)
                            {
                                Console.WriteLine(childProperty);
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            using (_PRF_ProcessSelfAttributes.Auto())
            {
                if (property.Info.TypeOfValue == null)
                {
                    return;
                }

                var isOverridable = property.Info.TypeOfValue.IsOverridable();

                if (property.Name == "alpha")
                {
                    Console.WriteLine(property);
                }

                if (isOverridable)
                {
                    RemoveAttributesFromSelf(attributes);
                }
            }
        }

        private static bool ShouldPushToChildren(Attribute a)
        {
            using (_PRF_ShouldPushToChildren.Auto())
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

                if (a is DelayedPropertyAttribute)
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

        private void AddAttributeToChild(
            MemberInfo child,
            Attribute[] parentAttributes,
            List<Attribute> childAttributes)
        {
            using (_PRF_AddAttributeToChild.Auto())
            {
                _overridableFieldNames ??= new HashSet<string> { VALUE_FIELD_NAME };

                if (!_overridableFieldNames.Contains(child.Name))
                {
                    return;
                }

                for (var index = 0; index < parentAttributes.Length; index++)
                {
                    var parentAttribute = parentAttributes[index];

                    if (ShouldPushToChildren(parentAttribute))
                    {
                        childAttributes.Add(parentAttribute);
                    }
                }
            }
        }

        private void RemoveAttributesFromSelf(List<Attribute> attributes)
        {
            using (_PRF_RemoveAttributesFromSelf.Auto())
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

        #region Profiling

        private const string _PRF_PFX = nameof(OverrideableAttributeProcessor) + ".";

        private static readonly ProfilerMarker _PRF_RemoveAttributesFromSelf =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveAttributesFromSelf));

        private static readonly ProfilerMarker _PRF_ProcessSelfAttributes =
            new ProfilerMarker(_PRF_PFX + nameof(ProcessSelfAttributes));

        private static readonly ProfilerMarker _PRF_ProcessChildMemberAttributes =
            new ProfilerMarker(_PRF_PFX + nameof(ProcessChildMemberAttributes));

        private static readonly ProfilerMarker _PRF_AddAttributeToChild =
            new ProfilerMarker(_PRF_PFX + nameof(AddAttributeToChild));

        private static readonly ProfilerMarker _PRF_ShouldPushToChildren =
            new ProfilerMarker(_PRF_PFX + nameof(ShouldPushToChildren));

        #endregion
    }
}

#endif
