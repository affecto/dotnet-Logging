using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.Logging.Log4Net.Tests
{
    [TestClass]
    public class LoggerRepositoryTests
    {
        private LoggerRepository sut;
        private Log4NetWrapper wrapper;

        [TestInitialize]
        public void Setup()
        {
            wrapper = Substitute.For<Log4NetWrapper>();
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
            ILog log = Substitute.For<ILog>();
            wrapper.GetLog(typeof(string)).Returns(log);

            ILog firstLogger = sut.GetLogger(typeof(string));

            Assert.AreSame(log, firstLogger);
        }

        [TestMethod]
        public void SameLoggerInstanceIsReturnedFromMultipleCalls()
        {
            ILog log = Substitute.For<ILog>();
            wrapper.GetLog(typeof(string)).Returns(log);

            ILog firstLogger = sut.GetLogger(typeof(string));
            ILog secondLogger = sut.GetLogger(typeof(string));

            Assert.AreSame(firstLogger, secondLogger);
            wrapper.Received(1).GetLog(typeof(string));
        }
    }
}