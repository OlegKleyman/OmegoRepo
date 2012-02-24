using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class ReflectiveStringTypeConverterTests : TestsBase
    {
        #region Overrides of TestsBase

        public override void Setup()
        {
        }

        #endregion

        [Test]
        public void ConvertToTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.ConvertTo(null, null, 0, typeof(string));
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual("System.Int32", result);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(NotSupportedException),
                           ExpectedExceptionName = "System.NotSupportedException",
                           ExpectedMessage = "'ReflectiveStringTypeConverter' is unable to convert 'System.Int32' to 'System.Int32'.",
                           MatchType = MessageMatch.Exact)]
        public void ConvertToNotSupportedTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            converter.ConvertTo(null, null, 0, typeof(int));
        }

        [Test]
        public void ConvertFromTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.ConvertFrom(null, null, "System.Collections.Generic.List`1[[System.String]]");
            Assert.IsInstanceOf<List<string>>(result);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(NotSupportedException),
                           ExpectedExceptionName = "System.NotSupportedException",
                           ExpectedMessage = "ReflectiveStringTypeConverter cannot convert from System.Int32.",
                           MatchType = MessageMatch.Exact)]
        public void ConvertFromNotSupportedTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            converter.ConvertFrom(null, null, 0);
        }

        [Test]
        public void CanConvertToTrueTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.CanConvertTo(null, typeof(string));
            Assert.IsTrue(result);
        }

        [Test]
        public void CanConvertFromTrueTest()
        {
            var converter = new ReflectiveStringTypeConverter();

            var result = converter.CanConvertFrom(null, typeof(string));
            Assert.IsTrue(result);
        }

        [Test]
        public void CanConvertToFalseTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.CanConvertTo(null, typeof(int));
            Assert.IsFalse(result);
        }

        [Test]
        public void CanConvertFromFalseTest()
        {
            var converter = new ReflectiveStringTypeConverter();

            var result = converter.CanConvertFrom(null, typeof(int));
            Assert.IsFalse(result);
        }
    }
}
