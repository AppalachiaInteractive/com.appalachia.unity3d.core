/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Internal;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Appalachia.Core.Editing.Groups
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class VerticalTabGroupAttribute : PropertyGroupAttribute, ISubGroupProviderAttribute
    {
        public const string DEFAULT_NAME = "_DefaultTabGroup";

        public string TabName;

        public bool UseFixedHeight;

        public bool Paddingless;

        public bool HideTabGroupIfTabGroupOnlyHasOneTab;

        public VerticalTabGroupAttribute(string tab, bool useFixedHeight = false, int order = 0) : this(
            "_DefaultTabGroup",
            tab,
            useFixedHeight,
            order
        )
        {
        }

        public VerticalTabGroupAttribute(string group, string tab, bool useFixedHeight = false, int order = 0) : base(group, order)
        {
            TabName = tab;
            UseFixedHeight = useFixedHeight;
            Tabs = new List<string>();
            if (tab != null)
            {
                Tabs.Add(tab);
            }

            Tabs = new List<string>(Tabs);
        }

        public List<string> Tabs { get; }

        IList<PropertyGroupAttribute> ISubGroupProviderAttribute.GetSubGroupAttributes()
        {
            var num = 0;
            var propertyGroupAttributeList = new List<PropertyGroupAttribute>(Tabs.Count);
            foreach (var tab in Tabs)
            {
                propertyGroupAttributeList.Add(new VerticalTabSubGroupAttribute(GroupID + "/" + tab, num++));
            }

            return propertyGroupAttributeList;
        }

        string ISubGroupProviderAttribute.RepathMemberAttribute(PropertyGroupAttribute attr)
        {
            return GroupID + "/" + ((VerticalTabGroupAttribute) attr).TabName;
        }

        protected override void CombineValuesWith(PropertyGroupAttribute other)
        {
            base.CombineValuesWith(other);
            var VerticalTabGroupAttribute = other as VerticalTabGroupAttribute;
            if (VerticalTabGroupAttribute.TabName == null)
            {
                return;
            }

            UseFixedHeight = UseFixedHeight || VerticalTabGroupAttribute.UseFixedHeight;
            Paddingless = Paddingless || VerticalTabGroupAttribute.Paddingless;
            HideTabGroupIfTabGroupOnlyHasOneTab =
                HideTabGroupIfTabGroupOnlyHasOneTab || VerticalTabGroupAttribute.HideTabGroupIfTabGroupOnlyHasOneTab;
            if (Tabs.Contains(VerticalTabGroupAttribute.TabName))
            {
                return;
            }

            Tabs.Add(VerticalTabGroupAttribute.TabName);
        }

        [Conditional("UNITY_EDITOR")]
        private class VerticalTabSubGroupAttribute : PropertyGroupAttribute
        {
            public VerticalTabSubGroupAttribute(string groupId, int order) : base(groupId, order)
            {
            }
        }
    }

    public class VerticalTabGroupAttributeDrawer : OdinGroupDrawer<VerticalTabGroupAttribute>
    {
        private static readonly object animatedTabGroupKey = new object();
        private GUIVerticalTabGroup tabGroup;
        private LocalPersistentContext<int> currentPage;
        private List<Tab> tabs;

        public static GUIVerticalTabGroup CreateAnimatedTabGroup(object key)
        {
            var guiTabGroup = GUIHelper.GetTemporaryContext<GUIVerticalTabGroup>(animatedTabGroupKey, key).Value;
            guiTabGroup.AnimationSpeed = 1f / SirenixEditorGUI.TabPageSlideAnimationDuration;
            return guiTabGroup;
        }

        protected override void Initialize()
        {
            tabGroup = CreateAnimatedTabGroup(Property);
            currentPage = this.GetPersistentValue<int>("CurrentPage");
            tabs = new List<Tab>();
            var tabList = new List<Tab>();
            for (var index1 = 0; index1 < Property.Children.Count; ++index1)
            {
                var child = Property.Children[index1];
                var flag = false;
                if (child.Info.PropertyType == PropertyType.Group)
                {
                    var type = child.GetAttribute<PropertyGroupAttribute>().GetType();
                    if (type.IsNested && (type.DeclaringType == typeof(VerticalTabGroupAttribute)))
                    {
                        var tab = new Tab();
                        tab.TabName = child.NiceName;
                        tab.Title = new StringMemberHelper(Property, child.Name.TrimStart('#'));
                        for (var index2 = 0; index2 < child.Children.Count; ++index2)
                        {
                            tab.InspectorProperties.Add(child.Children[index2]);
                        }

                        tabs.Add(tab);
                        flag = true;
                    }
                }

                if (!flag)
                {
                    tabList.Add(
                        new Tab
                        {
                            TabName = child.NiceName,
                            Title = new StringMemberHelper(Property, child.Name.TrimStart('#')),
                            InspectorProperties = {child}
                        }
                    );
                }
            }

            foreach (var tab in tabList)
            {
                tabs.Add(tab);
            }

            for (var index = 0; index < tabs.Count; ++index)
            {
                tabGroup.RegisterTab(tabs[index].TabName);
            }

            tabGroup.SetCurrentPage(tabGroup.RegisterTab(tabs[currentPage.Value].TabName));
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var property = Property;
            var attribute = Attribute;
            if (attribute.HideTabGroupIfTabGroupOnlyHasOneTab && (tabs.Count <= 1))
            {
                for (var index1 = 0; index1 < tabs.Count; ++index1)
                {
                    var count = tabs[index1].InspectorProperties.Count;
                    for (var index2 = 0; index2 < count; ++index2)
                    {
                        var inspectorProperty = tabs[index1].InspectorProperties[index2];
                        inspectorProperty.Update();
                        inspectorProperty.Draw(inspectorProperty.Label);
                    }
                }
            }
            else
            {
                tabGroup.AnimationSpeed = 1f / SirenixEditorGUI.TabPageSlideAnimationDuration;
                tabGroup.FixedHeight = attribute.UseFixedHeight;
                if ((currentPage.Value >= tabs.Count) || (currentPage.Value < 0))
                {
                    currentPage.Value = 0;
                }

                SirenixEditorGUI.BeginIndentedVertical(SirenixGUIStyles.PropertyPadding);
                tabGroup.BeginGroup(true, attribute.Paddingless ? GUIStyle.none : null);
                for (var index1 = 0; index1 < tabs.Count; ++index1)
                {
                    var gUIVerticalTabPage = tabGroup.RegisterTab(tabs[index1].TabName);
                    gUIVerticalTabPage.Title = tabs[index1].Title.GetString(property);
                    if (gUIVerticalTabPage.BeginPage())
                    {
                        currentPage.Value = index1;
                        var count = tabs[index1].InspectorProperties.Count;
                        for (var index2 = 0; index2 < count; ++index2)
                        {
                            var inspectorProperty = tabs[index1].InspectorProperties[index2];
                            inspectorProperty.Update();
                            inspectorProperty.Draw(inspectorProperty.Label);
                        }
                    }

                    gUIVerticalTabPage.EndPage();
                }

                tabGroup.EndGroup();
                SirenixEditorGUI.EndIndentedVertical();
            }
        }

        private class Tab
        {
            public readonly List<InspectorProperty> InspectorProperties = new List<InspectorProperty>();
            public string TabName;
            public StringMemberHelper Title;
        }
    }

    public class GUIVerticalTabPage
    {
        private static GUIStyle innerContainerStyle;
        private static int pageIndexIncrementer;
        internal int Order;
        private readonly GUIVerticalTabGroup tabGroup;
        private Color prevColor;
        private bool isSeen;
        private bool isMessured;

        internal GUIVerticalTabPage(GUIVerticalTabGroup tabGroup, string title)
        {
            Name = title;
            Title = title;
            this.tabGroup = tabGroup;
            IsActive = true;
        }

        private static GUIStyle InnerContainerStyle
        {
            get
            {
                if (innerContainerStyle == null)
                {
                    innerContainerStyle = new GUIStyle {padding = new RectOffset(3, 3, 3, 3)};
                }

                return innerContainerStyle;
            }
        }

        internal string Name { get; }

        public string Title { get; set; }

        public Rect Rect { get; private set; }

        internal bool IsActive { get; set; }

        internal bool IsVisible { get; set; }

        internal void OnBeginGroup()
        {
            pageIndexIncrementer = 0;
            isSeen = false;
        }

        internal void OnEndGroup()
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            IsActive = isSeen;
        }

        public bool BeginPage()
        {
            if (tabGroup.FixedHeight && !isMessured)
            {
                IsVisible = true;
            }

            isSeen = true;
            if (IsVisible)
            {
                var rect = EditorGUILayout.BeginVertical(
                    InnerContainerStyle,
                    GUILayoutOptions.Width(tabGroup.InnerContainerWidth).ExpandHeight(tabGroup.ExpandHeight)
                );
                GUIHelper.PushHierarchyMode(false);
                GUIHelper.PushLabelWidth(tabGroup.LabelWidth - 4f);
                if (Event.current.type == EventType.Repaint)
                {
                    Rect = rect;
                }

                if (tabGroup.IsAnimating)
                {
                    this.prevColor = GUI.color;
                    var prevColor = this.prevColor;
                    prevColor.a *= tabGroup.CurrentPage == this ? tabGroup.T : 1f - tabGroup.T;
                    GUI.color = prevColor;
                }
            }

            return IsVisible;
        }

        public void EndPage()
        {
            if (IsVisible)
            {
                GUIHelper.PopLabelWidth();
                GUIHelper.PopHierarchyMode();
                if (tabGroup.IsAnimating)
                {
                    GUI.color = prevColor;
                }

                EditorGUILayout.EndVertical();
            }

            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            isMessured = true;
            Order = pageIndexIncrementer++;
        }
    }

    public class GUIVerticalTabGroup
    {
        private GUILayoutOption[] options = GUILayoutOptions.ExpandWidth().ExpandHeight(false);
        private readonly EditorTimeHelper time = new EditorTimeHelper();
        private readonly Dictionary<string, GUIVerticalTabPage> pages = new Dictionary<string, GUIVerticalTabPage>();

        public float AnimationSpeed = 4f;
        private GUIVerticalTabPage currentPage;
        private GUIVerticalTabPage targetPage;
        private Vector2 scrollPosition;
        private float currentHeight;
        private GUIVerticalTabPage nextPage;
        private bool drawToolbar;
        private Rect toolbarRect;
        public bool FixedHeight;

        // ReSharper disable once UnassignedField.Global
        public bool ExpandHeight;

        private IEnumerable<GUIVerticalTabPage> OrderedPages
        {
            get { return pages.Select(x => x.Value).OrderBy(x => x.Order); }
        }

        public Rect OuterRect { get; private set; }

        public Rect InnerRect { get; private set; }

        public GUIVerticalTabPage CurrentPage => targetPage ?? currentPage;

        public float T { get; private set; } = 1f;

        internal bool IsAnimating { get; private set; }

        internal float InnerContainerWidth { get; private set; }

        internal float LabelWidth { get; private set; }

        public float ToolbarHeight { get; set; } = 18f;

        public void SetCurrentPage(GUIVerticalTabPage page)
        {
            if (!pages.ContainsValue(page))
            {
                throw new InvalidOperationException("Page is not part of TabGroup");
            }

            currentPage = page;
            targetPage = null;
        }

        public GUIVerticalTabPage RegisterTab(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            GUIVerticalTabPage gUIVerticalTabPage;
            if (!pages.TryGetValue(title, out gUIVerticalTabPage))
            {
                gUIVerticalTabPage = pages[title] = new GUIVerticalTabPage(this, title);
            }

            return gUIVerticalTabPage;
        }

        public void BeginGroup(bool drawToolbar = true, GUIStyle style = null)
        {
            LabelWidth = GUIHelper.BetterLabelWidth;
            if (Event.current.type == EventType.Layout)
            {
                this.drawToolbar = drawToolbar;
            }

            style = style ?? SirenixGUIStyles.ToggleGroupBackground;
            InnerContainerWidth = OuterRect.width - (style.padding.left + style.padding.right + style.margin.left + style.margin.right);
            if ((currentPage == null) && (pages.Count > 0))
            {
                currentPage = pages.Select(x => x.Value).OrderBy(x => x.Order).First();
            }

            if ((currentPage != null) && !pages.ContainsKey(currentPage.Name))
            {
                currentPage = pages.Count <= 0 ? null : OrderedPages.First();
            }

            var num1 = 0.0f;
            foreach (var gUIVerticalTabPage in pages.GFValueIterator())
            {
                gUIVerticalTabPage.OnBeginGroup();
                var rect = gUIVerticalTabPage.Rect;
                num1 = Mathf.Max(rect.height, num1);
                if ((Event.current.type == EventType.Layout) &&
                    (gUIVerticalTabPage.IsVisible !=
                        (gUIVerticalTabPage.IsVisible = (gUIVerticalTabPage == targetPage) || (gUIVerticalTabPage == currentPage))))
                {
                    if (targetPage == null)
                    {
                        scrollPosition.x = 0.0f;
                        rect = currentPage.Rect;
                        currentHeight = rect.height;
                    }
                    else
                    {
                        ref var local1 = ref scrollPosition;
                        double num2;
                        if (targetPage.Order < currentPage.Order)
                        {
                            ref var local2 = ref scrollPosition;
                            rect = OuterRect;
                            double width;
                            var num3 = (float) (width = rect.width);
                            local2.x = (float) width;
                            num2 = num3;
                        }
                        else
                        {
                            num2 = 0.0;
                        }

                        local1.x = (float) num2;
                        rect = currentPage.Rect;
                        currentHeight = rect.height;
                    }
                }
            }

            GUILayout.Space(1f);
            var rect1 = EditorGUILayout.BeginVertical(style, GUILayoutOptions.ExpandWidth().ExpandHeight(ExpandHeight));
            if (this.drawToolbar)
            {
                DrawToolbar();
            }

            if ((InnerRect.width > 0.0) && !ExpandHeight)
            {
                if (options.Length == 2)
                {
                    if (currentPage != null)
                    {
                        currentHeight = currentPage.Rect.height;
                    }

                    options = GUILayoutOptions.ExpandWidth().ExpandHeight(ExpandHeight).Height(currentHeight);
                }

                options[2] = !FixedHeight ? GUILayout.Height(currentHeight) : GUILayout.Height(num1);
            }

            GUIHelper.PushGUIEnabled(false);
            GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUIStyle.none, options);
            GUIHelper.PopGUIEnabled();
            var rect2 = EditorGUILayout.BeginHorizontal(GUILayoutOptions.ExpandHeight(ExpandHeight));
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            OuterRect = rect1;
            InnerRect = rect2;
        }

        public void EndGroup()
        {
            EditorGUILayout.EndHorizontal();
            GUIHelper.PushGUIEnabled(false);
            GUILayout.EndScrollView();
            GUIHelper.PopGUIEnabled();
            EditorGUILayout.EndVertical();
            if (targetPage != currentPage)
            {
                GUIHelper.RequestRepaint();
            }

            if ((currentPage != null) && (Event.current.type == EventType.Repaint))
            {
                if (IsAnimating && (targetPage != null) && (targetPage != currentPage))
                {
                    T += time.DeltaTime * AnimationSpeed;
                    scrollPosition.x = Mathf.Lerp(currentPage.Rect.x, targetPage.Rect.x, Mathf.Min(1f, MathUtilities.Hermite01(T)));
                    currentHeight = Mathf.Lerp(currentPage.Rect.height, targetPage.Rect.height, Mathf.Min(1f, MathUtilities.Hermite01(T)));
                    if (T >= 1.0)
                    {
                        currentPage.IsVisible = false;
                        currentPage = targetPage;
                        targetPage = null;
                        scrollPosition.x = 0.0f;
                        currentHeight = currentPage.Rect.height;
                        T = 1f;
                    }
                }
                else
                {
                    T = 0.0f;
                    IsAnimating = false;
                    scrollPosition.x = currentPage.Rect.x;
                    currentHeight = currentPage.Rect.height;
                    if ((targetPage != null) && (targetPage != currentPage) && targetPage.IsVisible)
                    {
                        IsAnimating = true;
                        ref var local1 = ref scrollPosition;
                        double num1;
                        if (targetPage.Order <= currentPage.Order)
                        {
                            ref var local2 = ref scrollPosition;
                            var outerRect = OuterRect;
                            double width;
                            var num2 = (float) (width = outerRect.width);
                            local2.x = (float) width;
                            num1 = num2;
                        }
                        else
                        {
                            num1 = 0.0;
                        }

                        local1.x = (float) num1;
                        T = 0.0f;
                    }
                }
            }

            foreach (var gUIVerticalTabPage in pages.GFValueIterator())
            {
                gUIVerticalTabPage.OnEndGroup();
            }

            time.Update();
            if (IsAnimating || (nextPage == null))
            {
                return;
            }

            targetPage = nextPage;
            nextPage = null;
        }

        private void DrawToolbar()
        {
            if (Event.current.type == EventType.Layout)
            {
                toolbarRect = OuterRect;
                toolbarRect.height = ToolbarHeight;
                ++toolbarRect.x;
                --toolbarRect.width;
            }

            BeginVerticalToolbar(ToolbarHeight);
            foreach (var orderedPage in OrderedPages)
            {
                if (orderedPage.IsActive && SirenixEditorGUI.ToolbarTab(orderedPage == (nextPage ?? CurrentPage), orderedPage.Title))
                {
                    nextPage = orderedPage;
                }
            }

            EndVerticalToolbar();
            if (!Event.current.OnRepaint())
            {
                return;
            }

            SirenixEditorGUI.DrawBorders(new Rect(GUILayoutUtility.GetLastRect()) {height = ToolbarHeight}, 1, 1, 0, 0);
        }

        public void GoToPage(GUIVerticalTabPage page)
        {
            nextPage = page;
        }

        public void GoToNextPage()
        {
            if (currentPage == null)
            {
                return;
            }

            var flag = false;
            var list = OrderedPages.ToList();
            for (var index = 0; index < list.Count; ++index)
            {
                if (flag && list[index].IsActive)
                {
                    nextPage = list[index];
                    break;
                }

                if (list[index] == (nextPage ?? CurrentPage))
                {
                    flag = true;
                }
            }
        }

        public void GoToPreviousPage()
        {
            if (currentPage == null)
            {
                return;
            }

            var list = OrderedPages.ToList();
            var index1 = -1;
            for (var index2 = 0; index2 < list.Count; ++index2)
            {
                if (list[index2] == (nextPage ?? CurrentPage))
                {
                    if (index1 < 0)
                    {
                        break;
                    }

                    nextPage = list[index1];
                    break;
                }

                if (list[index2].IsActive)
                {
                    index1 = index2;
                }
            }
        }

        public static Rect BeginVerticalToolbar(float width = 22f, int paddingLeft = 4)
        {
            return BeginVerticalToolbar(SirenixGUIStyles.ToolbarBackground, width, paddingLeft);
        }

        public static Rect BeginVerticalToolbar(GUIStyle style, float width = 22f, int paddingLeft = 4)
        {
            currentDrawingToolbarWidth = width;
            Rect rect = EditorGUILayout.BeginVertical(style, (GUILayoutOption[]) GUILayoutOptions.Width(width).ExpandHeight(false));
            GUIHelper.PushHierarchyMode(false, true);
            GUIHelper.PushIndentLevel(0);
            return rect;
        }

        public static void EndVerticalToolbar()
        {
            if (Event.current.type == EventType.Repaint)
            {
                Rect currentLayoutRect = GUIHelper.GetCurrentLayoutRect();

                var min = currentLayoutRect.min;
                min.x -= 1;
                currentLayoutRect.min = min;

                SirenixEditorGUI.DrawBorders(currentLayoutRect, 1, true);
            }

            GUIHelper.PopIndentLevel();
            GUIHelper.PopHierarchyMode();
            EditorGUILayout.EndVertical();
        }

        public static Rect BeginVerticalToolbarBox(string label, bool centerLabel = false, params GUILayoutOption[] options)
        {
            return string.IsNullOrEmpty(label)
                ? SirenixEditorGUI.BeginToolbarBox(options)
                : SirenixEditorGUI.BeginToolbarBox(GUIHelper.TempContent(label), centerLabel, options);
        }

        public static Rect BeginVerticalToolbarBox(GUIContent label, bool centerLabel = false, params GUILayoutOption[] options)
        {
            Rect rect = SirenixEditorGUI.BeginToolbarBox(options);
            if (label != null)
            {
                SirenixEditorGUI.BeginToolbarBoxHeader(22f);
                float fieldWidth = EditorGUIUtility.get_fieldWidth();
                EditorGUIUtility.set_fieldWidth(10f);
                Rect controlRect = EditorGUILayout.GetControlRect(false, new GUILayoutOption[0]);
                EditorGUIUtility.set_fieldWidth(fieldWidth);
                GUI.Label(controlRect, label, centerLabel ? SirenixGUIStyles.LabelCentered : SirenixGUIStyles.Label);
                SirenixEditorGUI.EndToolbarBoxHeader();
            }

            return rect;
        }

        public static Rect BeginVerticalToolbarBox(params GUILayoutOption[] options)
        {
            SirenixEditorGUI.BeginIndentedVertical(SirenixGUIStyles.BoxContainer, options);
            GUIHelper.PushHierarchyMode(false, true);
            GUIHelper.PushLabelWidth(GUIHelper.BetterLabelWidth - 4f);
            return GUIHelper.GetCurrentLayoutRect();
        }

        public static void EndVerticalToolbarBox()
        {
            GUIHelper.PopLabelWidth();
            GUIHelper.PopHierarchyMode();
            SirenixEditorGUI.EndIndentedVertical();
        }

        private static float currentDrawingToolbarWidth;

        public static Rect BeginVerticalToolbarBoxHeader(float width = 22f)
        {
            GUILayout.Space(-3f);
            currentDrawingToolbarWidth = width;
            Rect rect1 = EditorGUILayout.BeginVertical(
                SirenixGUIStyles.BoxHeaderStyle,
                (GUILayoutOption[]) GUILayoutOptions.Width(width).ExpandHeight(true)
            );
            GUILayout.Space(0.0f);
            if (Event.current.type == EventType.Repaint)
            {
                rect1.width = rect1.width + 6;
                
                var min = rect1.min;
                min.y -= 3f;                
                rect1.min = min;
                
                SirenixGUIStyles.ToolbarBackground.Draw(rect1, (GUIContent) GUIContent.none, 0);
            }

            return rect1;
        }

        public static void EndVerticalToolbarBoxHeader()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
*/


