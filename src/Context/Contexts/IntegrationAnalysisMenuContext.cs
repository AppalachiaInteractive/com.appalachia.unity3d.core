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
    public abstract class IntegrationAnalysisMenuContext<TT, TA, TE> : AnalysisMenuContext<TT, TA, TE>
        where TT : IntegrationMetadata<TT>
        where TA : AnalysisMetadata<TA, TT, TE>, new()
        where TE : Enum
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(IntegrationAnalysisMenuContext<TT, TA, TE>) + ".";

        private const string _TRACE_PFX = nameof(IntegrationAnalysisMenuContext<TT, TA, TE>) + ".";

        private static readonly ProfilerMarker _PRF_OnInitialize = new(_PRF_PFX + nameof(OnInitialize));

        private static readonly TraceMarker _TRACE_OnInitialize = new(_TRACE_PFX + nameof(OnInitialize));

        private static readonly ProfilerMarker _PRF_ShouldShowInMenu =
            new(_PRF_PFX + nameof(ShouldShowInMenu));

        #endregion

        #region Preferences

        private PREF<bool> _appalachiaOnly;
        private PREF<bool> _assetsOnly;

        public PREF<bool> AppalachiaOnly => _appalachiaOnly;

        public PREF<bool> AssetsOnly => _assetsOnly;

        #endregion

        protected override string GetPreferencePrefix => APPASTR.PREF.CI.Package_Review;

        public override string GetMenuDisplayName(TA item)
        {
            return item.Target.Name;
        }

        public override bool ShouldShowInMenu(TA analysis)
        {
            using (_PRF_ShouldShowInMenu.Auto())
            {
                var baseResult = base.ShouldShowInMenu(analysis);

                if (!baseResult)
                {
                    return false;
                }

                if (analysis.Target.readOnly)
                {
                    return false;
                }

                if (_appalachiaOnly.Value && !analysis.Target.IsAppalachia)
                {
                    return false;
                }

                if (_assetsOnly.Value && !analysis.Target.IsAsset)
                {
                    return false;
                }

                return true;
            }
        }

        protected override IEnumerable<AppaProgress> OnInitialize(AppaProgressCounter p)
        {
            using (_TRACE_OnInitialize.Auto())
            using (_PRF_OnInitialize.Auto())
            {
                var progress = p.Current;

                yield return p.Get(AppaProgress.FINDING, ref progress, 10);

                var instances = IntegrationMetadata.FindAll<TT>();

                yield return p.Get(AppaProgress.INITIALIZING, ref progress, 10);

                if (items == null)
                {
                    items = new List<TA>();
                }
                else
                {
                    items.Clear();
                }

                yield return p.Get(AppaProgress.CHECKING, ref progress, 10);

                foreach (var instance in instances)
                {
                    if (instance == null)
                    {
                        continue;
                    }

                    p.Increment(ref progress, 20, instances);

                    yield return p.Get($"{AppaProgress.CHECKING}: {instance.Name}", progress);

                    var analysis = new TA();
                    analysis.SetTarget(instance);

                    items.Add(analysis);
                }

                items.Sort((a, b) => a.Target.CompareTo(b.Target));

                foreach (var instance in instances)
                {
                    p.Increment(ref progress, 20, instances);

                    yield return p.Get($"{AppaProgress.SETTING}: {instance.Name}", progress);

                    if (!instance.readOnly)
                    {
                        instance.InitializeForAnalysis();
                    }
                }

                yield return p.Get(AppaProgress.VALIDATING, ref progress, 10);

                ValidateSummaryProperties();

                foreach (var analysis in MenuOneItems)
                {
                    yield return p.Get($"{AppaProgress.ANALYZING}: {analysis.Target.Name}", progress);

                    analysis.Analyze();

                    p.Increment(ref progress, 20, MenuOneItems);
                }
            }
        }

        protected override IEnumerable<AppaProgress> OnPreInitialize(AppaProgressCounter pc)
        {
            foreach (var baseResult in base.OnPreInitialize(pc))
            {
                yield return baseResult;
            }

            yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Appalachia_Only}", 1);

            _appalachiaOnly = PREFS.REG(APPASTR.PREF.CI.Package_Review, APPASTR.Appalachia_Only, true);

            yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Assets_Only}", 1);

            _assetsOnly = PREFS.REG(APPASTR.PREF.CI.Package_Review, APPASTR.Assets_Only, true);
        }

        public override IEnumerator RegisterPreferences(IPreferencesDrawer drawer)
        {
            var baseResults = base.RegisterPreferences(drawer);

            while (baseResults.MoveNext())
            {
                yield return baseResults.Current;
            }
            
            drawer.RegisterFilterPref(AppalachiaOnly);

            yield return null;

            drawer.RegisterFilterPref(AssetsOnly);

            yield return null;
        }
    }
}