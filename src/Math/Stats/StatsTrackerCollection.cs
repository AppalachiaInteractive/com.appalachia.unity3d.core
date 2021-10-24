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

        private readonly TS[] _stats;

        public TS this[int index] => _stats[index];
    }
}
