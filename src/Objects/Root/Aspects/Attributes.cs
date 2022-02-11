using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Core.Objects.Root
{
    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    public partial class AppalachiaObject
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaRepository
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaObject<T>
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class SingletonAppalachiaObject<T>
    {
    }

    [DoNotReorderFields]
    [ExecuteInEditMode]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaBehaviour
    {
    }

    [DoNotReorderFields]
    [ExecuteInEditMode]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaBehaviour<T>
    {
    }

    [DoNotReorderFields]
    [ExecuteInEditMode]
    [Serializable]
    [SmartLabelChildren]
    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    [InlineProperty]
    public partial class AppalachiaSimpleBase
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaBase
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaBase<T>
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaSimplePlayable
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaPlayable
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaPlayable<T>
    {
    }

    [DoNotReorderFields]
    [Serializable]
    [SmartLabelChildren]
    public partial class AppalachiaSelectable<T>
    {
    }
}
