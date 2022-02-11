#region

using System;
using System.Diagnostics;
using Appalachia.Core.Objects.Layers.Extensions;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Layers
{
    [Serializable]
    [InlineProperty]
    public class LayerSelection : AppalachiaBase<LayerSelection>
    {
        #region Static Fields and Autoproperties

        private static ValueDropdownList<int> _layers;

        #endregion

        #region Fields and Autoproperties

        [ValueDropdown(nameof(GetLayers))]
        [HideLabel]
        [OnValueChanged(nameof(ResetMask))]
        [SerializeField]
        public int layer;

        private LayerMask mask;

        #endregion

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

        [DebuggerStepThrough]
        public static implicit operator int(LayerSelection l)
        {
            return l.layer;
        }

        [DebuggerStepThrough]
        public static implicit operator LayerSelection(int l)
        {
            return new() { layer = l };
        }

        public bool IsLayerInMask(int layer)
        {
            return mask.IsLayerInMask(layer);
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

                var text = ZString.Format("{0}: {1}", i, layerName);
                _layers.Add(text, i);
            }

            return _layers;
        }

        private void ResetMask()
        {
            mask = default;
        }
    }
}
