#region

using System;
using System.Diagnostics;
using Appalachia.Core.Layers.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Layers
{
    [Serializable]
    [InlineProperty]
    public struct LayerSelection
    {
        private static ValueDropdownList<int> _layers;

        [ValueDropdown(nameof(GetLayers))]
        [HideLabel]
        [OnValueChanged(nameof(ResetMask))]
        [SerializeField]
        public int layer;

        private LayerMask mask;

        public LayerMask Mask
        {
            get
            {
                if (mask == default)
                {
                    mask = LayerMask.GetMask(LayerMask.LayerToName(layer));
                }

                return mask;
            }
        }

        private ValueDropdownList<int> GetLayers()
        {
            if (_layers != null)
            {
                return _layers;
            }

            _layers = new ValueDropdownList<int>();

            for (var i = 0; i < 32; i++)
            {
                var layerName = LayerMask.LayerToName(i);

                var text = $"{i}: {layerName}";
                _layers.Add(text, i);
            }

            return _layers;
        }

        [DebuggerStepThrough] public static implicit operator int(LayerSelection l)
        {
            return l.layer;
        }

        [DebuggerStepThrough] public static implicit operator LayerSelection(int l)
        {
            return new() {layer = l};
        }

        private void ResetMask()
        {
            mask = default;
        }

        public bool IsLayerInMask(int layer)
        {
            return mask.IsLayerInMask(layer);
        }
    }
}
