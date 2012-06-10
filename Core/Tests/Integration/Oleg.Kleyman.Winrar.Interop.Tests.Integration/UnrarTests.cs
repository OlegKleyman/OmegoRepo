using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Interop.Tests.Integration
{
    [TestFixture]
    public class UnrarTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {
            
        }

        #endregion

        [Test]
        public void OpenTest()
        {
            var archive = Unrar.Open(@"C:\test\test.rar");

            Assert.IsNotNull(archive);
            
            archive.Close();
        }
    }
}
