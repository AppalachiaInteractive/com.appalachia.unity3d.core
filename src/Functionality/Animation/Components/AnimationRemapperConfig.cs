using System;
using Appalachia.Core.ControlModel.Components;
using Appalachia.Core.Objects.Initialization;
using Object = UnityEngine.Object;

namespace Appalachia.Core.Functionality.Animation.Components
{
    [Serializable]
    public class AnimationRemapperConfig : AppaComponentConfig<AnimationRemapper, AnimationRemapperConfig>
    {
        public AnimationRemapperConfig()
        {
        }

        public AnimationRemapperConfig(Object owner) : base(owner)
        {
        }

    }
}
