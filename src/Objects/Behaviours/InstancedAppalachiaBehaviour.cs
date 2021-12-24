#region

using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Root;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Appalachia.Core.Objects.Behaviours
{
    [InspectorIcon(Brand.InstancedAppalachiaBehaviour.Icon)]
    public abstract class InstancedAppalachiaBehaviour<T> : AppalachiaBehaviour<T>
        where T : InstancedAppalachiaBehaviour<T>
    {
        #region Fields and Autoproperties

        protected bool checkedForRenderers;

        [NonSerialized] private MaterialPropertyBlock[] _materialPropertyBlocks;

        [SerializeField]
        [HideInInspector]
        private MeshRenderer[] _meshRenderers;

        #endregion

        protected MaterialPropertyBlock[] materialPropertyBlocks
        {
            get
            {
                var mr = meshRenderers;

                if ((mr != null) &&
                    ((_materialPropertyBlocks == null) ||
                     (_materialPropertyBlocks.Length != meshRenderers.Length)))
                {
                    _materialPropertyBlocks = new MaterialPropertyBlock[meshRenderers.Length];

                    for (var i = 0; i < _materialPropertyBlocks.Length; i++)
                    {
                        _materialPropertyBlocks[i] = new MaterialPropertyBlock();
                        mr[i].GetPropertyBlock(_materialPropertyBlocks[i]);
                    }
                }

                return _materialPropertyBlocks;
            }
        }

        protected MeshRenderer[] meshRenderers
        {
            get
            {
                if ((_meshRenderers == null) || ((_meshRenderers.Length == 0) && !checkedForRenderers))
                {
                    _meshRenderers = GetComponentsInChildren<MeshRenderer>();
                    checkedForRenderers = true;
                }

                return _meshRenderers;
            }
        }

        [Button]
        public void UpdateAllInstancedProperties()
        {
            try
            {
                var mrs = meshRenderers;
                var bs = materialPropertyBlocks;

                for (var i = 0; i < bs.Length; i++)
                {
                    var b = bs[i];
                    var mr = mrs[i];

                    var mats = mr.sharedMaterials;

                    if (mats.Length == 0)
                    {
                        continue;
                    }

                    var mat = mats[0];

                    var shader = mat.shader;

                    if (b == null)
                    {
                        continue;
                    }

                    UpdateInstancedProperties(b, mat);

                    mr.SetPropertyBlock(b);
                }
            }
            catch (MissingReferenceException)
            {
                _meshRenderers = null;
                _materialPropertyBlocks = null;
            }
        }

        protected abstract void UpdateInstancedProperties(MaterialPropertyBlock block, Material m);

        protected override string GetBackgroundColor()
        {
            return Brand.InstancedAppalachiaBehaviour.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.InstancedAppalachiaBehaviour.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.InstancedAppalachiaBehaviour.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.InstancedAppalachiaBehaviour.Color;
        }
    }
}
