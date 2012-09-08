using System.Diagnostics;
using NUnit.Framework;

namespace Oleg.Kleyman.Core.Tests.Integration
{
    [TestFixture]
    public class ProcessManagerTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [Test]
        public void StartTest()
        {
            var processManager = new ProcessManager();
            var processInfo = new ProcessStartInfo("cmd")
                                  {
                                      CreateNoWindow = true,
                                      WindowStyle = ProcessWindowStyle.Hidden
                                  };
            var process = processManager.Start(processInfo);
            Assert.IsInstanceOf<SystemProcess>(process);
            Assert.IsFalse(process.HasExited);
            Assert.AreEqual(ProcessPriorityClass.Normal, process.PriorityClass);
            process.Kill();
            process.WaitForExit();
            Assert.IsTrue(process.HasExited);
        }
    }
}