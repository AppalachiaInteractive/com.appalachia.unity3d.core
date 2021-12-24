using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.Core.Context.Elements;
using Appalachia.Utility.Execution.Progress;
using UnityEngine;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AppaMenuContext : AppaUIContext
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(AppaMenuContext) + ".";

        #endregion

        private AppaMenuSelection[] _menuSelections;

        public virtual bool NeedsProgressDraw { get; }

        public string ProgressVerb { get; set; }

        public abstract int RequiredMenuCount { get; }
        public virtual float MenuWidth => 300f;

        public bool CurrentSelectionVisible
        {
            get
            {
                var menuSelection = GetMenuSelection(0);
                var menuItemIndex = menuSelection.currentIndex;
                var visibility = menuSelection.IsVisible(menuItemIndex);

                return visibility;
            }
        }

        public abstract int GetMenuItemCount(int menuIndex);

        public abstract void ValidateMenuSelection(int menuIndex);

        protected abstract void OnReset();

        public virtual AppaProgress GetProgress()
        {
            return default;
        }

        // ReSharper disable once UnusedParameter.Global
        protected virtual void OnAfterChangeMenuSelection(int menuIndex)
        {
        }

        // ReSharper disable once UnusedParameter.Global
        protected virtual void OnBeforeChangeMenuSelection(int menuIndex)
        {
        }

        public bool ChangeMenuSelection(int menuIndex, bool up)
        {
            OnBeforeChangeMenuSelection(menuIndex);

            var menuSelection = GetMenuSelection(menuIndex);

            var visibleIndex = menuSelection.currentVisibleIndex;

            if (up && (visibleIndex == 0))
            {
                return false;
            }

            if (!up && (visibleIndex >= (menuSelection.visibleCount - 1)))
            {
                return false;
            }

            var nextVisibleIndex = visibleIndex + (up ? -1 : 1);

            var nextIndex = menuSelection.GetIndex(nextVisibleIndex);

            menuSelection.SetSelected(nextIndex);

            OnAfterChangeMenuSelection(menuIndex);

            return true;
        }

        public int GetActiveMenuIndex(Vector2 mousePosition)
        {
            for (var i = 0; i < RequiredMenuCount; i++)
            {
                var selection = GetMenuSelection(i);

                if (selection.position.Contains(mousePosition))
                {
                    return i;
                }
            }

            return 0;
        }

        public AppaMenuSelection GetMenuSelection(int menuIndex)
        {
            InitializeMenuSelections();

            return _menuSelections[menuIndex];
        }

        public void Reset()
        {
            _menuSelections = null;
            Initialized = false;

            OnReset();
        }

        protected override IEnumerable<AppaProgress> OnPostInitialize(AppaProgressCounter pc)
        {
            InitializeMenuSelections();

            yield return (APPASTR.Completed, 100f);
        }

        protected IEnumerable<TL> GetVisibleItems<TL>(int menuIndex, IReadOnlyList<TL> items)
        {
            var menuSelection = GetMenuSelection(menuIndex);

            for (var menuItemIndex = 0; menuItemIndex < items.Count; menuItemIndex++)
            {
                var visible = menuSelection.IsVisible(menuItemIndex);

                if (!visible)
                {
                    continue;
                }

                var menuItem = items[menuItemIndex];

                yield return menuItem;
            }
        }

        private void InitializeMenuSelections()
        {
            if (_menuSelections == null)
            {
                _menuSelections = new AppaMenuSelection[RequiredMenuCount];
            }

            for (var menuIndex = 0; menuIndex < RequiredMenuCount; menuIndex++)
            {
                var current = _menuSelections[menuIndex];

                if (current == null)
                {
                    current = new AppaMenuSelection();
                    _menuSelections[menuIndex] = current;
                }
            }
        }
    }
}
