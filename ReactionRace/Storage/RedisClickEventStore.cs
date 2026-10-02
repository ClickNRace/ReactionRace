using StackExchange.Redis;

namespace ReactionRace.Storage
{
    public class RedisClickEventStore : IClickEventStore
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisClickEventStore(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task RecordClickAsync(ClickEvent evt, CancellationToken ct)
        {
            var db = _redis.GetDatabase();
            await db.ListRightPushAsync("clicks", $"{evt.ConnectionId}:{evt.ServerTimeUtc:O}");
        }
    }
}