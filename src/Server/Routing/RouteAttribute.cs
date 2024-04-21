namespace StardewWebApi.Server.Routing;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RouteAttribute : Attribute
{
    public RouteAttribute(string path, string method = "GET")
    {
        Path = path;
        Method = method;
    }

    public string Path { get; init; }
    public string Method { get; init; }
}