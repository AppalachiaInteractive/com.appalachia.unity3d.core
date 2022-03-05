#region

using System.Diagnostics;
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
}
