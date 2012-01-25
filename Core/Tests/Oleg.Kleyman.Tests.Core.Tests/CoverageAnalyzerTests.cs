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
            var knownMethods = new Dictionary<string, int> { { "Equals", 2 }, { "GetHashCode", 1 }, { "GetType", 1 }, { "ToString", 1 }, { "ReferenceEquals", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<object>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidatePropertiesTest()
        {
            var knownProperties = new Dictionary<string, int> { { "Baz", 1 }, { "Qux", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<TestClass>();

            var result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateMethodsChildClassTest()
        {
            var knownMethods = new Dictionary<string, int> { { "Foo", 1 }, { "Bar", 1 }, { "CompareTo", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<TestClass>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidatePropertiesChildClassTest()
        {
            var knownProperties = new Dictionary<string, int> { { "Baz", 1 }, { "Quux", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<TestClassTwo>();

            var result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateMethodsOverrideTest()
        {
            var knownMethods = new Dictionary<string, int> { { "Foo", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<TestClassTwo>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidatePropertyOverrideTest()
        {
            var knownProperties = new Dictionary<string, int> { { "Baz", 1 }, { "Quux", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<TestClassTwo>();

            var result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateAllTest()
        {
            var knownMembers = new Dictionary<string, int>
                                      {
                                          { "Foo", 1 }, 
                                          { "Bar", 1 }, 
                                          { "Baz", 1 }, 
                                          { "Qux", 1 }, 
                                          { "CompareTo", 1 }
                                      };

            var coverageAnalyzer = new CoverageAnalyzer<TestClass>();

            var result = coverageAnalyzer.ValidateMembers(knownMembers);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateMethodsMethodMissingTest()
        {
            var knownMethods = new Dictionary<string, int> { { "Equals", 2 }, { "GetType", 1 }, { "ToString", 1 }, { "ReferenceEquals", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<object>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMethodsMethodCountIncorrectTest()
        {
            var knownMethods = new Dictionary<string, int> { { "Equals", 2 }, { "GetHashCode", 2 }, { "GetType", 1 }, { "ToString", 1 }, { "ReferenceEquals", 1 } };

            var coverageAnalyzer = new CoverageAnalyzer<object>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMethodsExtraMethodTest()
        {
            var knownMethods = new Dictionary<string, int> { { "Equals", 2 }, { "GetHashCode", 1 }, { "GetType", 1 }, { "ToString", 1 }, { "ReferenceEquals", 1 }, {"IsNull", 1} };

            var coverageAnalyzer = new CoverageAnalyzer<object>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsFalse(result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), 
                           ExpectedExceptionName = "System.ArgumentNullException",
                           ExpectedMessage = "Argument cannot be null or nothing.\r\nParameter name: names",
                           MatchType = MessageMatch.Exact,
                           UserMessage = "dsds",
                           Handler = "ArgumentNullExceptionHandler")]
        public void ValidateMethodsDictionaryArguementNullExceptionTest()
        {
            IDictionary<string, int> knownMethods = null;

            var coverageAnalyzer = new CoverageAnalyzer<object>();

            var result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsFalse(result);
        }

        public void ArgumentNullExceptionHandler(Exception exception)
        {
            if(exception.GetType() != typeof(ArgumentNullException))
            {
                const string invalidExceptionType = "This handler can only be used with an ArgumentNullException";
                throw new InvalidOperationException(invalidExceptionType);
            }
            var argumentNullException = (ArgumentNullException) exception;
            Assert.AreEqual("names", argumentNullException.ParamName);
        }
    }
}