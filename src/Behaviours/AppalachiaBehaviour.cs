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
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Behaviours
{
    [Serializable, DoNotReorderFields, ExecuteInEditMode]
    [InspectorIcon(Icons.Squirrel.Yellow)]
    public abstract class AppalachiaBehaviour : MonoBehaviour, IInitializable
    {
        #region Constants and Static Readonly
        
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
        
        protected const string TITLEFONT = APPASTR.Fonts.Montserrat.Regular;
        protected const string SUBTITLEFONT = APPASTR.Fonts.Montserrat.Regular;
        
        protected const int TITLESIZE = 12;
        protected const int SUBTITLESIZE = 12;
        
        protected const string LABELCOLOR = Utility.Colors.Colors.Appalachia.HEX.Bone;
        protected const string GROUPBACKGROUNDCOLOR = Utility.Colors.Colors.Appalachia.HEX.DarkYellow;
        protected const string CHILDCOLOR = Utility.Colors.Colors.Appalachia.HEX.LightYellow;
        protected const string BANNERBACKGROUNDCOLOR = Utility.Colors.Colors.Appalachia.HEX.Black;
#endif
        
        #endregion
        
        #region Fields and Autoproperties

#if UNITY_EDITOR
        [SerializeField, HideLabel, InlineProperty, LabelWidth(0)]
        [SmartTitleGroup(
            BASE,
            "$"+nameof(GetTitle),
            "$"+nameof(GetSubtitle),
            TitleAlignment.Split,
            true,
            TITLE_BOLD,
            false,
            titleColor: "$"+nameof(GetTitleColor),
            titleFont: TITLEFONT,
            subtitleColor: "$"+nameof(GetSubtitleColor),
            subtitleFont: SUBTITLEFONT,
            titleSize: TITLESIZE,
            subtitleSize: SUBTITLESIZE,
            titleIcon: "$"+nameof(GetTitleIcon),
            subtitleIcon: "$"+nameof(GetSubtitleIcon),
            backgroundColor: "$"+nameof(GetBackgroundColor)
        )]
        [SmartFoldoutGroup(GROUP, false, GROUPBACKGROUNDCOLOR, true, LABELCOLOR, true, CHILDCOLOR)]
#endif
        private InitializationData _initializationData;

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

        protected InitializationData initializationData => _initializationData;

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
        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }
        
        // ReSharper disable once Unity.RedundantEventFunction
        protected virtual void OnDisable()
        {
        }

        // ReSharper disable once Unity.RedundantEventFunction
        protected virtual void OnDestroy()
        {
        }
        
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            var iconName = GetGameObjectIcon();
            var icon = AssetDatabaseManager.FindFirstAsset<Texture2D>(iconName);
            UnityEditor.EditorGUIUtility.SetIconForObject(gameObject, icon);
#endif
        }

        
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


        protected virtual void Reset()
        {
            ___renderingBounds = default;
            ___transform = default;
            
#if UNITY_EDITOR

            InitializeInEditor();
#endif
        }

#if UNITY_EDITOR
        protected virtual void InitializeInEditor()
        {
        }

        protected void SetDirty()
        {
            UnityEditor.EditorUtility.SetDirty(gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
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
        public virtual void Initialize()
        {
        }
    }
}
