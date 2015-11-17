using System;
using log4net;

namespace Affecto.Logging.Log4Net
{
    internal interface ILoggerRepository
    {
        ILog GetLogger(Type type);
    }
}