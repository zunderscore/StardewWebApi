using System.Reflection;

namespace StardewWebApi.Server.Routing;

internal class MatchedRoute
{
    public MatchedRoute(Route route, MethodInfo method, string staticRoute)
    {
        Route = route;
        Method = method;
        Parameters = route.GetRouteParameters(staticRoute);
    }

    public Route Route { get; }
    public MethodInfo Method { get; }
    public Dictionary<string, string> Parameters { get; }
}