// LogHandler.cs
public class LogHandler
{
    private readonly ILogger _logger;

    public LogHandler(ILogger logger)
    {
        _logger = logger;
    }

    public void AddLog(LogRecord log)
    {
        _logger.Log(log);
    }
}
