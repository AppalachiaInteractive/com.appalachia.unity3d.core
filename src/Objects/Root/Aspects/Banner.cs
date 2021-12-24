using Appalachia.CI.Integration.Attributes;

namespace Appalachia.Core.Objects.Root
{
    [InspectorIcon(Brand.AppalachiaObject.Icon)]
    public partial class AppalachiaObject
    {
        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaObject.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaObject.Fallback;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaObject.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaObject.Color;
        }
    }

    [InspectorIcon(Brand.AppalachiaRepository.Icon)]
    public partial class AppalachiaRepository
    {
        protected override string GetBackgroundColor()
        {
            return Brand.AppalachiaRepository.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.AppalachiaRepository.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.AppalachiaRepository.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.AppalachiaRepository.Color;
        }
    }

    public partial class AppalachiaObject<T>
    {
    }

    [InspectorIcon(Brand.SingletonAppalachiaObject.Icon)]
    public partial class SingletonAppalachiaObject<T>
    {
        protected override string GetBackgroundColor()
        {
            return Brand.SingletonAppalachiaObject.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.SingletonAppalachiaObject.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.SingletonAppalachiaObject.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.SingletonAppalachiaObject.Color;
        }
    }

    [InspectorIcon(Brand.AppalachiaBehaviour.Icon)]
    public partial class AppalachiaBehaviour
    {
        #region Constants and Static Readonly

        public const string GAMEOBJECTICON = Brand.Squirrel.Outline;

        #endregion

        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaBehaviour.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaBehaviour.Fallback;
        }

        protected virtual string GetGameObjectIcon()
        {
            return GAMEOBJECTICON;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaBehaviour.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaBehaviour.Color;
        }
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    [InspectorIcon(Brand.SingletonAppalachiaBehaviour.Icon)]
    public partial class SingletonAppalachiaBehaviour<T>
    {
        protected override string GetBackgroundColor()
        {
            return Brand.SingletonAppalachiaBehaviour.Banner;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.SingletonAppalachiaBehaviour.Fallback;
        }

        protected override string GetTitle()
        {
            return Brand.SingletonAppalachiaBehaviour.Text;
        }

        protected override string GetTitleColor()
        {
            return Brand.SingletonAppalachiaBehaviour.Color;
        }
    }

    [InspectorIcon(Brand.AppalachiaBase.Icon)]
    public partial class AppalachiaBase
    {
        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaBase.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaBase.Fallback;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaBase.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaBase.Color;
        }
    }

    public partial class AppalachiaBase<T>
    {
    }

    [InspectorIcon(Brand.AppalachiaPlayable.Icon)]
    public partial class AppalachiaPlayable
    {
        protected virtual string GetBackgroundColor()
        {
            return Brand.AppalachiaPlayable.Banner;
        }

        protected virtual string GetFallbackSubtitle()
        {
            return Brand.Subtitle.Fallback;
        }

        protected virtual string GetFallbackTitle()
        {
            return Brand.AppalachiaPlayable.Fallback;
        }

        protected virtual string GetSubtitle()
        {
            return Brand.Subtitle.Text;
        }

        protected virtual string GetTitle()
        {
            return Brand.AppalachiaPlayable.Text;
        }

        protected virtual string GetTitleColor()
        {
            return Brand.AppalachiaPlayable.Color;
        }
    }

    public partial class AppalachiaPlayable<T>
    {
    }
}
