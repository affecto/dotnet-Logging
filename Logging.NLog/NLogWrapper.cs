using System;
using nLog = NLog;

namespace Affecto.Logging.NLog
{
    internal class NLogWrapper
    {
        public virtual void Configure()
        {
            nLog.LogManager.ReconfigExistingLoggers();
        }

        public virtual nLog.ILogger GetLogger(Type type)
        {
            return nLog.LogManager.GetLogger(type.FullName);
        }
    }
}