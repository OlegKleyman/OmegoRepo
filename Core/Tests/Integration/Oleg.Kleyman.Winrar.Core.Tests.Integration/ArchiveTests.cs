using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration
{
    [TestFixture]
    public class ArchiveTests : TestsBase
    {
        private IUnrarDll UnrarDll { get; set; }

        #region Overrides of TestsBase

        public override void Setup()
        {
            UnrarDll = new UnrarDll();
        }

        #endregion
    }
}
