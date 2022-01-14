namespace Appalachia.Core.Math.Stats
{
    public sealed class StatsTrackerCollection<TS, T>
        where TS : StatsTracker<T>, new()
    {
        public StatsTrackerCollection(int size)
        {
            _stats = new TS[size];
            for (var i = 0; i < size; i++)
            {
                _stats[i] = new TS();
            }
        }

        #region Fields and Autoproperties

        private readonly TS[] _stats;

        #endregion

        public TS this[int index] => _stats[index];
    }
}
