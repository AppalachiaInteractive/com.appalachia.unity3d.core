#region

using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Behaviours
{
    [Serializable, ExecuteInEditMode]
    [InspectorIcon(Icons.Squirrel.DarkYellow)]
    public abstract class SingletonAppalachiaBehaviour<T> : AppalachiaBehaviour
        where T : SingletonAppalachiaBehaviour<T>
    {
        #region Static Fields and Autoproperties

        private static T _singletonInstance;

        #endregion

        public static T instance
        {
            get
            {
                if (_singletonInstance != null)
                {
                    return _singletonInstance;
                }

                var gameObject = new GameObject(nameof(T));

                var i = gameObject.AddComponent<T>();

                SetInstance(i);
                
                return _singletonInstance;
            }
        }

        protected virtual bool DestroyObjectOfSubsequentInstances => false;

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();

                SetInstance(this as T);
                _singletonInstance.SetDirty();

#if !UNITY_EDITOR
                DontDestroyOnLoad(this);
#endif
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                SetInstance(this as T);
                _singletonInstance.SetDirty();

#if !UNITY_EDITOR
                DontDestroyOnLoad(this);
#endif
            }
        }

        #endregion

        private static void SetInstance(T checkingFrom)
        {
            var hasExistingInstance = _singletonInstance != null;
            var wasProvidedInstance = checkingFrom != null;
            var isCheckingFromMainInstance = checkingFrom == _singletonInstance;

            if (hasExistingInstance)
            {
                if (isCheckingFromMainInstance)
                {
                    return;
                }

                if (wasProvidedInstance)
                {
                    if (checkingFrom.DestroyObjectOfSubsequentInstances)
                    {
                        checkingFrom.gameObject.DestroySafely();
                    }
                    else
                    {
                        checkingFrom.DestroySafely();
                    }
                }

                return;
            }

            if (wasProvidedInstance)
            {
                _singletonInstance = checkingFrom;
                return;
            }

            _singletonInstance = FindObjectOfType<T>();

            hasExistingInstance = _singletonInstance != null;

            if (hasExistingInstance)
            {
                return;
            }

            var go = new GameObject(typeof(T).GetSimpleReadableName());
            _singletonInstance = go.AddComponent<T>();
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SingletonAppalachiaBehaviour<T>) + ".";

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));

        #endregion

#if UNITY_EDITOR

        public new const string TITLE = APPASTR.SINGLETON_BEHAVIOUR;

        public new const string TITLECOLOR = Utility.Colors.Colors.Appalachia.HEX.DarkYellow;
        public new const string TITLEICON = "";

        protected override string GetTitle()
        {
            return TITLE;
        }

        protected override string GetTitleColor()
        {
            return TITLECOLOR;
        }

        protected override string GetTitleIcon()
        {
            return TITLEICON;
        }

        protected override string GetGameObjectIcon()
        {
            return GAMEOBJECTICON;
        }

        protected override string GetSubtitleColor()
        {
            return TITLECOLOR;
        }

#endif
    }
}
