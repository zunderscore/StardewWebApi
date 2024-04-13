namespace StardewWebApi.Game;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireLoadedGameAttribute : Attribute { }