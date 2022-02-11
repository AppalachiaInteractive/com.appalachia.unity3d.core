using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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

    public partial class AppalachiaBehaviour
    {
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

    public partial class AppalachiaSelectable<T>
    {
        protected Vector2 GetCurrentInputVector()
        {
            using (_PRF_GetCurrentInputVector.Auto())
            {
                var down = false;
                var up = false;
                var left = false;
                var right = false;
                var shiftHeld = false;

                for (var deviceIndex = 0; deviceIndex < InputSystem.devices.Count; deviceIndex++)
                {
                    if (down || up || left || right)
                    {
                        break;
                    }

                    var device = InputSystem.devices[deviceIndex];

                    if (!device.enabled)
                    {
                        continue;
                    }

                    if (device is Keyboard k)
                    {
                        down |= IsButtonControlPressed(k.downArrowKey) ||
                                IsButtonControlPressed(k.sKey) ||
                                IsButtonControlPressed(k.numpad2Key);

                        up |= IsButtonControlPressed(k.upArrowKey) ||
                              IsButtonControlPressed(k.wKey) ||
                              IsButtonControlPressed(k.numpad8Key);

                        left |= IsButtonControlPressed(k.leftArrowKey) ||
                                IsButtonControlPressed(k.aKey) ||
                                IsButtonControlPressed(k.numpad4Key);

                        right |= IsButtonControlPressed(k.rightArrowKey) ||
                                 IsButtonControlPressed(k.dKey) ||
                                 IsButtonControlPressed(k.numpad6Key);

                        shiftHeld |= k.shiftKey.isPressed;
                    }

                    if (device is Mouse m)
                    {
                        ProcessVector2Control(m.scroll, ref down, ref up, ref left, ref right, shiftHeld);
                    }

                    if (device is Joystick j)
                    {
                        ProcessStickControl(j.stick, ref down, ref up, ref left, ref right);
                        ProcessVector2Control(j.hatswitch, ref down, ref up, ref left, ref right, shiftHeld);
                    }

                    if (device is Gamepad g)
                    {
                        ProcessDpadControl(g.dpad, ref down, ref up, ref left, ref right);
                        ProcessStickControl(g.leftStick, ref down, ref up, ref left, ref right);
                    }
                }

                var x = left
                    ? -1
                    : right
                        ? 1
                        : 0;
                var y = down
                    ? -1
                    : up
                        ? 1
                        : 0;

                return new Vector2(x, y);
            }
        }

        private bool IsButtonControlPressed(ButtonControl control)
        {
            using (_PRF_IsPressed.Auto())
            {
                return control.isPressed || control.wasReleasedThisFrame || control.wasPressedThisFrame;
            }
        }

        private void ProcessDpadControl(
            DpadControl control,
            ref bool down,
            ref bool up,
            ref bool left,
            ref bool right)
        {
            using (_PRF_ProcessDpadControl.Auto())
            {
                down |= IsButtonControlPressed(control.down);
                up |= IsButtonControlPressed(control.up);
                left |= IsButtonControlPressed(control.left);
                right |= IsButtonControlPressed(control.right);
            }
        }

        private void ProcessStickControl(
            StickControl control,
            ref bool down,
            ref bool up,
            ref bool left,
            ref bool right)
        {
            using (_PRF_ProcessStickControl.Auto())
            {
                down |= IsButtonControlPressed(control.down);
                up |= IsButtonControlPressed(control.up);
                left |= IsButtonControlPressed(control.left);
                right |= IsButtonControlPressed(control.right);
            }
        }

        private void ProcessVector2Control(
            Vector2Control control,
            ref bool down,
            ref bool up,
            ref bool left,
            ref bool right,
            bool alternateToggled)
        {
            using (_PRF_ProcessVector2Control.Auto())
            {
                var controlVector = control.ReadValue();

                down |= !alternateToggled && (controlVector.y < 0f);
                up |= !alternateToggled && (controlVector.y > 0f);
                left |= (controlVector.x < 0f) || (alternateToggled && (controlVector.y < 0f));
                right |= (controlVector.x > 0f) || (alternateToggled && (controlVector.y > 0f));
            }
        }

        #region Profiling

        protected static readonly string _PRF_PFX6 = typeof(T).Name + ".";

        private static readonly ProfilerMarker _PRF_GetCurrentInputVector =
            new ProfilerMarker(_PRF_PFX6 + nameof(GetCurrentInputVector));

        private static readonly ProfilerMarker _PRF_IsPressed =
            new ProfilerMarker(_PRF_PFX6 + nameof(IsButtonControlPressed));

        private static readonly ProfilerMarker _PRF_ProcessDpadControl =
            new ProfilerMarker(_PRF_PFX6 + nameof(ProcessDpadControl));

        private static readonly ProfilerMarker _PRF_ProcessStickControl =
            new ProfilerMarker(_PRF_PFX6 + nameof(ProcessStickControl));

        private static readonly ProfilerMarker _PRF_ProcessVector2Control =
            new ProfilerMarker(_PRF_PFX6 + nameof(ProcessVector2Control));

        #endregion
    }
}
