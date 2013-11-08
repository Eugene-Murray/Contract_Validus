using System;
using System.Linq;

namespace Validus.Core.LogHandling
{
    public class LogHandler : ILogHandler
    {
        public void WriteLogAsync(string message, LogSeverity logSeverity, LogCategory logCategory)
        {
            LogEntryData.LogEntryAsync(message, logSeverity, logCategory);
        }

        public void WriteLog(string message, LogSeverity logSeverity, LogCategory logCategory)
        {
            LogEntryData.LogEntry(message, logSeverity, logCategory);
        }
    }
}
