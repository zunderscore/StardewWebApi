using System.Net;

namespace StardewWebApi.Server;

public abstract class ApiControllerBase
{
    public HttpListenerContext? HttpContext { get; set; }
    public HttpListenerRequest Request => HttpContext?.Request!;
    public HttpListenerResponse Response => HttpContext?.Response!;
}