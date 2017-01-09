using System;
using nLog = NLog;

namespace Affecto.Logging.NLog
{
    internal interface ILoggerRepository
    {
        nLog.ILogger GetLogger(Type type);
    }
}
