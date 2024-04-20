namespace StardewWebApi.Server.Routing;

[AttributeUsage(AttributeTargets.Method)]
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