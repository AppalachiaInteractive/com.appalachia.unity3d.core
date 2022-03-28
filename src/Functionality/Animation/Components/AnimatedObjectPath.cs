#if UNITY_EDITOR
using System;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Reflection.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Functionality.Animation.Components
{
    [Serializable]
    [HideReferenceObjectPicker]
    public class AnimatedObjectPath : AppalachiaSimpleBase
    {
        public AnimatedObjectPath(AnimationRemapper remapper, UnityEditor.EditorCurveBinding originalBinding)
        {
            OriginalBinding = originalBinding;

            this.remapper = remapper;

            _propertyName = originalBinding.propertyName;
            componentType = originalBinding.type;

            if (originalBinding.path != null)
            {
                var gameObjectTransform = remapper.transform.Editor_GetTransformFromRelativePath(originalBinding.path);

                if (gameObjectTransform != null)
                {
                    _gameObject = gameObjectTransform.gameObject;
                }
            }

            if ((_gameObject != null) && (remapper != null))
            {
                var objComponent = UnityEditor.AnimationUtility.GetAnimatedObject(remapper.gameObject, originalBinding);

                _component = objComponent as Component;
            }
        }

        #region Fields and Autoproperties

        public UnityEditor.EditorCurveBinding OriginalBinding { get; }

        [SerializeField, HideInInspector]
        private AnimationRemapper remapper;

        [GUIColor(nameof(componentColor))]
        [SerializeField, PropertyOrder(10), HorizontalGroup("A"), OnValueChanged(nameof(OnComponentChanged))]
        [HideLabel, InfoBox("Component", InfoMessageType.None)]
        [EnableIf(nameof(EnableComponentField))]
        private Component _component;

        [GUIColor(nameof(gameObjectColor))]
        [SerializeField, PropertyOrder(0), HorizontalGroup("A"), OnValueChanged(nameof(OnGameObjectChanged))]
        [HideLabel, InfoBox("Game Object", InfoMessageType.None)]
        private GameObject _gameObject;

        [SerializeField, HideInInspector]
        private string _componentAssemblyQualifiedName;

        [SerializeField, PropertyOrder(20), HorizontalGroup("A")]
        [EnableIf(nameof(EnablePropertyNameField))]
        [HideLabel, InfoBox("Property Name", InfoMessageType.None)]
        private string _propertyName;

        [NonSerialized] private Type _componentType;

        #endregion

        public bool ShouldUpdate =>
            (OriginalBinding.path != TransformPath) ||
            (OriginalBinding.type != ComponentType) ||
            (OriginalBinding.propertyName != PropertyName);

        public GameObject GameObject => _gameObject;
        public Object Component => _component;

        public string PropertyName => _propertyName;

        public string TransformPath => _gameObject.transform.GetPathRelativeTo(remapper.transform);
        public Type ComponentType => componentType;

        private bool EnableComponentField => _gameObject != null;
        private bool EnablePropertyNameField => EnableComponentField && (_component != null);
        private bool ShowComponentError => _component == null;

        private bool ShowGameObjectError => _gameObject == null;
        private Color componentColor => _component == null ? Colors.Red3 : Color.white;

        private Color gameObjectColor => _gameObject == null ? Colors.Red3 : Color.white;

        private Type componentType
        {
            get
            {
                using (_PRF_componentType.Auto())
                {
                    if (_componentType == null)
                    {
                        _componentType = Type.GetType(_componentAssemblyQualifiedName);
                    }

                    return _componentType;
                }
            }
            set
            {
                using (_PRF_componentType.Auto())
                {
                    _componentType = value;
                    _componentAssemblyQualifiedName = _componentType.AssemblyQualifiedName;
                }
            }
        }

        public UnityEditor.EditorCurveBinding ToCurveBinding()
        {
            using (_PRF_ToCurveBinding.Auto())
            {
                return UnityEditor.EditorCurveBinding.FloatCurve(TransformPath, ComponentType, PropertyName);
            }
        }

        private void OnComponentChanged()
        {
            using (_PRF_OnComponentChanged.Auto())
            {
                if (_component != null)
                {
                    var actualComponentType = _component.GetType();
                    if (actualComponentType != componentType)
                    {
                        componentType = actualComponentType;
                    }

                    var componentFields = componentType.GetFields_CACHE();

                    var propertyFound = false;

                    for (var fieldIndex = 0; fieldIndex < componentFields.Length; fieldIndex++)
                    {
                        var componentField = componentFields[fieldIndex];

                        if (componentField.Name == PropertyName)
                        {
                            propertyFound = true;
                            break;
                        }
                    }

                    if (!propertyFound)
                    {
                        _propertyName = null;
                    }
                }
            }
        }

        private void OnGameObjectChanged()
        {
            using (_PRF_OnGameObjectChanged.Auto())
            {
                if (_gameObject != null)
                {
                    if (_component == null)
                    {
                        if (componentType != null)
                        {
                            _component = _gameObject.GetComponent(componentType);
                        }
                    }
                    else
                    {
                        if (_component.gameObject != _gameObject)
                        {
                            _component = _gameObject.GetComponent(componentType);
                        }
                    }
                }
                else
                {
                    _component = null;
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AnimatedObjectPath) + ".";

        private static readonly ProfilerMarker _PRF_componentType =
            new ProfilerMarker(_PRF_PFX + nameof(componentType));

        private static readonly ProfilerMarker _PRF_OnComponentChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnComponentChanged));

        private static readonly ProfilerMarker _PRF_OnGameObjectChanged =
            new ProfilerMarker(_PRF_PFX + nameof(OnGameObjectChanged));

        private static readonly ProfilerMarker _PRF_ToCurveBinding =
            new ProfilerMarker(_PRF_PFX + nameof(ToCurveBinding));

        #endregion
    }
}

#endif
