namespace ReactionRace.Storage
{
    public interface IClickEventStore
    {
        Task RecordClickAsync(ClickEvent evt, CancellationToken ct);
    }

    public record ClickEvent(string ConnectionId, DateTime ServerTimeUtc);
}
