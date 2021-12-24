#region

using System;
using Appalachia.Utility.Strings;
using Unity.Profiling;

#endregion

namespace Appalachia.Core.Aspects.Profiling
{
    public class ProfilerMarkerSet : DisposableAspectSet<ProfilerMarker>
    {
        private static readonly ProfilerMarker _marker_Create = new("ProfilerMarkerSet.Create");

        public override IDisposable Initiate(ProfilerMarker instance)
        {
            return instance.Auto();
        }

        protected override ProfilerMarker Create(
            string typePrefix,
            string memberName,
            string additive,
            int sourceLineNumber)
        {
            using (_marker_Create.Auto())
            {
                var markerName = ZString.Format(
                    "{0}.{1} {2} [{3}]",
                    typePrefix,
                    memberName,
                    additive == null ? string.Empty : ZString.Format("({0})", additive),
                    sourceLineNumber
                );

                var marker = new ProfilerMarker(markerName);

                return marker;
            }
        }
    }
}
