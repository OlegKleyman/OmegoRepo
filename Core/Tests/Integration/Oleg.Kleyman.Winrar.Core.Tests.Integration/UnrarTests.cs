using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration
{
    [TestFixture]
    public class UnrarTests : TestsBase
    {
        private IUnrarDll UnrarDll { get; set; }

        #region Overrides of TestsBase

        public override void Setup()
        {
            UnrarDll = new UnrarDll();
        }

        #endregion

        [Test]
        public void OpenTest()
        {
        }

        
    }
}
