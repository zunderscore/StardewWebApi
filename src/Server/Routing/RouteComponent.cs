namespace StardewWebApi.Server.Routing;

internal abstract class RouteComponent : IEquatable<RouteComponent>
{
    public RouteComponent(RouteComponentType type)
    {
        Type = type;
    }

    public RouteComponentType Type { get; }

    public bool Equals(RouteComponent? other)
    {
        if (other is null || Type != other.Type) return false;

        return other switch
        {
            StaticRouteComponent staticRouteComponent => staticRouteComponent.Equals(this as StaticRouteComponent),
            ParameterizedRouteComponent parameterizedRouteComponent => parameterizedRouteComponent.Equals(this as ParameterizedRouteComponent),
            _ => throw new ArgumentException("Unknown route component type")
        };
    }

    public override bool Equals(object? obj) => Equals(obj as RouteComponent);

    public static bool operator ==(RouteComponent a, RouteComponent b) => a.Equals(b);

    public static bool operator !=(RouteComponent a, RouteComponent b) => !(a == b);

    public override int GetHashCode() => base.GetHashCode();
}