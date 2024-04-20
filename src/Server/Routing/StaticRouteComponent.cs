namespace StardewWebApi.Server.Routing;

internal class StaticRouteComponent : RouteComponent, IEquatable<StaticRouteComponent>
{
    public StaticRouteComponent(string value) : base(RouteComponentType.Static)
    {
        Value = value;
    }

    public readonly string Value;


    public bool Equals(StaticRouteComponent? other) => other is not null && other.Value == Value;

    public override bool Equals(object? obj) => Equals(obj as StaticRouteComponent);

    public static bool operator ==(StaticRouteComponent a, StaticRouteComponent b) => a.Equals(b);

    public static bool operator !=(StaticRouteComponent a, StaticRouteComponent b) => !(a == b);

    public override int GetHashCode() => base.GetHashCode();
}