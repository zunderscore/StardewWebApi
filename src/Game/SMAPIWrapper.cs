using System.Diagnostics.CodeAnalysis;
using StardewModdingAPI;

namespace StardewWebApi.Game;

public class SMAPIWrapper
{
    private SMAPIWrapper() { }

    private static SMAPIWrapper? _instance;
    public static SMAPIWrapper Instance => _instance ??= new SMAPIWrapper();

    // The very first thing we do in the mod is call Initialize, so these are never null for the life of the mod
    [NotNull] public IMonitor? Monitor { get; private set; }
    [NotNull] public IModHelper? Helper { get; private set; }

    public void Initialize(IMonitor monitor, IModHelper helper)
    {
        Monitor = monitor;
        Helper = helper;
    }

    public void Log(string message, LogLevel logLevel = LogLevel.Trace)
    {
        Monitor?.Log(message, logLevel);
    }
}