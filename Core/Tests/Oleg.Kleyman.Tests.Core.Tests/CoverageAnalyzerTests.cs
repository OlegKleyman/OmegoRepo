using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Oleg.Kleyman.Tests.Core.Tests
{
    [TestFixture]
    public class CoverageAnalyzerTests
    {
        private object TestObject { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            TestObject = new object();
        }

        [Test]
        public void ValidateMethodsTest()
        {
            var knownMethods = new[] { "Equals", "GetHashCode", "GetType", "ToString", "ReferenceEquals" };

            var coverageAnalyzer = new CoverageAnalyzer<object>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateMethodsChildClassTest()
        {
            var knownMethods = new[] { "Foo", "Bar", "CompareTo" };

            var coverageAnalyzer = new CoverageAnalyzer<TestClass>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidatePropertiesTest()
        {
            var knownProperties = new[] { "Baz", "Qux" };

            var coverageAnalyzer = new CoverageAnalyzer<TestClass>();

            var result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsTrue(result);
        }
    }
}