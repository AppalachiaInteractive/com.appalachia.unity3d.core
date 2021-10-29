using System;
using UnityEngine;

namespace Appalachia.Core.Volumes.Parameters
{
    [Serializable]
    public sealed class GradientParameter : AppaVolumeParameter<Gradient>
    {
        public GradientParameter(Gradient value, bool overrideState = false) : base(value, overrideState)
        {
            _colorKeys = new GradientColorKey[8];
            _alphaKeys = new GradientAlphaKey[8];
        }

        private GradientAlphaKey[] _alphaKeys;
        private GradientColorKey[] _colorKeys;

        // XXX: this is hardly efficient

        public override void Interp(Gradient from, Gradient to, float t)
        {
            var atTime = 0f;

            for (var i = 0; i < 8; ++i)
            {
                var fromColor = from.Evaluate(atTime);
                var toColor = to.Evaluate(atTime);
                var newColor = Color.Lerp(fromColor, toColor, t);

                _colorKeys[i] = new GradientColorKey {color = newColor, time = atTime};
                _alphaKeys[i] = new GradientAlphaKey {alpha = newColor.a, time = atTime};

                atTime += 0.125f;
            }

            m_Value.SetKeys(_colorKeys, _alphaKeys);
        }
    }
}
