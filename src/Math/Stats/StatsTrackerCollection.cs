namespace Appalachia.Core.Math.Stats
{
    public sealed class StatsTrackerCollection<TS, T>
        where TS : StatsTracker<T>, new()
    {
        private readonly TS[] _stats;

        public StatsTrackerCollection(int size)
        {
            _stats = new TS[size];
            for (var i = 0; i < size; i++)
            {
                _stats[i] = new TS();
            }
        }

        public TS this[int index] => _stats[index];
    }
}
