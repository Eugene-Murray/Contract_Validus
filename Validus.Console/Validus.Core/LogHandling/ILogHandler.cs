using System;
using System.Linq;

namespace Validus.Core.LogHandling
{
    public interface ILogHandler
    {
        void WriteLog(string message, LogSeverity logSeverity, LogCategory logCategory);
    }
}
