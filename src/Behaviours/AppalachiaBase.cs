#region

using System;

#endregion

namespace Appalachia.Core.Behaviours
{
    [Serializable]
    public abstract class AppalachiaBase<T>
        where T : AppalachiaBase<T>
    {
    }
}
