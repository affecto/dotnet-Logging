using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Affecto.Logging.Log4Net
{
    internal class Log4NetWrapper
    {
        public virtual void Configure()
        {
            log4net.Repository.ILoggerRepository logRepository = LogManager.GetRepository(GetInitialAssembly());

            var configFileInfo = new FileInfo("log4net.config");

            if (configFileInfo.Exists)
            {
                XmlConfigurator.Configure(logRepository, configFileInfo);
            }
            else
            {
                XmlConfigurator.Configure(logRepository);
            }
        }

        public virtual ILog GetLog(Type type)
        {
            return LogManager.GetLogger(type);
        }

        private Assembly GetInitialAssembly()
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                IEnumerable<Assembly> callerAssemblies = new StackTrace().GetFrames()
                    .Select(x => x.GetMethod().ReflectedType.Assembly).Distinct()
                    .Where(x => x.GetReferencedAssemblies().Any(y => y.FullName == currentAssembly.FullName));
                assembly = callerAssemblies.Last();
            }

            return assembly;
        }
    }
}