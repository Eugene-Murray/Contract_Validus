using System;
using System.Linq;
using EFTracingProvider;
using System.Data.Common;

namespace Validus.Core.EFLogging
{
    public class DbTracingConnection : EFTracingConnection
    {
        protected override DbCommand CreateDbCommand()
        {
            return this.WrappedConnection.CreateCommand();
        }
    }
}