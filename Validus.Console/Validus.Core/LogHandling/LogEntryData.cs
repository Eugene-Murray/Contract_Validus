

namespace Validus.Core.LogHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public static class LogEntryData
    {

        private static System.Diagnostics.TraceEventType GetSeverity(LogSeverity logSeverity)
        {
            switch (logSeverity)
            {
                case LogSeverity.Information:
                    return System.Diagnostics.TraceEventType.Information;
                case LogSeverity.Tracing:
                    return System.Diagnostics.TraceEventType.Verbose;
                case LogSeverity.Warning:
                    return System.Diagnostics.TraceEventType.Warning;
                case LogSeverity.Error:
                    return System.Diagnostics.TraceEventType.Error;
                default:
                    return System.Diagnostics.TraceEventType.Information;
            }

        }

        public static void LogEntryAsync(string message, LogSeverity logSeverity, LogCategory logCategory)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Logger.Write(new LogEntry
                {
                    Title = logCategory.ToString(),
                    Message = message,
                    Categories = new List<string> { logCategory.ToString() },
                    Severity = GetSeverity(logSeverity)
                });
            });
        }

        public static void LogEntry(string message, LogSeverity logSeverity, LogCategory logCategory)
        {
            Logger.Write(new LogEntry
            {
                Title = logCategory.ToString(),
                Message = message,
                Categories = new List<string> { logCategory.ToString() },
                Severity = GetSeverity(logSeverity)
            });
        }
    }
}
