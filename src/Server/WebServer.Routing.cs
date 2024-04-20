using System.Net;
using System.Reflection;
using StardewWebApi.Server.Routing;

namespace StardewWebApi.Server;

internal partial class WebServer
{
    private readonly SortedList<Route, MethodInfo> _routes = new();

    private void CreateRouteTable()
    {
        Assembly.GetCallingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ApiControllerBase)))
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetCustomAttribute(typeof(RouteAttribute), false) is not null)
            .ToList()
            .ForEach(m =>
            {
                var routeAttribute = m.GetCustomAttribute(typeof(RouteAttribute), false) as RouteAttribute;
                _routes.Add(new Route(routeAttribute!.Path), m);
            });
    }

    private MatchedRoute? FindRoute(HttpListenerRequest request)
    {
        var path = (request.Url?.AbsolutePath ?? "").ToLower();
        var route = _routes.Keys.FirstOrDefault(r => r.MatchesStaticRoute(path));

        return route is not null
            ? new MatchedRoute(route, _routes[route], path)
            : null;
    }
}