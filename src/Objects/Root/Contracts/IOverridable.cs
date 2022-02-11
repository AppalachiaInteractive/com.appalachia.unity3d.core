using System;
using UnityEngine;

namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IOverridable
    {
        Color ToggleColor { get; }
        object Value { get; }

        public Type ValueType { get; }
        bool Overriding { get; set; }
        bool Frozen { get; set; }
        bool Disabled { get; set; }
        string ToggleLabel { get; }
    }
}
