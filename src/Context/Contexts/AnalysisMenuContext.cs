using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Core.Aspects.Tracing;
using Appalachia.Core.Context.Analysis.Core;
using Appalachia.Core.Context.Interfaces;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Execution.Progress;
using Appalachia.Utility.Strings;
using Unity.Profiling;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class AnalysisMenuContext<TT, TA, TE> : AppaOneMenuContext<TA>
        where TA : AnalysisGroup<TA, TT, TE>, new()
        where TE : Enum
    {
        #region Constants and Static Readonly

        private static readonly ProfilerMarker _PRF_ShouldShowInMenu =
            new(_PRF_PFX + nameof(ShouldShowInMenu));

        private static readonly ProfilerMarker _PRF_ValidateSummaryProperties =
            new(_PRF_PFX + nameof(ValidateSummaryProperties));

        private static readonly ProfilerMarker _PRF_OnReset = new(_PRF_PFX + nameof(OnReset));
        private static readonly ProfilerMarker _PRF_OnInitialize = new(_PRF_PFX + nameof(OnInitialize));
        private static readonly TraceMarker _TRACE_OnInitialize = new(_TRACE_PFX + nameof(OnInitialize));

        #endregion

        #region Preferences

        private PREF<bool> _generateTestFiles;

        private PREF<TE> _issueType;

        private PREF<float> _menuWidth;

        private PREF<bool> _onlyShowIssues;

        public PREF<bool> GenerateTestFiles => _generateTestFiles;

        public PREF<TE> IssueType => _issueType;

        public PREF<bool> OnlyShowIssues => _onlyShowIssues;

        #endregion

        #region Fields and Autoproperties

        public int detailTabIndex;

        protected List<TA> items;

        private AnalysisAggregate<TA, TT, TE> _aggregateAnalysis;

        #endregion

        protected abstract string GetPreferencePrefix { get; }

        protected abstract string IssueTypeName { get; }

        public override float MenuWidth => _menuWidth.v;
        public override IReadOnlyList<TA> MenuOneItems => items;

        public AnalysisAggregate<TA, TT, TE> AggregateAnalysis
        {
            get => _aggregateAnalysis;
            protected set => _aggregateAnalysis = value;
        }

        public virtual IEnumerator RegisterPreferences(IPreferencesDrawer drawer)
        {
            drawer.RegisterFilterPref(OnlyShowIssues);

            yield return null;

            drawer.RegisterFilterPref(IssueType, () => OnlyShowIssues.v);

            yield return null;

            drawer.RegisterFilterPref(GenerateTestFiles);

            yield return null;

            drawer.RegisterFilterPref(_menuWidth);

            yield return null;
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

        public void ResetAnalysis()
        {
            AggregateAnalysis = null;
        }

        public void ValidateSummaryProperties()
        {
            using (_PRF_ValidateSummaryProperties.Auto())
            {
                RecreateAnalysisResults();

                for (var index = 0; index < MenuOneItems.Count; index++)
                {
                    var analysis = MenuOneItems[index];

                    analysis.OnReanalyzeNecessary -= RecreateAnalysisResults;
                    analysis.OnReanalyzeNecessary += RecreateAnalysisResults;
                }
            }
        }

        protected override IEnumerable<AppaProgress> OnPreInitialize(AppaProgressCounter pc)
        {
            yield return pc.Get(ZString.Format("{0}: {1}", AppaProgress.REGISTERING, APPASTR.Only_Issues), 1);

            _onlyShowIssues = PREFS.REG(GetPreferencePrefix, APPASTR.Only_Issues, true);

            yield return pc.Get(
                ZString.Format("{0}: {1}", AppaProgress.REGISTERING, APPASTR.Issue_Type_Assembly),
                1
            );

            _issueType = PREFS.REG(
                GetPreferencePrefix,
                ZString.Format("{0} {1}", APPASTR.Issue_Type, IssueTypeName),
                default(TE)
            );

            yield return pc.Get(ZString.Format("{0}: {1}", AppaProgress.REGISTERING, APPASTR.Test_Files), 1);

            _generateTestFiles = PREFS.REG(GetPreferencePrefix, APPASTR.Test_Files, true);

            yield return pc.Get(ZString.Format("{0}: {1}", AppaProgress.REGISTERING, APPASTR.Menu_Width), 1);

            _menuWidth = PREFS.REG(GetPreferencePrefix, APPASTR.Menu_Width, 300f, 150f, 500f);
        }

        protected override void OnReset()
        {
            using (_PRF_OnReset.Auto())
            {
                AggregateAnalysis?.Reset();

                items = null;
            }
        }

        private IEnumerable<IEnumerable<AnalysisType<TA, TT, TE>>> GetAnalysisResults()
        {
            return MenuOneItems.Select(
                analysis =>
                {
                    if (analysis == null)
                    {
                        return null;
                    }

                    var target = analysis.Target;

                    if ((target != null) && ShouldShowInMenu(analysis))
                    {
                        return analysis.AllTypes;
                    }

                    return null;
                }
            );
        }

        private void RecreateAnalysisResults()
        {
            _aggregateAnalysis = new AnalysisAggregate<TA, TT, TE>();

            var allResults = GetAnalysisResults();

            _aggregateAnalysis.AddAll(allResults);
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AnalysisMenuContext<TT, TA, TE>) + ".";

        private static readonly ProfilerMarker _PRF_ValidateMenuSelection =
            new(_PRF_PFX + nameof(ValidateMenuSelection));

        #endregion

        #region Tracing

        private const string _TRACE_PFX = nameof(AnalysisMenuContext<TT, TA, TE>) + ".";

        #endregion
    }
}
