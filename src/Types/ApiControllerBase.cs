using System.Net;

namespace StardewWebApi.Types;

public abstract class ApiControllerBase
{
    public HttpListenerContext? HttpContext { get; set; }
    public HttpListenerRequest Request => HttpContext?.Request!;
    public HttpListenerResponse Response => HttpContext?.Response!;
}