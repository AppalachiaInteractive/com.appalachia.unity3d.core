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
    public class SelectableStateAttributeProcessor : OdinAttributeProcessor
    {
        #region Constants and Static Readonly

        private const string NORMAL_FIELD_NAME = "_normal";
        private const string HOVERING_FIELD_NAME = "_hovering";
        private const string PRESSED_FIELD_NAME = "_pressed";
        private const string DRAGGING_FIELD_NAME = "_dragging";
        private const string SELECTED_FIELD_NAME = "_selected";
        private const string DISABLED_FIELD_NAME = "_disabled";

        #endregion

        #region Static Fields and Autoproperties

        private static HashSet<string> _fieldNames;

        #endregion

        public override void ProcessChildMemberAttributes(
            InspectorProperty parentProperty,
            MemberInfo member,
            List<Attribute> attributes)
        {
            using (_PRF_ProcessChildMemberAttributes.Auto())
            {
                InitializeFieldNames();

                var isSelectableState = parentProperty.Info.TypeOfValue.IsSelectableState();

                if (!isSelectableState)
                {
                    return;
                }

                if (_fieldNames.Contains(member.Name))
                {
                    var parentFieldInfo = parentProperty.Info.GetMemberInfo();

                    var parentAttributes = parentFieldInfo?.GetAttributes_CACHE(true);

                    if (parentAttributes == null)
                    {
                        return;
                    }

                    AddAttributeToChild(parentAttributes, attributes);
                }
            }
        }
        
        /// <inheritdoc />
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            using (_PRF_ProcessSelfAttributes.Auto())
            {
                InitializeFieldNames();

                if (property.Info.TypeOfValue == null)
                {
                    return;
                }

                var isValidTarget = property.Info.TypeOfValue.IsSelectableState();

                if (!isValidTarget)
                {
                    return;
                }

                RemoveAttributesFromSelf(attributes);
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

        private void InitializeFieldNames()
        {
            _fieldNames ??= new HashSet<string>
            {
                NORMAL_FIELD_NAME,
                HOVERING_FIELD_NAME,
                PRESSED_FIELD_NAME,
                DRAGGING_FIELD_NAME,
                SELECTED_FIELD_NAME,
                DISABLED_FIELD_NAME
            };
        }

        private void AddAttributeToChild(
            IList<Attribute> parentAttributes,
            List<Attribute> childAttributes)
        {
            using (_PRF_AddAttributeToChild.Auto())
            {
                InitializeFieldNames();

                for (var index = 0; index < parentAttributes.Count; index++)
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
