using System.Collections.Concurrent;

namespace ReactionRace.Storage
{
    public class InMemoryClickEventStore : IClickEventStore
    {
        private readonly ConcurrentBag<ClickEvent> _events = new();

        public Task RecordClickAsync(ClickEvent evt, CancellationToken ct)
        {
            _events.Add(evt);
            return Task.CompletedTask;
        }
    }
}
