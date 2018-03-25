using System;
using System.IO;
using System.Reflection;

using log4net;
using log4net.Config;

namespace Affecto.Logging.Log4Net
{
    internal class Log4NetWrapper
    {
        public virtual void Configure()
        {
            log4net.Repository.ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public virtual ILog GetLog(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}