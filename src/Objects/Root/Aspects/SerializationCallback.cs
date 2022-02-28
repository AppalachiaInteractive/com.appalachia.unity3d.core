using System;
using System.Collections.Generic;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Standards;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject : ISerializationCallbackReceiver
    {
        #region ISerializationCallbackReceiver Members

        public virtual void OnBeforeSerialize()
        {
            using (_PRF_OnBeforeSerialize.Auto())
            {
                using var scope = APPASERIALIZE.OnBeforeSerialize();

                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }
            }
        }

        public virtual void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                using var scope = APPASERIALIZE.OnAfterDeserialize();

                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnAfterDeserialize));

        #endregion
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

    public partial class AppalachiaBehaviour : ISerializationCallbackReceiver
    {
        #region ISerializationCallbackReceiver Members

        public virtual void OnBeforeSerialize()
        {
            using (_PRF_OnBeforeSerialize.Auto())
            {
                using var scope = APPASERIALIZE.OnBeforeSerialize();

                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }
            }
        }

        public virtual void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                using var scope = APPASERIALIZE.OnAfterDeserialize();

                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnAfterDeserialize));

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

    public partial class AppalachiaBase : ISerializationCallbackReceiver
    {
        #region ISerializationCallbackReceiver Members

        public virtual void OnBeforeSerialize()
        {
            using (_PRF_OnBeforeSerialize.Auto())
            {
                using var scope = APPASERIALIZE.OnBeforeSerialize();

                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }
            }
        }

        public virtual void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                using var scope = APPASERIALIZE.OnAfterDeserialize();

                if (!AppalachiaApplication.OnMainThread)
                {
                    return;
                }
                
                if (_owner == null)
                {
                    //Context.Log.Warn($"The {GetType().FormatForLogging()} has no owner!", this);
                }
                
                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }

                if (_hasBeenInitialized)
                {
                    return;
                }

                _initializationFunctions ??= new Queue<Func<AppaTask>>();
                _initializationFunctions.Enqueue(HandleInitialization);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnBeforeSerialize));

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnAfterDeserialize));

        #endregion
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

    public partial class AppalachiaSelectable<T> : ISerializationCallbackReceiver
    {
        #region ISerializationCallbackReceiver Members

        public virtual void OnBeforeSerialize()
        {
            using (_PRF_OnBeforeSerialize.Auto())
            {
                using var scope = APPASERIALIZE.OnBeforeSerialize();

                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }
            }
        }

        public virtual void OnAfterDeserialize()
        {
            using (_PRF_OnAfterDeserialize.Auto())
            {
                using var scope = APPASERIALIZE.OnAfterDeserialize();

                if (_objectId == null)
                {
                    _objectId = ObjectId.NewObjectId();
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnAfterDeserialize =
            new ProfilerMarker(typeof(T).Name + "." + nameof(OnAfterDeserialize));

        private static readonly ProfilerMarker _PRF_OnBeforeSerialize =
            new ProfilerMarker(typeof(T).Name + "." + nameof(OnBeforeSerialize));

        #endregion
    }
}
