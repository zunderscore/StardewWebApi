using StardewValley;
using StardewWebApi.Game;
using StardewWebApi.Game.Events;
using StardewWebApi.Types;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace StardewWebApi;

internal class WebServer
{
    private WebServer()
    {
        _apiEndpoints = Assembly.GetCallingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ApiControllerBase)))
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttributes(typeof(ApiEndpointAttribute), false).Length > 0)
            .ToList();
    }

    private static WebServer? _instance;
    public static WebServer Instance => _instance ??= new WebServer();

    public static SemaphoreSlim ListenerLock = new(1, 1);

    private volatile bool _runListenLoop;
    private readonly List<MethodInfo> _apiEndpoints = new();
    private readonly List<WebSocket> _webSocketConnections = new();

    private readonly HttpListener _listener = new()
    {
        Prefixes = { "http://localhost:7882/", "http://127.0.0.1:7882/", "http://[::1]:7882/" }
    };

    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public void StartWebServer()
    {
        _runListenLoop = true;
        Task.Run(MainLoop);
    }

    public void StopWebServer()
    {
        _runListenLoop = false;
    }

    private void MainLoop()
    {
        _listener.Start();

        var sem = new SemaphoreSlim(20, 20);

        while (_runListenLoop)
        {
            sem.Wait();

            try
            {
                _listener.GetContextAsync().ContinueWith(async (t) =>
                {
                    try
                    {
                        sem.Release();

                        var ctx = await t;
                        await ProcessRequest(ctx);
                    }
                    catch { }
                });
            }
            catch { }
        }

        _listener.Stop();
    }

    private async Task ProcessRequest(HttpListenerContext context)
    {
        using var response = context.Response;

        try
        {
            SMAPIWrapper.Instance.Log($"Received request for {context.Request.Url}");

            var path = (context.Request.Url?.AbsolutePath ?? "").ToLower();

            if (String.IsNullOrEmpty(path))
            {
                response.StatusCode = 500;
                return;
            }

            if (path == "/events")
            {
                if (context.Request.IsWebSocketRequest)
                {
                    await ProcessWebSocketRequest(context);
                }
                else
                {
                    response.BadRequest("This endpoint requires a WebSocket connection");
                }
            }

            var endpoint = _apiEndpoints.FirstOrDefault(e =>
            {
                var apiAttribute = e.GetCustomAttribute<ApiEndpointAttribute>()!;

                return apiAttribute.Path.ToLower() == path && apiAttribute.Method == context.Request.HttpMethod;
            });

            if (endpoint is null)
            {
                SMAPIWrapper.Instance.Log($"No endpoint handler found for {context.Request.Url!.AbsolutePath}");
                response.NotFound();
            }
            else
            {
                SMAPIWrapper.Instance.Log($"Found endpoint handler for {context.Request.Url!.AbsolutePath}");
                ProcessEndpointRequest(endpoint, context);
            }
        }
        catch (Exception ex)
        {
            SMAPIWrapper.Instance.Log($"Error processing request: {ex.Message}");
            response.ServerError(ex);
        }
    }

    private static void ProcessEndpointRequest(MethodInfo endpoint, HttpListenerContext context)
    {
        if ((endpoint.GetCustomAttribute<RequireLoadedGameAttribute>() != null
            || endpoint.DeclaringType?.GetCustomAttribute<RequireLoadedGameAttribute>() != null)
            && !Game1.hasLoadedGame)
        {
            context.Response.BadRequest("No save loaded. Please load a save and try again.");
            return;
        }

        var controller = Activator.CreateInstance(endpoint.DeclaringType!);
        (controller as ApiControllerBase)!.HttpContext = context;

        endpoint.Invoke(controller, null);
    }

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

internal record ErrorResponse(string Error);

internal static class HttpListenerExtensions
{
    public static void WriteResponseBody(this HttpListenerResponse response, object responseBody)
    {
        response.ContentType = "application/json";
        var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseBody, WebServer.SerializerOptions));
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    public static void CreateResponse(this HttpListenerResponse response, int statusCode, object? responseBody = null)
    {
        response.StatusCode = statusCode;
        if (responseBody != null)
        {
            response.WriteResponseBody(responseBody);
        }
    }

    public static void Ok(this HttpListenerResponse response, object? responseBody)
    {
        response.CreateResponse(200, responseBody);
    }

    public static void NoContent(this HttpListenerResponse response)
    {
        response.CreateResponse(204);
    }

    public static void BadRequest(this HttpListenerResponse response, string errorMessage)
    {
        response.CreateResponse(400, new ErrorResponse(errorMessage));
    }

    public static void NotFound(this HttpListenerResponse response, string? errorMessage = null)
    {
        response.CreateResponse(404, errorMessage != null ? new ErrorResponse(errorMessage) : null);
    }

    public static void ServerError(this HttpListenerResponse response, object responseBody)
    {
        response.CreateResponse(500, responseBody);
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