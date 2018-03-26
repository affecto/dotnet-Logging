using System;
using System.Collections.Generic;

using log4net;

namespace Affecto.Logging.Log4Net
{
    internal class LoggerRepository : ILoggerRepository
    {
        private readonly Log4NetWrapper wrapper;
        private readonly object createLock;
        private readonly Dictionary<Type, ILog> createdLoggers;

        private bool isConfigured;

        public LoggerRepository(Log4NetWrapper wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("wrapper");
            }
            this.wrapper = wrapper;

            createLock = new object();
            createdLoggers = new Dictionary<Type, ILog>();
            isConfigured = false;
        }

        public ILog GetLogger(Type type)
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

                    ILog log = wrapper.GetLog(type);
                    createdLoggers[type] = log;
                    return log;
                }
            }

            return createdLoggers[type];
        }
    }
}