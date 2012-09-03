using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class OutputTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {
            
        }

        #endregion

        [Test]
        public void ConstructorTest()
        {
            var output = new Output("test.txt", "C:\\testOutput");
            Assert.AreEqual("test.txt", output.FileName);
            Assert.AreEqual("C:\\testOutput", output.TargetDirectory);
        }
    }
}
