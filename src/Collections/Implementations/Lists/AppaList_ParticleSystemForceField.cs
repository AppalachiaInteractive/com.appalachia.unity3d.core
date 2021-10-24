#region

using System;
using UnityEngine;

#endregion

namespace Appalachia.Core.Collections.Implementations.Lists
{
    [Serializable]
    public sealed class AppaList_ParticleSystemForceField : AppaList<ParticleSystemForceField>
    {
        public AppaList_ParticleSystemForceField()
        {
        }

        public AppaList_ParticleSystemForceField(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_ParticleSystemForceField(AppaList<ParticleSystemForceField> list) : base(list)
        {
        }

        public AppaList_ParticleSystemForceField(ParticleSystemForceField[] values) : base(values)
        {
        }
    }
}
