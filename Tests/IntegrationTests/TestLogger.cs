using System.IO;
using Serilog;

namespace IntegrationTests;

public class TestLogger
{
    public static ILogger GetTestLogger(string fullPathToLog)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fullPathToLog) ?? "");

        // Otherwise, if it is null, make new logger:
        var logger = new LoggerConfiguration()
            .Enrich.WithProperty("Application", "SerilogTestContext")
            .MinimumLevel.Debug()
            .WriteTo.File(fullPathToLog, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();

        return logger;
    }
}