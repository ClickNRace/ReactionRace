using System.Net.WebSockets;

namespace ReactionRace.Handlers
{
    public interface IWebSocketConnectionHandler
    {
        Task HandleAsync(WebSocket socket, CancellationToken ct); // можем засунуть сюда любой обработчик событий (клики, чат и пр.)
    }
}
