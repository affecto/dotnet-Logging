using System;
using System.Diagnostics;
using System.Linq;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Affecto.Logging.Serilog
{
    internal class CallingTypeAndMethodEnricher : ILogEventEnricher
    {
        private readonly int stackFramesToSkip;

        public CallingTypeAndMethodEnricher(int stackFramesToSkip = 0)
        {
            if (stackFramesToSkip < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stackFramesToSkip));
            }

            this.stackFramesToSkip = stackFramesToSkip;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            int skip = stackFramesToSkip + 6;

            while (true)
            {
                var stack = new StackFrame(skip);
                if (!stack.HasMethod())
                {
                    logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue("<unknown method>")));
                    return;
                }

                var method = stack.GetMethod();
                if (method != null && method.DeclaringType != null && method.DeclaringType.Assembly != typeof(Log).Assembly)
                {
                    var caller = $"{method.DeclaringType.FullName}.{method.Name}({string.Join(", ", method.GetParameters().Select(pi => pi.ParameterType.Name))})";
                    logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue(caller)));
                    return;
                }

                skip++;
            }
        }
    }

    internal static class EnrichmentExtensions
    {
        public static LoggerConfiguration WithCallingTypeAndMethod(this LoggerEnrichmentConfiguration enrichmentConfiguration, int stackFramesToSkip = 0)
        {
            return enrichmentConfiguration.With(new CallingTypeAndMethodEnricher(stackFramesToSkip));
        }
    }
}