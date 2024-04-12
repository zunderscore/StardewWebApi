using StardewModdingAPI;

namespace StardewWebApi.Game;

public class SMAPIWrapper
{
    private SMAPIWrapper() { }

    private static SMAPIWrapper? _instance;
    public static SMAPIWrapper Instance => _instance ??= new SMAPIWrapper();

    public IMonitor? Monitor { get; private set; }
    public IModHelper? Helper { get; private set; }

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