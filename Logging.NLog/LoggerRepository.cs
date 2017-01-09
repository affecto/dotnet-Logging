using System;
using System.Collections.Generic;
using nLog = NLog;

namespace Affecto.Logging.NLog
{
    internal class LoggerRepository : ILoggerRepository
    {
        private readonly NLogWrapper wrapper;
        private readonly object createLock;
        private readonly Dictionary<Type, nLog.ILogger> createdLoggers;

        private bool isConfigured;

        public LoggerRepository(NLogWrapper wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException(nameof(wrapper));
            }
            this.wrapper = wrapper;

            createLock = new object();
            createdLoggers = new Dictionary<Type, nLog.ILogger>();
            isConfigured = false;
        }

        public nLog.ILogger GetLogger(Type type)
        {
            lock (createLock)
            {
                if (!createdLoggers.ContainsKey(type))
                {
                    if (!isConfigured)
                    {
                        wrapper.Configure();
                        isConfigured = true;
                    }

                    nLog.ILogger logger = wrapper.GetLogger(type);
                    createdLoggers[type] = logger;
                    return logger;
                }
            }

            return createdLoggers[type];
        }
    }
}