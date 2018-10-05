using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using nLog = NLog;

namespace Affecto.Logging.NLog.Tests
{
    [TestClass]
    public class LogWriterTests
    {
        private LogWriter sut;
        private ICorrelation correlation;
        private nLog.ILogger logger;
        private ILoggerRepository repository;

        private readonly object source = new StringBuilder();
        private const string CallerId = "TestCaller";
        private const string CorrelationId = "TestCorrelation";
        private static readonly Exception Exception = new Exception("Test");
        private const string Message = "Test message, p1: {0}, p2: {1}";
        private static readonly object[] MessageParams = { "param1", "param2" };

        [TestInitialize]
        public void Setup()
        {
            correlation = new Correlation(CorrelationId, CallerId);
            logger = Substitute.For<nLog.ILogger>();

            repository = Substitute.For<ILoggerRepository>();
            repository.GetLogger(source.GetType()).Returns(logger);

            sut = new LogWriter(repository, source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerRepositoryCannotBeNull()
        {
            sut = new LogWriter(null, null);
        }

        [TestMethod]
        public void SourceTypeIsLogWriterItselfIfNullSourceIsPassedInConstructor()
        {
            sut = new LogWriter(repository, null);
            sut.WriteLog(correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            repository.Received().GetLogger(typeof(LogWriter));
        }

        [TestMethod]
        public void SourceTypeIsUsedToGetLogger()
        {
            sut.WriteLog(correlation, LogEventLevel.Information, Exception, Message, MessageParams);
            repository.Received().GetLogger(typeof(StringBuilder));
        }

        [TestMethod]
        public void VerboseLevelStateIsCheckedAfterCreatingLogger()
        {
            logger.IsDebugEnabled.Returns(false);

            repository
                .When(r => r.GetLogger(Arg.Any<Type>()))
                .Do(callInfo => logger.IsDebugEnabled.Returns(true));

            sut.WriteLog(correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void VerboseEventsAreNotLoggedIfVerboseLevelIsDisabled()
        {
            logger.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            logger.DidNotReceive().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void InformationEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            logger.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void WarningEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            logger.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Warning, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void ErrorEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            logger.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Error, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void CriticalEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            logger.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void LoggerIsQueriedFromRepositoryOnlyOnce()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            repository.Received(1).GetLogger(Arg.Any<Type>());
        }

        [TestMethod]
        public void MessageIsFormattedCorrectly()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            const string expectedMessage = "Test message, p1: param1, p2: param2";

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.FormattedMessage.Equals(expectedMessage)));
        }

        [TestMethod]
        public void NullMessageParamsArrayIsIgnored()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, null);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Message.Equals(Message)));
        }

        [TestMethod]
        public void EmptyMessageParamsArrayIsIgnored()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, new object[0]);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Message.Equals(Message)));
        }

        [TestMethod]
        public void CorrelationCanBeNull()
        {
            sut.WriteLog(null, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => !e.Properties.Keys.Contains("CallerId") && !e.Properties.Keys.Contains("CorrelationId")));
        }

        [TestMethod]
        public void CallerIdIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Properties["CallerId"].Equals(CallerId)));
        }

        [TestMethod]
        public void CorrelationIdIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Properties["CorrelationId"].Equals(CorrelationId)));
        }

        [TestMethod]
        public void ExceptionIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Exception == Exception));
        }

        [TestMethod]
        public void VerboseLevelIsLogged()
        {
            logger.IsDebugEnabled.Returns(true);
            sut.WriteLog(correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Debug));
        }

        [TestMethod]
        public void DebugLevelIsLogged()
        {
            logger.IsDebugEnabled.Returns(true);
            sut.WriteLog(correlation, LogEventLevel.Debug, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Debug));
        }

        [TestMethod]
        public void InformationLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Info));
        }

        [TestMethod]
        public void WarningLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Warning, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Warn));
        }

        [TestMethod]
        public void ErrorLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Error, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Error));
        }

        [TestMethod]
        public void CriticalLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Fatal));
        }
    }
}
