using System;
using System.Text;
using log4net;
using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.Logging.Log4Net.Tests
{
    [TestClass]
    public class LogWriterTests
    {
        private LogWriter sut;
        private ICorrelation correlation;
        private ILog log;
        private log4net.Core.ILogger logger;
        private ILoggerRepository repository;
        
        private readonly object source = new StringBuilder();
        private static readonly string CallerId = "TestCaller";
        private static readonly string CorrelationId = "TestCorrelation";
        private static readonly Exception Exception = new Exception("Test");
        private static readonly string Message = "Test message, p1: {0}, p2: {1}";
        private static readonly object[] MessageParams = { "param1", "param2" };

        [TestInitialize]
        public void Setup()
        {
            correlation = new Correlation(CorrelationId, CallerId);
            log = Substitute.For<ILog>();
            logger = Substitute.For<log4net.Core.ILogger>();
            log.Logger.Returns(logger);

            repository = Substitute.For<ILoggerRepository>();
            repository.GetLogger(source.GetType()).Returns(log);

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
            log.IsDebugEnabled.Returns(false);

            repository
                .When(r => r.GetLogger(Arg.Any<Type>()))
                .Do(callInfo => log.IsDebugEnabled.Returns(true));

            sut.WriteLog(correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<LoggingEvent>());
        }

        [TestMethod]
        public void VerboseEventsAreNotLoggedIfVerboseLevelIsDisabled()
        {
            log.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            logger.DidNotReceive().Log(Arg.Any<LoggingEvent>());
        }

        [TestMethod]
        public void InformationEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            log.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<LoggingEvent>());
        }

        [TestMethod]
        public void WarningEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            log.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Warning, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<LoggingEvent>());
        }

        [TestMethod]
        public void ErrorEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            log.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Error, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<LoggingEvent>());
        }

        [TestMethod]
        public void CriticalEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            log.IsDebugEnabled.Returns(false);
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Any<LoggingEvent>());
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
            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.MessageObject.Equals(expectedMessage)));
        }

        [TestMethod]
        public void NullMessageParamsArrayIsIgnored()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, null);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.MessageObject.Equals(Message)));
        }

        [TestMethod]
        public void EmptyMessageParamsArrayIsIgnored()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, new object[0]);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.MessageObject.Equals(Message)));
        }

        [TestMethod]
        public void CorrelationCanBeNull()
        {
            sut.WriteLog(null, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => !e.Properties.Contains("CallerId") && !e.Properties.Contains("CorrelationId")));
        }

        [TestMethod]
        public void CallerIdIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.Properties["CallerId"].Equals(CallerId)));
        }

        [TestMethod]
        public void CorrelationIdIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.Properties["CorrelationId"].Equals(CorrelationId)));
        }

        [TestMethod]
        public void ExceptionIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.ExceptionObject == Exception));
        }

        [TestMethod]
        public void VerboseLevelIsLogged()
        {
            log.IsDebugEnabled.Returns(true);
            sut.WriteLog(correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.Level == Level.Verbose));
        }

        [TestMethod]
        public void InformationLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.Level == Level.Info));
        }

        [TestMethod]
        public void WarningLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Warning, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.Level == Level.Warn));
        }

        [TestMethod]
        public void ErrorLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Error, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.Level == Level.Error));
        }

        [TestMethod]
        public void CriticalLevelIsLogged()
        {
            sut.WriteLog(correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            logger.Received().Log(Arg.Is<LoggingEvent>(e => e.Level == Level.Critical));
        }


    }
}