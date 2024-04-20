using StardewWebApi.Game;
using System.Net;
using System.Text;
using System.Text.Json;

namespace StardewWebApi.Server;

internal partial class WebServer
{
    private WebServer()
    {
        CreateRouteTable();
    }

    private static WebServer? _instance;
    public static WebServer Instance => _instance ??= new WebServer();

    private volatile bool _runListenLoop;

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
                response.ServerError();
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

            var route = FindRoute(context.Request);

            if (route is not null)
            {
                SMAPIWrapper.Instance.Log($"Found matching route for {context.Request.Url!.AbsolutePath}");
                ProcessEndpointRequest(route, context);
            }
            else
            {
                SMAPIWrapper.Instance.Log($"No matching route found for {context.Request.Url!.AbsolutePath}");
                response.NotFound();
            }
        }
        catch (Exception ex)
        {
            SMAPIWrapper.Instance.Log($"Error processing request: {ex.Message}");
            response.ServerError(ex);
        }
    }
}

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
        if (responseBody is not null)
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
        response.CreateResponse(404, errorMessage is not null ? new ErrorResponse(errorMessage) : null);
    }

    public static void ServerError(this HttpListenerResponse response, string errorMessage)
    {
        response.ServerError(new ErrorResponse(errorMessage));
    }

    public static void ServerError(this HttpListenerResponse response, object? responseBody = null)
    {
        response.CreateResponse(500, responseBody);
    }
}