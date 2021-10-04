#region

using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Collections;
using Appalachia.Core.Collections.Extensions;
using Appalachia.Core.Collections.NonSerialized;
using Appalachia.Filtering;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Editing.Framing
{
    public static class FramingUtility
    {

        private const float _quaternionRotationCheck = 1023.5f / 1024.0f;

        private static NonSerializedList<GameObject> _lastFramed;
        private static NonSerializedList<Vector3> _framingVectorForwards;
        private static NonSerializedList<Vector3> _framingVectorUps;
        
        private static readonly Func<Camera, Transform, FramingDirection, Vector3> _basicLook = (c, t, dir) =>
        {
            switch (dir)
            {
                case FramingDirection.Existing:
                    return c.transform.forward;
                case FramingDirection.Opposite:
                    return -c.transform.forward;
                case FramingDirection.Front:
                    return -t.forward;
                case FramingDirection.Right:
                    return -t.right;
                case FramingDirection.Top:
                    return -t.up;
                case FramingDirection.Back:
                    return t.forward;
                case FramingDirection.Left:
                    return t.right;
                case FramingDirection.Bottom:
                    return t.up;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        };

        public static void Frame(
            this GameObject go,
            Bounds bounds = default,
            bool adjustAngle = true,
            FramingDirection direction = FramingDirection.Existing)
        {
            var up = direction == FramingDirection.Top
                ? Vector3.forward
                : direction == FramingDirection.Bottom
                    ? Vector3.back
                    : Vector3.up;

            Frame(go, bounds, adjustAngle, direction, _basicLook, up);
        }

        public static void Frame(this GameObject[] gos, bool adjustAngle = true, FramingDirection direction = FramingDirection.Existing)
        {
            var up = direction == FramingDirection.Top
                ? Vector3.forward
                : direction == FramingDirection.Bottom
                    ? Vector3.back
                    : Vector3.up;

            Frame(gos, adjustAngle, direction, _basicLook, up);
        }

        public static void Frame(this IEnumerable<GameObject> gos, bool adjustAngle = true, FramingDirection direction = FramingDirection.Existing)
        {
            var up = direction == FramingDirection.Top
                ? Vector3.forward
                : direction == FramingDirection.Bottom
                    ? Vector3.back
                    : Vector3.up;

            Frame(gos, adjustAngle, direction, _basicLook, up);
        }

        public static void Frame(this AppaList<GameObject> gos, bool adjustAngle = true, FramingDirection direction = FramingDirection.Existing)
        {
            var up = direction == FramingDirection.Top
                ? Vector3.forward
                : direction == FramingDirection.Bottom
                    ? Vector3.back
                    : Vector3.up;

            Frame(gos, adjustAngle, direction, _basicLook, up);
        }

        private static void Frame(
            this GameObject go,
            Bounds bounds,
            bool adjustAngle,
            FramingDirection direction,
            Func<Camera, Transform, FramingDirection, Vector3> look,
            Vector3 up)
        {
            InitializeState(out var view, out var viewCamera, out var viewCameraTransform);
            _lastFramed.Add(go);

            var rotation = viewCameraTransform.rotation;

            if (adjustAngle)
            {
                var lookDirection = look(viewCamera, go.transform, direction);
                rotation = Quaternion.LookRotation(lookDirection, up);
            }

            if (bounds == default)
            {
                bounds = FramingUtility.GetRenderingBounds(go);
            }

            var target = bounds.center;
            var size = bounds.size.y;

            view.LookAt(target, rotation, size);
        }

        private static void Frame(
            this AppaList<GameObject> gos,
            bool adjustAngle,
            FramingDirection direction,
            Func<Camera, Transform, FramingDirection, Vector3> look,
            Vector3 up)
        {
            InitializeState(out var view, out var viewCamera, out var viewCameraTransform);
            _lastFramed.AddRange(gos);

            var rotation = viewCameraTransform.rotation;
            var bounds = new Bounds();

            for (var i = 0; i < gos.Count; i++)
            {
                var go = gos[i];

                MultiFrameIteration(go, adjustAngle, direction, viewCamera, look, up, ref bounds);
            }

            FinalizeMultiFrame(view, adjustAngle, rotation, bounds);
        }

        private static void Frame(
            this IEnumerable<GameObject> gos,
            bool adjustAngle,
            FramingDirection direction,
            Func<Camera, Transform, FramingDirection, Vector3> look,
            Vector3 up)
        {
            InitializeState(out var view, out var viewCamera, out var viewCameraTransform);
            var goArray = gos.ToArray();

            _lastFramed.AddRange(goArray);

            var rotation = viewCameraTransform.rotation;
            var bounds = new Bounds();

            for (var i = 0; i < goArray.Length; i++)
            {
                var go = goArray[i];

                MultiFrameIteration(go, adjustAngle, direction, viewCamera, look, up, ref bounds);
            }

            FinalizeMultiFrame(view, adjustAngle, rotation, bounds);
        }

        private static void Frame(
            this GameObject[] gos,
            bool adjustAngle,
            FramingDirection direction,
            Func<Camera, Transform, FramingDirection, Vector3> look,
            Vector3 up)
        {
            InitializeState(out var view, out var viewCamera, out var viewCameraTransform);
            _lastFramed.AddRange(gos);

            var rotation = viewCameraTransform.rotation;
            var bounds = new Bounds();

            for (var i = 0; i < gos.Length; i++)
            {
                var go = gos[i];

                MultiFrameIteration(go, adjustAngle, direction, viewCamera, look, up, ref bounds);
            }

            FinalizeMultiFrame(view, adjustAngle, rotation, bounds);
        }

        private static void InitializeState(out SceneView view, out Camera viewCamera, out Transform viewCameraTransform)
        {
            if (_lastFramed == null)
            {
                _lastFramed = new NonSerializedList<GameObject>();
            }

            _lastFramed.ClearFast();

            if (_framingVectorForwards == null)
            {
                _framingVectorForwards = new NonSerializedList<Vector3>(12);
            }

            _framingVectorForwards.ClearFast();

            if (_framingVectorUps == null)
            {
                _framingVectorUps = new NonSerializedList<Vector3>(12);
            }

            _framingVectorUps.ClearFast();

            view = SceneView.lastActiveSceneView;
            viewCamera = view.camera;
            viewCameraTransform = viewCamera.transform;
        }
        
        private static void MultiFrameIteration(
            GameObject go,
            bool adjustAngle,
            FramingDirection direction,
            Camera viewCamera, 
            Func<Camera, Transform, FramingDirection, Vector3> look,
            Vector3 up,
            ref Bounds bounds)
        {
            if (adjustAngle)
            {
                var lookDirection = look(viewCamera, go.transform, direction);
                _framingVectorForwards.Add(lookDirection);
                _framingVectorUps.Add(up);
            }

            bounds.Encapsulate(FramingUtility.GetRenderingBounds(go));
        }

        private static void FinalizeMultiFrame(SceneView view, bool adjustAngle, Quaternion rotation, Bounds bounds)
        {
            if (adjustAngle)
            {
                var forward = _framingVectorForwards.Average_NoAlloc();
                var up = _framingVectorUps.Average_NoAlloc();

                rotation = Quaternion.LookRotation(forward, up);
            }

            var target = bounds.center;
            var size = bounds.size.y;

            view.LookAt(target, rotation, size);
        }

        public static void FRAME_MENU(KeyCode keyCode, bool control)
        {
            var objs = Selection.gameObjects;
            
            var direction = keyCode == KeyCode.Keypad1
                ? control ? FramingDirection.Back : FramingDirection.Front
                : keyCode == KeyCode.Keypad3
                    ? control ? FramingDirection.Right :FramingDirection.Left
                    : keyCode == KeyCode.Keypad7
                        ? control ? FramingDirection.Bottom :FramingDirection.Top
                        : keyCode == KeyCode.Keypad9
                        ? FramingDirection.Opposite
                        : FramingDirection.Existing;
            
            if (objs == null || objs.Length == 0)
            {
                Frame(_lastFramed, true, direction);
            }
            else
            {
                Frame(objs, true, direction);
            }
        }

        public static Bounds GetRenderingBounds(this GameObject gameObject)
        {
            var renderers = gameObject.FilterComponents<Renderer>(true).RunFilter();
            var colliders = gameObject.FilterComponents<Collider>(true).NoTriggers().RunFilter();

            var bounds = new Bounds {center = gameObject.transform.position, size = Vector3.zero};

            foreach (var renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            foreach (var collider in colliders)
            {
                bounds.Encapsulate(collider.bounds);
            }

            return bounds;
        }

    }
}