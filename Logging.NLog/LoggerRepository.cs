using System;
using System.Collections.Generic;
using nLog = NLog;

namespace Affecto.Logging.NLog
{
    internal class LoggerRepository : ILoggerRepository
    {
        private readonly NLogWrapper _wrapper;
        private readonly object _createLock;
        private readonly Dictionary<Type, nLog.ILogger> _createdLoggers;

        private bool isConfigured;

        public LoggerRepository(NLogWrapper wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException(nameof(wrapper));
            }
            this._wrapper = wrapper;

            _createLock = new object();
            _createdLoggers = new Dictionary<Type, nLog.ILogger>();
            isConfigured = false;
        }

        public nLog.ILogger GetLogger(Type type)
        {
            lock (_createLock)
            {
                if (!_createdLoggers.ContainsKey(type))
                {
                    if (!isConfigured)
                    {
                        _wrapper.Configure();
                        isConfigured = true;
                    }

                    nLog.ILogger logger = _wrapper.GetLogger(type);
                    _createdLoggers[type] = logger;
                    return logger;
                }
            }

            return _createdLoggers[type];
        }
    }
}