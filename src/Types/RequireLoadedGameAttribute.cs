namespace StardewWebApi.Types;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireLoadedGameAttribute : Attribute { }