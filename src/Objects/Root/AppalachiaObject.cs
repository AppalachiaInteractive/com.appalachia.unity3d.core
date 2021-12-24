#region

using UnityEngine;

// ReSharper disable StaticMemberInGenericType

#endregion

namespace Appalachia.Core.Objects.Root
{
    public abstract partial class AppalachiaObject : ScriptableObject
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaObject) + ".";

        #endregion
    }

    public abstract partial class AppalachiaObject<T> : AppalachiaObject
        where T : AppalachiaObject<T>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(AppalachiaObject<T>) + ".";

        #endregion
    }
}
