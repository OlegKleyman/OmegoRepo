using System;
using System.Security;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class SecureStringTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {

        }

        #endregion

        [Test]
        public void ToUnsecureStringShouldConvertCorrectly()
        {
            var secureString = new SecureString();
            secureString.AppendChar('t');
            secureString.AppendChar('e');
            secureString.AppendChar('s');
            secureString.AppendChar('t');
            secureString.MakeReadOnly();

            var result = secureString.ToUnsecureString();
            Assert.AreEqual("test", result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), 
                           ExpectedMessage = "Value cannot be null.\r\nParameter name: secureString", 
                           MatchType = MessageMatch.Exact)]
        public void ToUnsecureStringShouldThrowExceptionWhenArgumentIsNull()
        {
            SecureStringExtensions.ToUnsecureString(null);
        }
    }
}