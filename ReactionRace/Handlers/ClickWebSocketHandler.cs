using ReactionRace.Common;
using ReactionRace.DTO;
using ReactionRace.DTO.Client;
using ReactionRace.DTO.Server;
using ReactionRace.Storage;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReactionRace.Handlers
{
    public class ClickWebSocketHandler : IWebSocketConnectionHandler
    {
        private readonly IClickEventStore _store;
        private DateTime? _goTime;
        private readonly string _connectionId = Guid.NewGuid().ToString();

        public ClickWebSocketHandler(IClickEventStore store)
        {
            _store = store;
        }

        public async Task HandleAsync(WebSocket socket, CancellationToken ct)
        {
            for (int i = 0; i < 3; i++)
            {


                await RunRoundAsync(socket, ct);

                var buffer = new byte[4096];

                while (socket.State == WebSocketState.Open && !ct.IsCancellationRequested)
                {
                    using var messageStream = new MemoryStream();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await socket.ReceiveAsync(buffer, ct);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, ct);
                            return;
                        }

                        messageStream.Write(buffer, 0, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ClientMessage? message;

                    try
                    {
                        var json = Encoding.UTF8.GetString(messageStream.ToArray());
                        message = JsonSerializer.Deserialize<ClientMessage>(json);
                    }
                    catch (JsonException)
                    {
                        continue;
                    }

                    if (message?.Type == MessageTypes.Click)
                    {
                        if (_goTime is null)
                        {
                            //await SendMessageAsync(
                            //socket,
                            //new ResultMessage { Type = MessageTypes.FalseStart, ReactionSec = -1 },
                            //ct);
                            continue;
                        }

                        var reactionSec = (DateTime.UtcNow - _goTime.Value).TotalSeconds;
                        _goTime = null;

                        await _store.RecordClickAsync(
                            new ClickEvent(_connectionId, DateTime.UtcNow),
                            ct);

                        await SendMessageAsync(
                            socket,
                            new ResultMessage { Type = MessageTypes.Result, ReactionSec = reactionSec },
                            ct);
                    }
                }
            }
        }

        private async Task RunRoundAsync(
            WebSocket socket,
            CancellationToken ct)
        {
            var delay = Random.Shared.Next(2000, 5000);
            await Task.Delay(delay, ct);

            _goTime = DateTime.UtcNow;

            await SendMessageAsync(
                socket,
                new ServerMessage { Type = MessageTypes.Go },
                ct);
        }

        private static async Task SendMessageAsync(
            WebSocket socket,
            object payload,
            CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(payload);
            var bytes = Encoding.UTF8.GetBytes(json);

            await socket.SendAsync(
                bytes,
                WebSocketMessageType.Text,
                true,
                ct);
        }
    }
}
