using System;
using System.Collections;
using System.Collections.Generic;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Aspects.Tracing;
using Appalachia.Core.Context.Analysis.Core;
using Appalachia.Core.Context.Elements.Progress;
using Appalachia.Core.Context.Interfaces;
using Appalachia.Core.Preferences;
using Unity.Profiling;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AnalysisMenuContext<TT, TA, TE> : AppaOneMenuContext<TA>
        where TA : AnalysisMetadata<TA, TT, TE>, new()
        where TE : Enum
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(AnalysisMenuContext<TT, TA, TE>) + ".";

        private const string _TRACE_PFX = nameof(AnalysisMenuContext<TT, TA, TE>) + ".";

        private static readonly ProfilerMarker _PRF_ShouldShowInMenu =
            new(_PRF_PFX + nameof(ShouldShowInMenu));

        private static readonly ProfilerMarker _PRF_ValidateSummaryProperties =
            new(_PRF_PFX + nameof(ValidateSummaryProperties));

        private static readonly ProfilerMarker _PRF_OnReset = new(_PRF_PFX + nameof(OnReset));
        private static readonly ProfilerMarker _PRF_OnInitialize = new(_PRF_PFX + nameof(OnInitialize));

        private static readonly TraceMarker _TRACE_OnInitialize = new(_TRACE_PFX + nameof(OnInitialize));

        private static readonly ProfilerMarker _PRF_ValidateMenuSelection =
            new(_PRF_PFX + nameof(ValidateMenuSelection));

        #endregion

        #region Preferences

        private PREF<bool> _generateTestFiles;

        private PREF<TE> _issueType;

        private PREF<bool> _onlyShowIssues;

        public PREF<bool> GenerateTestFiles => _generateTestFiles;

        public PREF<TE> IssueType => _issueType;

        public PREF<bool> OnlyShowIssues => _onlyShowIssues;

        #endregion

        public int detailTabIndex;

        protected List<TA> items;

        private AnalysisAggregate<TE> _aggregateAnalysis;

        protected abstract string GetPreferencePrefix { get; }

        protected abstract string IssueTypeName { get; }
        public override IReadOnlyList<TA> MenuOneItems => items;

        public AnalysisAggregate<TE> AggregateAnalysis
        {
            get => _aggregateAnalysis;
            protected set => _aggregateAnalysis = value;
        }

        public virtual bool ShouldShowInMenu(TA analysis)
        {
            using (_PRF_ShouldShowInMenu.Auto())
            {
                if ((analysis == null) || (analysis.Target == null))
                {
                    return false;
                }

                if (_onlyShowIssues.Value && !analysis.AnyIssues)
                {
                    return false;
                }

                if (_onlyShowIssues.Value && !Equals(_issueType.Value, default(TE)))
                {
                    if (!analysis.HasIssues(_issueType.v))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void ValidateSummaryProperties()
        {
            using (_PRF_ValidateSummaryProperties.Auto())
            {
                if (_aggregateAnalysis == null)
                {
                    _aggregateAnalysis = new AnalysisAggregate<TE>();
                }

                for (var index = 0; index < MenuOneItems.Count; index++)
                {
                    var analysis = MenuOneItems[index];

                    if (analysis == null)
                    {
                        continue;
                    }

                    var target = analysis.Target;

                    if ((target != null) && ShouldShowInMenu(analysis))
                    {
                        _aggregateAnalysis.Add(analysis.AllIssues);
                    }
                }
            }
        }

        public override void ValidateMenuSelection(int menuIndex)
        {
            using (_PRF_ValidateMenuSelection.Auto())
            {
                var menuSelection = GetMenuSelection(menuIndex);

                if (menuSelection.length != MenuOneItems.Count)
                {
                    menuSelection.SetLength(MenuOneItems.Count);

                    ValidateSummaryProperties();
                }
            }
        }

        protected override IEnumerable<AppaProgress> OnPreInitialize(AppaProgressCounter pc)
        {
            yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Only_Issues}", 1);

            _onlyShowIssues = PREFS.REG(GetPreferencePrefix, APPASTR.Only_Issues, true);

            yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Issue_Type_Assembly}", 1);

            _issueType = PREFS.REG(GetPreferencePrefix, $"{APPASTR.Issue_Type} {IssueTypeName}", default(TE));

            yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Test_Files}", 1);

            _generateTestFiles = PREFS.REG(GetPreferencePrefix, APPASTR.Test_Files, true);
        }

        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                AggregateAnalysis?.Reset();

                items = null;
            }
        }

        public void ResetAnalysis()
        {
            AggregateAnalysis = null;
        }

        public virtual IEnumerator RegisterPreferences(IPreferencesDrawer drawer)
        {
            drawer.RegisterFilterPref(OnlyShowIssues);

            yield return null;

            drawer.RegisterFilterPref(IssueType, () => OnlyShowIssues.v);

            yield return null;

            drawer.RegisterFilterPref(GenerateTestFiles);

            yield return null;
        }
    }
}
