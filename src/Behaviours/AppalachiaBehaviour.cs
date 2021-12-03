using System;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Framing;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Behaviours
{
    [Serializable, DoNotReorderFields, ExecuteInEditMode]
    [InspectorIcon(Icons.Squirrel.Yellow)]
    public abstract class AppalachiaBehaviour : MonoBehaviour, IInitializable
    {
        #region Fields and Autoproperties

        [NonSerialized] private int _wakeFrame;
        public int WakeFrame => _wakeFrame;
        public int WakeDuration => Time.frameCount - _wakeFrame;
        
#if UNITY_EDITOR
        [SerializeField, HideLabel, InlineProperty, LabelWidth(0)]
        [SmartTitleGroup(
            BASE,
            "$" + nameof(GetTitle),
            "$" + nameof(GetSubtitle),
            true,
            TITLE_BOLD,
            false,
            titleColor: "$" + nameof(GetTitleColor),
            titleFont: TITLEFONT,
            subtitleColor: "$" + nameof(GetSubtitleColor),
            subtitleFont: SUBTITLEFONT,
            titleSize: TITLESIZE,
            subtitleSize: SUBTITLESIZE,
            titleIcon: "$" + nameof(GetTitleIcon),
            subtitleIcon: "$" + nameof(GetSubtitleIcon),
            backgroundColor: "$" + nameof(GetBackgroundColor),
            titleHeight: TITLEHEIGHT
        )]
        [SmartFoldoutGroup(GROUP, false, GROUPBACKGROUNDCOLOR, true, LABELCOLOR, true, CHILDCOLOR)]
#endif
        private Initializer _initializationData;

        private Bounds ___renderingBounds;

        private Transform ___transform;

        #endregion

        public Bounds renderingBounds
        {
            get
            {
                if (___renderingBounds == default)
                {
                    ___renderingBounds = gameObject.GetRenderingBounds(out _);
                }

                return ___renderingBounds;
            }
        }

        protected Initializer initializationData => _initializationData;

        protected Transform _transform
        {
            get
            {
                if (___transform == null)
                {
                    ___transform = transform;
                }

                return ___transform;
            }
        }

        #region Event Functions

        protected virtual void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                _wakeFrame = Time.frameCount;
                
                if (InitializeAlways || InitializeOnAwake)
                {
                    Initialize();
                }
            }
        }

        protected virtual void Reset()
        {
            ___renderingBounds = default;
            ___transform = default;

#if UNITY_EDITOR

            InitializeInEditor();
#endif
        }

        protected virtual void Start()
        {
            if (InitializeAlways || InitializeOnStart)
            {
                Initialize();
            }
        }
        
        
        protected virtual bool InitializeOnEnable => false;
        protected virtual bool InitializeOnAwake => false;
        protected virtual bool InitializeOnStart => false;
        protected virtual bool InitializeAlways => false;

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            var iconName = GetGameObjectIcon();
            var icon = AssetDatabaseManager.FindFirstAssetMatch<Texture2D>(iconName);
            UnityEditor.EditorGUIUtility.SetIconForObject(gameObject, icon);
#endif

            if (InitializeAlways || InitializeOnEnable)
            {
                Initialize();
            }
        }

        // ReSharper disable once Unity.RedundantEventFunction
        protected virtual void OnDisable()
        {
        }

        // ReSharper disable once Unity.RedundantEventFunction
        protected virtual void OnDestroy()
        {
        }

        #endregion

        // ReSharper disable once UnusedParameter.Global
        protected void DontDestroyOnLoadSafe(Object obj)
        {
#if !UNITY_EDITOR
            DontDestroyOnLoad(obj);
#endif
        }

        protected float3 LocalToWorldDirection(float3 direction)
        {
            return _transform.TransformDirection(direction);
        }

        protected float3 LocalToWorldPoint(float3 point)
        {
            return _transform.TransformPoint(point);
        }

        protected float3 LocalToWorldVector(float3 vector)
        {
            return _transform.TransformVector(vector);
        }

        protected void RecalculateBounds()
        {
            ___renderingBounds = default;
        }

        protected float3 WorldToLocalDirection(float3 direction)
        {
            return _transform.InverseTransformDirection(direction);
        }

        protected float3 WorldToLocalPoint(float3 point)
        {
            return _transform.InverseTransformPoint(point);
        }

        protected float3 WorldToLocalVector(float3 vector)
        {
            return _transform.InverseTransformVector(vector);
        }

        #region IInitializable Members

        protected virtual void Initialize()
        {
        }

        public void InitializeExternal()
        {
            using (_PRF_InitializeExternal.Auto())
            {
                Initialize();
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaBehaviour) + ".";
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + nameof(Start));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_InitializeExternal = new ProfilerMarker(_PRF_PFX + nameof(InitializeExternal));

        #endregion

#if UNITY_EDITOR
        protected const string GROUP = BASE + "/" + APPASTR.Internal;
        protected const string GROUP_BUTTONS = GROUP + "/" + APPASTR.Buttons;
        protected const string GROUP_WORKFLOW = SHOW_WORKFLOW + "/" + APPASTR.Workflow;
        protected const string GROUP_WORKFLOW_PROD = GROUP_WORKFLOW + "/" + APPASTR.Productivity;
        protected const string SHOW_WORKFLOW = GROUP + "/$ShowWorkflow";
        protected const string BASE = "BASE";

        public const string TITLE = APPASTR.BEHAVIOUR;
        public const string TITLECOLOR = Utility.Colors.Colors.Appalachia.HEX.RichYellow;
        public const string TITLEICON = "";
        public const string GAMEOBJECTICON = Icons.Squirrel.Outline;

        protected const string SUBTITLE = APPASTR.APPALACHIA_INTERACTIVE;
        protected const string SUBTITLECOLOR = Utility.Colors.Colors.Appalachia.HEX.Yellow;
        protected const string SUBTITLEICON = "";

        protected const bool TITLE_BOLD = false;

        protected const string TITLEFONT = APPASTR.Fonts.Montserrat.Medium;
        protected const string SUBTITLEFONT = APPASTR.Fonts.Montserrat.Medium;

        protected const int TITLESIZE = 13;
        protected const int SUBTITLESIZE = 13;
        protected const int TITLEHEIGHT = 24;

        protected const string LABELCOLOR = Utility.Colors.Colors.Appalachia.HEX.Bone;
        protected const string GROUPBACKGROUNDCOLOR = Utility.Colors.Colors.Appalachia.HEX.DarkYellow;
        protected const string CHILDCOLOR = Utility.Colors.Colors.Appalachia.HEX.LightYellow;
        protected const string BANNERBACKGROUNDCOLOR = Utility.Colors.Colors.Appalachia.HEX.Black;
#endif

#if UNITY_EDITOR

        protected virtual string GetTitle()
        {
            return TITLE;
        }

        protected virtual string GetSubtitle()
        {
            return SUBTITLE;
        }

        protected virtual string GetTitleColor()
        {
            return TITLECOLOR;
        }

        protected virtual string GetSubtitleColor()
        {
            return SUBTITLECOLOR;
        }

        protected virtual string GetTitleIcon()
        {
            return TITLEICON;
        }

        protected virtual string GetSubtitleIcon()
        {
            return SUBTITLEICON;
        }

        protected virtual string GetGameObjectIcon()
        {
            return GAMEOBJECTICON;
        }

        protected virtual string GetBackgroundColor()
        {
            return BANNERBACKGROUNDCOLOR;
        }
#endif

#if UNITY_EDITOR
        protected virtual void InitializeInEditor()
        {
        }

        private bool? _showButtons;

        private bool ShowButtons
        {
            get
            {
                if (!_showButtons.HasValue)
                {
                    _showButtons = !GetType().InheritsFrom(typeof(SingletonAppalachiaBehaviour<>));
                }

                return _showButtons.Value;
            }
        }

        [Button(ButtonSizes.Small)]
        [ButtonGroup(GROUP_BUTTONS)]
        [PropertyOrder(-1000)]
        [ShowIf(nameof(ShowButtons))]
        private void SelectAllInScene()
        {
            var type = GetType();

            var instances = FindObjectsOfType(type);

            UnityEditor.Selection.objects = instances;
        }

        [Button(ButtonSizes.Small)]
        [ButtonGroup(GROUP_BUTTONS)]
        [PropertyOrder(-1000)]
        [ShowIf(nameof(ShowButtons))]
        private void SelectObjectsInScene()
        {
            var type = GetType();

            // ReSharper disable once CoVariantArrayConversion
            Object[] instances = FindObjectsOfType(type)
                                .Select(
                                     o =>
                                     {
                                         if (o is Component c)
                                         {
                                             return c.gameObject;
                                         }

                                         return null;
                                     }
                                 )
                                .ToArray();

            UnityEditor.Selection.objects = instances;
        }

        public void Frame(bool recalculateBounds = false, bool adjustAngle = true)
        {
            if (recalculateBounds)
            {
                RecalculateBounds();

                var filters = GetComponentsInChildren<MeshFilter>();

                for (var i = 0; i < filters.Length; i++)
                {
                    var mf = filters[i];

                    if (mf != null)
                    {
                        var mesh = mf.sharedMesh;
                        mesh.RecalculateBounds();
                        mesh.UploadMeshData(false);
                    }
                }
            }

            gameObject.Frame(FrameTarget.SceneView, adjustAngle);
        }
#endif
    }
}
