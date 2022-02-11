#if UNITY_EDITOR
using Appalachia.Utility.Extensions;
using Unity.Profiling;

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaObject
    {
        public void RecordUndo(string operation, string modifiedBy)
        {
            using (_PRF_RecordUndo.Auto())
            {
                RecordUndo($"{modifiedBy}: {operation}");
            }
        }

        protected void RecordUndo(string operation)
        {
            using (_PRF_RecordUndo.Auto())
            {
                MarkAsModified();
                this.CreateUndoStep(operation);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RecordUndo =
            new ProfilerMarker(_PRF_PFX + nameof(RecordUndo));

        #endregion
    }

    public partial class AppalachiaRepository
    {
    }

    public partial class AppalachiaObject<T>
    {
    }

    public partial class SingletonAppalachiaObject<T>
    {
    }

    public partial class AppalachiaBehaviour
    {
        public void RecordUndo(string operation, string modifiedBy)
        {
            using (_PRF_RecordUndo.Auto())
            {
                RecordUndo($"{modifiedBy}: {operation}");
            }
        }

        protected void RecordUndo(string operation)
        {
            using (_PRF_RecordUndo.Auto())
            {
                this.MarkAsModified();
                this.CreateUndoStep(operation);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RecordUndo =
            new ProfilerMarker(_PRF_PFX + nameof(RecordUndo));

        #endregion
    }

    public partial class AppalachiaBehaviour<T>
    {
    }

    public partial class SingletonAppalachiaBehaviour<T>
    {
    }

    public partial class AppalachiaSimpleBase
    {
    }

    public partial class AppalachiaBase
    {
        public void RecordUndo(string operation, string modifiedBy)
        {
            using (_PRF_RecordUndo.Auto())
            {
                RecordUndo($"{modifiedBy}: {operation}");
            }
        }

        protected void RecordUndo(string operation)
        {
            using (_PRF_RecordUndo.Auto())
            {
                MarkAsModified();
                _owner.CreateUndoStep(operation);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RecordUndo =
            new ProfilerMarker(_PRF_PFX + nameof(RecordUndo));

        #endregion
    }

    public partial class AppalachiaBase<T>
    {
    }

    public partial class AppalachiaSimplePlayable
    {
    }

    public partial class AppalachiaPlayable
    {
    }

    public partial class AppalachiaPlayable<T>
    {
    }

    public partial class AppalachiaSelectable<T>
    {
        public void RecordUndo(string operation)
        {
            using (_PRF_RecordUndo.Auto())
            {
                MarkAsModified();
                this.CreateUndoStep(operation);
            }
        }

        #region Profiling

        private static readonly string _PRF_PFX7 = typeof(T).Name + ".";

        private static readonly ProfilerMarker _PRF_RecordUndo =
            new ProfilerMarker(_PRF_PFX7 + nameof(RecordUndo));

        #endregion
    }
}

#endif
