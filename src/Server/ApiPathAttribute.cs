namespace StardewWebApi.Server;

[AttributeUsage(AttributeTargets.Method)]
public class ApiEndpointAttribute : Attribute
{
    public ApiEndpointAttribute(string path, string method = "GET")
    {
        Path = path;
        Method = method;
    }

    public string Path { get; init; }
    public string Method { get; init; }
}