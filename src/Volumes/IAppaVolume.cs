using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Volumes
{
    /// <summary>
    ///     An interface for AppaVolumes
    /// </summary>
    public interface IAppaVolume
    {
        /// <summary>
        ///     Specifies whether to apply the AppaVolume to the entire Scene or not.
        /// </summary>
        bool isGlobal { get; set; }

        /// <summary>
        ///     The colliders of the volume if <see cref="isGlobal" /> is false
        /// </summary>
        List<Collider> colliders { get; }
    }
}
