using System;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject
    {
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
    }

    public partial class AppalachiaBehaviour : IUIBehaviour
    {
        #region Fields and Autoproperties

        [NonSerialized] protected RectTransform _cachedRectTransform;

        [NonSerialized] private bool _hasCachedRectTransform;

        #endregion

        #region IUIBehaviour Members

        public bool HasRectTransform
        {
            get
            {
                using (_PRF_HasRectTransform.Auto())
                {
                    return Transform is RectTransform;
                }
            }
        }

        public RectTransform RectTransform
        {
            get
            {
                using (_PRF_RectTransform.Auto())
                {
                    if (_hasCachedRectTransform)
                    {
                        return _cachedRectTransform;
                    }

                    if (_cachedRectTransform == null)
                    {
                        if (Transform is RectTransform transformCast)
                        {
                            _cachedRectTransform = transformCast;
                            _hasCachedRectTransform = true;
                        }
                        else
                        {
                            gameObject.GetOrAddComponent(ref _cachedRectTransform);
                            _hasCachedRectTransform = true;
                        }
                    }

                    return _cachedRectTransform;
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_HasRectTransform =
            new ProfilerMarker(_PRF_PFX + nameof(HasRectTransform));

        private static readonly ProfilerMarker _PRF_RectTransform =
            new ProfilerMarker(_PRF_PFX + nameof(RectTransform));

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase
    {
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T> : IUIBehaviour
    {
        #region Constants and Static Readonly

        protected static readonly string _PRF_PFX9 = typeof(T).Name + ".";

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] protected RectTransform _cachedRectTransform;

        [NonSerialized] private bool _hasCachedRectTransform;

        #endregion

        #region IUIBehaviour Members

        public bool HasRectTransform
        {
            get
            {
                using (_PRF_HasRectTransform.Auto())
                {
                    return Transform is RectTransform;
                }
            }
        }

        public RectTransform RectTransform
        {
            get
            {
                using (_PRF_RectTransform.Auto())
                {
                    if (_hasCachedRectTransform)
                    {
                        return _cachedRectTransform;
                    }

                    if (_cachedRectTransform == null)
                    {
                        if (Transform is RectTransform transformCast)
                        {
                            _cachedRectTransform = transformCast;
                            _hasCachedRectTransform = true;
                        }
                        else
                        {
                            gameObject.GetOrAddComponent(ref _cachedRectTransform);
                            _hasCachedRectTransform = true;
                        }
                    }

                    return _cachedRectTransform;
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_HasRectTransform =
            new ProfilerMarker(_PRF_PFX9 + nameof(HasRectTransform));

        private static readonly ProfilerMarker _PRF_RectTransform =
            new ProfilerMarker(_PRF_PFX9 + nameof(RectTransform));

        #endregion
    }
}
