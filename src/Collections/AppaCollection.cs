using System;
using Appalachia.CI.Constants;

namespace Appalachia.Core.Collections
{
    [Serializable]
    public abstract class AppaCollection
    {
        [NonSerialized] private AppaContext _context;

        protected AppaContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new AppaContext(this);
                }

                return _context;
            }
        }
    }
}
