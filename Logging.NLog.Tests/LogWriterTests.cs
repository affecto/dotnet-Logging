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
        private LogWriter _sut;
        private ICorrelation _correlation;
        private nLog.ILogger _logger;
        private ILoggerRepository _repository;

        private readonly object _source = new StringBuilder();
        private const string CallerId = "TestCaller";
        private const string CorrelationId = "TestCorrelation";
        private static readonly Exception Exception = new Exception("Test");
        private const string Message = "Test message, p1: {0}, p2: {1}";
        private static readonly object[] MessageParams = { "param1", "param2" };

        [TestInitialize]
        public void Setup()
        {
            _correlation = new Correlation(CorrelationId, CallerId);
            _logger = Substitute.For<nLog.ILogger>();

            _repository = Substitute.For<ILoggerRepository>();
            _repository.GetLogger(_source.GetType()).Returns(_logger);

            _sut = new LogWriter(_repository, _source);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerRepositoryCannotBeNull()
        {
            _sut = new LogWriter(null, null);
        }

        [TestMethod]
        public void SourceTypeIsLogWriterItselfIfNullSourceIsPassedInConstructor()
        {
            _sut = new LogWriter(_repository, null);
            _sut.WriteLog(_correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            _repository.Received().GetLogger(typeof(LogWriter));
        }

        [TestMethod]
        public void SourceTypeIsUsedToGetLogger()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Information, Exception, Message, MessageParams);
            _repository.Received().GetLogger(typeof(StringBuilder));
        }

        [TestMethod]
        public void VerboseLevelStateIsCheckedAfterCreatingLogger()
        {
            _logger.IsDebugEnabled.Returns(false);

            _repository
                .When(r => r.GetLogger(Arg.Any<Type>()))
                .Do(callInfo => _logger.IsDebugEnabled.Returns(true));

            _sut.WriteLog(_correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void VerboseEventsAreNotLoggedIfVerboseLevelIsDisabled()
        {
            _logger.IsDebugEnabled.Returns(false);
            _sut.WriteLog(_correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            _logger.DidNotReceive().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void InformationEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            _logger.IsDebugEnabled.Returns(false);
            _sut.WriteLog(_correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void WarningEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            _logger.IsDebugEnabled.Returns(false);
            _sut.WriteLog(_correlation, LogEventLevel.Warning, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void ErrorEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            _logger.IsDebugEnabled.Returns(false);
            _sut.WriteLog(_correlation, LogEventLevel.Error, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void CriticalEventsAreLoggedEvenIfVerboseLevelIsDisabled()
        {
            _logger.IsDebugEnabled.Returns(false);
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Any<nLog.LogEventInfo>());
        }

        [TestMethod]
        public void LoggerIsQueriedFromRepositoryOnlyOnce()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            _repository.Received(1).GetLogger(Arg.Any<Type>());
        }

        [TestMethod]
        public void MessageIsFormattedCorrectly()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            const string expectedMessage = "Test message, p1: param1, p2: param2";

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.FormattedMessage.Equals(expectedMessage)));
        }

        [TestMethod]
        public void NullMessageParamsArrayIsIgnored()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, null);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Message.Equals(Message)));
        }

        [TestMethod]
        public void EmptyMessageParamsArrayIsIgnored()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, new object[0]);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Message.Equals(Message)));
        }

        [TestMethod]
        public void CorrelationCanBeNull()
        {
            _sut.WriteLog(null, LogEventLevel.Critical, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => !e.Properties.Keys.Contains("CallerId") && !e.Properties.Keys.Contains("CorrelationId")));
        }

        [TestMethod]
        public void CallerIdIsLogged()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Properties["CallerId"].Equals(CallerId)));
        }

        [TestMethod]
        public void CorrelationIdIsLogged()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Properties["CorrelationId"].Equals(CorrelationId)));
        }

        [TestMethod]
        public void ExceptionIsLogged()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Exception == Exception));
        }

        [TestMethod]
        public void VerboseLevelIsLogged()
        {
            _logger.IsDebugEnabled.Returns(true);
            _sut.WriteLog(_correlation, LogEventLevel.Verbose, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Debug));
        }

        [TestMethod]
        public void InformationLevelIsLogged()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Information, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Info));
        }

        [TestMethod]
        public void WarningLevelIsLogged()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Warning, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Warn));
        }

        [TestMethod]
        public void ErrorLevelIsLogged()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Error, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Error));
        }

        [TestMethod]
        public void CriticalLevelIsLogged()
        {
            _sut.WriteLog(_correlation, LogEventLevel.Critical, Exception, Message, MessageParams);

            _logger.Received().Log(Arg.Is<nLog.LogEventInfo>(e => e.Level == nLog.LogLevel.Fatal));
        }
    }
}
