using System;
using log4net;
using log4net.Config;

namespace Affecto.Logging.Log4Net
{
    internal class Log4NetWrapper
    {
        public virtual void Configure()
        {
            XmlConfigurator.Configure();
        }

        public virtual ILog GetLog(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}