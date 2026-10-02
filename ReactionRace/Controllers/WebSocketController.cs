using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactionRace.Handlers;

namespace ReactionRace.Controllers
{
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketConnectionHandler _handler;

        public WebSocketController(IWebSocketConnectionHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await _handler.HandleAsync(socket, HttpContext.RequestAborted);
        }
    }
}
