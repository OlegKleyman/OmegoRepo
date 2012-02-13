using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests.Integration
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
        public void CanConvertFromStringTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.CanConvertFrom(null, typeof(string));
            Assert.IsTrue(result);
        }

        [Test]
        public void CanConvertFromIntTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.CanConvertFrom(null, typeof(int));
            Assert.IsFalse(result);
        }

        [Test]
        public void CanConvertToStringTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.CanConvertTo(null, typeof(string));
            Assert.IsTrue(result);
        }

        [Test]
        public void CanConvertToIntTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.CanConvertTo(null, typeof(int));
            Assert.IsFalse(result);
        }

        [Test]
        public void ConvertFromStringTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.ConvertFrom("Oleg.Kleyman.Core.FileSystem");
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<FileSystem>(result);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(NotSupportedException),
                           ExpectedExceptionName = "System.NotSupportedException",
                           ExpectedMessage = "ReflectiveStringTypeConverter cannot convert from System.Int32.",
                           MatchType = MessageMatch.Exact)]
        public void ConvertFromIntTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            converter.ConvertFrom(0);
        }

        [Test]
        public void ConvertToStringTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            var result = converter.ConvertTo(null, null, default(int), typeof (string));
            Assert.IsInstanceOf<string>(result);
            Assert.AreEqual("System.Int32", result);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(NotSupportedException),
                           ExpectedExceptionName = "System.NotSupportedException",
                           ExpectedMessage = "'ReflectiveStringTypeConverter' is unable to convert 'System.Double' to 'System.Int32'.",
                           MatchType = MessageMatch.Exact)]
        public void ConvertToIntTest()
        {
            var converter = new ReflectiveStringTypeConverter();
            converter.ConvertTo(null, null, default(double), typeof(int));
        }


    }
}
