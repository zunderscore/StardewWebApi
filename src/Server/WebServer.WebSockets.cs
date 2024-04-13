using StardewWebApi.Game;
using StardewWebApi.Game.Events;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace StardewWebApi.Server;

internal partial class WebServer
{
    private readonly List<WebSocket> _webSocketConnections = new();

    private async Task ProcessWebSocketRequest(HttpListenerContext context)
    {
        try
        {
            var webSocketContext = await context.AcceptWebSocketAsync(null);
            var webSocket = webSocketContext.WebSocket;

            _webSocketConnections.Add(webSocket);
            await webSocket.SendWebSocketMessageAsync(new GameEvent("Connected"));

            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);

                switch (message)
                {
                    default:
                        await webSocket.SendWebSocketMessageAsync(new ErrorResponse("Invalid command"));
                        break;
                }

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);

            _webSocketConnections.Remove(webSocket);
        }
        catch (Exception ex)
        {
            SMAPIWrapper.Instance.Log($"Error processing WebSocket request: {ex.Message}");
            context.Response.ServerError(ex);
        }
    }

    public async void BroadcastWebSocketMessageAsync(object messageBody)
    {
        foreach (var webSocket in _webSocketConnections)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendWebSocketMessageAsync(messageBody);
            }
        }
    }

    public void SendGameEvent(string eventName, object? data = null)
    {
        BroadcastWebSocketMessageAsync(new GameEvent(eventName, data));
    }
}

internal static class WebSocketExtensions
{
    public static async Task SendWebSocketMessageAsync(this WebSocket webSocket, object messageBody)
    {
        await webSocket.SendAsync(
            new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageBody, WebServer.SerializerOptions))),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None
        );
    }
}