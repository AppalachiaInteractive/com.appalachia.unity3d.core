using System;
using System.Collections;
using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Aspects.Tracing;
using Appalachia.Core.Context.Analysis.Core;
using Appalachia.Core.Context.Elements.Progress;
using Appalachia.Core.Context.Interfaces;
using Appalachia.Core.Preferences;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.Context.Contexts
{
    public abstract class IntegrationAnalysisMenuContext<TT, TA, TE> : AnalysisMenuContext<TT, TA, TE>
        where TT : IntegrationMetadata<TT>
        where TA : AnalysisGroup<TA, TT, TE>, new()
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

        private PREF<IntegrationTypeFlags> _hideFlags;
        public PREF<IntegrationTypeFlags> HideFlags => _hideFlags;
            
        //private PREF<bool> _appalachiaOnly;
        //private PREF<bool> _appalachiaManagedOnly;
        //private PREF<bool> _assetsOnly;
        //public PREF<bool> AppalachiaOnly => _appalachiaOnly;
        //public PREF<bool> AppalachiaManagedOnly => _appalachiaManagedOnly;
        //public PREF<bool> AssetsOnly => _assetsOnly;

        #endregion

        protected override string GetPreferencePrefix => APPASTR.PREF.CI.Package_Review + "/" + typeof(TT).Name + "/";

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

                if (analysis.Target.flags.HasAny(_hideFlags.Value))
                {
                    return false;
                }

                /*
                if (_appalachiaManagedOnly.Value && !analysis.Target.IsAppalachiaManaged)
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
                }*/

                return true;
            }
        }

        public override IEnumerator RegisterPreferences(IPreferencesDrawer drawer)
        {
            drawer.RegisterFilterPref(HideFlags);
            yield return null;
            
            /*drawer.RegisterFilterPref(AppalachiaManagedOnly);
            
            yield return null;
            
            drawer.RegisterFilterPref(AppalachiaOnly);

            yield return null;

            drawer.RegisterFilterPref(AssetsOnly);

            yield return null;*/

            var baseResults = base.RegisterPreferences(drawer);

            while (baseResults.MoveNext())
            {
                yield return baseResults.Current;
            }
        }

        public override string GetMenuDisplayName(TA item)
        {
            return item.Target.Name;
        }

        protected override IEnumerable<AppaProgress> OnInitialize(AppaProgressCounter p)
        {
            using (_TRACE_OnInitialize.Auto())
            using (_PRF_OnInitialize.Auto())
            {
                var progress = p.Current;
                using (_PRF_OnInitialize.Suspend())
                {
                    yield return p.Get(AppaProgress.FINDING, ref progress, 10);
                }

                var instances = IntegrationMetadata.FindAll<TT>();
                
                using (_PRF_OnInitialize.Suspend())
                {
                    yield return p.Get(AppaProgress.INITIALIZING, ref progress, 10);
                }

                if (items == null)
                {
                    items = new List<TA>();
                }
                else
                {
                    items.Clear();
                }

                using (_PRF_OnInitialize.Suspend())
                {
                    yield return p.Get(AppaProgress.CHECKING, ref progress, 10);
                }

                foreach (var instance in instances)
                {
                    if (instance == null)
                    {
                        continue;
                    }

                    p.Increment(ref progress, 20, instances);
                    
                    using (_PRF_OnInitialize.Suspend())
                    {
                        yield return p.Get($"{AppaProgress.CHECKING}: {instance.Name}", progress);
                    }

                    var analysis = AnalysisGroup<TA, TT, TE>.Create(instance);

                    items.Add(analysis);
                }

                items.Sort((a, b) => a.Target.CompareTo(b.Target));

                foreach (var instance in instances)
                {
                    p.Increment(ref progress, 20, instances);
                    
                    using (_PRF_OnInitialize.Suspend())
                    {
                        yield return p.Get($"{AppaProgress.SETTING}: {instance.Name}", progress);
                    }

                    if (!instance.readOnly)
                    {
                        instance.InitializeForAnalysis();
                    }
                }

                using (_PRF_OnInitialize.Suspend())
                {
                    yield return p.Get(AppaProgress.VALIDATING, ref progress, 10);
                }

                ValidateSummaryProperties();

                foreach (var analysis in MenuOneItems)
                {
                    using (_PRF_OnInitialize.Suspend())
                    {
                        yield return p.Get($"{AppaProgress.ANALYZING}: {analysis.Target.Name}", progress);
                    }

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

            yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Hide_Flags}", 1);

            _hideFlags = PREFS.REG(GetPreferencePrefix, APPASTR.Hide_Flags, IntegrationTypeFlags.None);
            
            //yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Appalachia_Only}", 1);

            //_appalachiaOnly = PREFS.REG(GetPreferencePrefix, APPASTR.Appalachia_Only, true);

            //yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Appalachia_Managed_Only}", 1);

            //_appalachiaManagedOnly = PREFS.REG(GetPreferencePrefix, APPASTR.Appalachia_Managed_Only, true);

            //yield return pc.Get($"{AppaProgress.REGISTERING}: {APPASTR.Assets_Only}", 1);

            //_assetsOnly = PREFS.REG(GetPreferencePrefix, APPASTR.Assets_Only, true);
        }
    }
}
