using System;
using Appalachia.CI.Constants;
using Sirenix.OdinInspector;

namespace Appalachia.Core.Collections
{
    [Serializable]
    [HideLabel]
    [Searchable(FuzzySearch = true, Recursive = true)]
    public abstract class AppaCollection
    {
        #region Fields and Autoproperties

        [NonSerialized] private AppaContext _context;

        #endregion

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
