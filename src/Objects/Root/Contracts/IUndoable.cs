namespace Appalachia.Core.Objects.Root.Contracts
{
    public interface IUndoable
    {
        void RecordUndo(string operation, string modifiedBy);
    }
}
