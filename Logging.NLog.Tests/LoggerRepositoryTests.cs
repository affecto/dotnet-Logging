using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using nLog = NLog;

namespace Affecto.Logging.NLog.Tests
{
    [TestClass]
    public class LoggerRepositoryTests
    {
        private LoggerRepository sut;
        private NLogWrapper wrapper;

        [TestInitialize]
        public void Setup()
        {
            wrapper = Substitute.For<NLogWrapper>();
            sut = new LoggerRepository(wrapper);
        }

        [TestMethod]
        public void ConfigurationIsDoneOnce()
        {
            sut.GetLogger(typeof(string));
            sut.GetLogger(typeof(int));
            sut.GetLogger(typeof(int));

            wrapper.Received(1).Configure();
        }

        [TestMethod]
        public void LoggerInstanceIsQueriedFromWrapper()
        {
            var logger = Substitute.For<nLog.ILogger>();
            wrapper.GetLogger(typeof(string)).Returns(logger);

            var firstLogger = sut.GetLogger(typeof(string));

            Assert.AreSame(logger, firstLogger);
        }

        [TestMethod]
        public void SameLoggerInstanceIsReturnedFromMultipleCalls()
        {
            var log = Substitute.For<nLog.ILogger>();
            wrapper.GetLogger(typeof(string)).Returns(log);

            var firstLogger = sut.GetLogger(typeof(string));
            var secondLogger = sut.GetLogger(typeof(string));

            Assert.AreSame(firstLogger, secondLogger);
            wrapper.Received(1).GetLogger(typeof(string));
        }
    }
}