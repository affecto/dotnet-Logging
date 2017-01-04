using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using nLog = NLog;

namespace Affecto.Logging.NLog.Tests
{
    [TestClass]
    public class LoggerRepositoryTests
    {
        private LoggerRepository _sut;
        private NLogWrapper _wrapper;

        [TestInitialize]
        public void Setup()
        {
            _wrapper = Substitute.For<NLogWrapper>();
            _sut = new LoggerRepository(_wrapper);
        }

        [TestMethod]
        public void ConfigurationIsDoneOnce()
        {
            _sut.GetLogger(typeof(string));
            _sut.GetLogger(typeof(int));
            _sut.GetLogger(typeof(int));

            _wrapper.Received(1).Configure();
        }

        [TestMethod]
        public void LoggerInstanceIsQueriedFromWrapper()
        {
            var logger = Substitute.For<nLog.ILogger>();
            _wrapper.GetLogger(typeof(string)).Returns(logger);

            var firstLogger = _sut.GetLogger(typeof(string));

            Assert.AreSame(logger, firstLogger);
        }

        [TestMethod]
        public void SameLoggerInstanceIsReturnedFromMultipleCalls()
        {
            var log = Substitute.For<nLog.ILogger>();
            _wrapper.GetLogger(typeof(string)).Returns(log);

            var firstLogger = _sut.GetLogger(typeof(string));
            var secondLogger = _sut.GetLogger(typeof(string));

            Assert.AreSame(firstLogger, secondLogger);
            _wrapper.Received(1).GetLogger(typeof(string));
        }
    }
}