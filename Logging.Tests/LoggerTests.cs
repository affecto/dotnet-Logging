using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;

namespace Affecto.Logging.Tests
{
    [TestClass]
    public class LoggerTests
    {
        private ILogWriter logWriter;
        private ICorrelation correlation;
        private Logger sut;

        private static readonly string CallerId = "TestCaller";
        private static readonly string CorrelationId = "TestCorrelation";
        private static readonly Exception Exception = new Exception("Test");
        private static readonly string Message = "Test message";
        private static readonly object[] MessageParams = { "param1", "param2" };

        [TestInitialize]
        public void Setup()
        {
            logWriter = Substitute.For<ILogWriter>();
            correlation = Substitute.For<ICorrelation>();
            sut = new Logger(logWriter);
        }

        [TestMethod]
        public void VerboseEventIsWrittenToLog()
        {
            sut.LogVerbose(Message, MessageParams);
            AssertCall(null, LogEventLevel.Verbose, null, Message, MessageParams);
        }

        [TestMethod]
        public void VerboseEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogVerbose(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Verbose, null, Message, MessageParams);
        }

        [TestMethod]
        public void VerboseEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogVerbose(correlation, Message, MessageParams);
            
            AssertCall(correlation, LogEventLevel.Verbose, null, Message, MessageParams);
        }

        [TestMethod]
        public void InformationEventIsWrittenToLog()
        {
            sut.LogInformation(Message, MessageParams);
            AssertCall(null, LogEventLevel.Information, null, Message, MessageParams);
        }

        [TestMethod]
        public void InformationEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogInformation(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Information, null, Message, MessageParams);
        }

        [TestMethod]
        public void InformationEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogInformation(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Information, null, Message, MessageParams);
        }

        [TestMethod]
        public void WarningEventIsWrittenToLog()
        {
            sut.LogWarning(Message, MessageParams);
            AssertCall(null, LogEventLevel.Warning, null, Message, MessageParams);
        }

        [TestMethod]
        public void WarningEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogWarning(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Warning, null, Message, MessageParams);
        }

        [TestMethod]
        public void WarningEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogWarning(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Warning, null, Message, MessageParams);
        }

        [TestMethod]
        public void WarningExceptionEventIsWrittenToLog()
        {
            sut.LogWarning(Exception, Message, MessageParams);
            AssertCall(null, LogEventLevel.Warning, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void WarningExceptionEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogWarning(correlation, Exception, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Warning, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void WarningExceptionEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogWarning(correlation, Exception, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Warning, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void ErrorEventIsWrittenToLog()
        {
            sut.LogError(Message, MessageParams);
            AssertCall(null, LogEventLevel.Error, null, Message, MessageParams);
        }

        [TestMethod]
        public void ErrorEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogError(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Error, null, Message, MessageParams);
        }

        [TestMethod]
        public void ErrorEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogError(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Error, null, Message, MessageParams);
        }

        [TestMethod]
        public void ErrorExceptionEventIsWrittenToLog()
        {
            sut.LogError(Exception, Message, MessageParams);
            AssertCall(null, LogEventLevel.Error, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void ErrorExceptionEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogError(correlation, Exception, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Error, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void ErrorExceptionEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogError(correlation, Exception, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Error, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void CriticalEventIsWrittenToLog()
        {
            sut.LogCritical(Message, MessageParams);
            AssertCall(null, LogEventLevel.Critical, null, Message, MessageParams);
        }

        [TestMethod]
        public void CriticalEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogCritical(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Critical, null, Message, MessageParams);
        }

        [TestMethod]
        public void CriticalEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogCritical(correlation, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Critical, null, Message, MessageParams);
        }

        [TestMethod]
        public void CriticalExceptionEventIsWrittenToLog()
        {
            sut.LogCritical(Exception, Message, MessageParams);
            AssertCall(null, LogEventLevel.Critical, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void CriticalExceptionEventWithCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            sut.LogCritical(correlation, Exception, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);
        }

        [TestMethod]
        public void CriticalExceptionEventWithCallerIdAndCorrelationIdIsWrittenToLog()
        {
            correlation.CorrelationId.Returns(CorrelationId);
            correlation.CallerId.Returns(CallerId);
            sut.LogCritical(correlation, Exception, Message, MessageParams);

            AssertCall(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);
        }

        private void AssertCall(ICorrelation expectedCorrelation, LogEventLevel eventLevel, Exception exception, string formatMessage,
            object[] args)
        {
            Assert.AreEqual(1, logWriter.ReceivedCalls().Count());

            ICall call = logWriter.ReceivedCalls().First();
            object[] arguments = call.GetArguments();

            Assert.AreEqual(5, arguments.Length);

            if (expectedCorrelation == null)
            {
                Assert.IsNull(arguments[0]);
            }
            else
            {
                ICorrelation actualCorrelation = arguments[0] as ICorrelation;
                Assert.IsNotNull(actualCorrelation);

                if (!string.IsNullOrEmpty(expectedCorrelation.CallerId))
                {
                    Assert.AreEqual(expectedCorrelation.CallerId, actualCorrelation.CallerId);
                }

                if (!string.IsNullOrEmpty(expectedCorrelation.CorrelationId))
                {
                    Assert.AreEqual(expectedCorrelation.CorrelationId, actualCorrelation.CorrelationId);
                }
            }

            Assert.AreEqual(eventLevel, arguments[1]);
            Assert.AreEqual(exception, arguments[2]);
            Assert.AreEqual(formatMessage, arguments[3]);

            object[] messageParams = arguments[4] as object[];

            Assert.IsNotNull(messageParams);
            Assert.AreEqual(args.Length, messageParams.Length);

            for (int i = 0; i < messageParams.Length; i++)
            {
                Assert.AreEqual(args[i], messageParams[i]);
            }
        }
    }
}