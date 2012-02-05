using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Oleg.Kleyman.Tests.Core.Tests
{
    [TestFixture]
    public class CoverageAnalyzerTests : TestsBase
    {
        private const string ARGUMENT_NULL_EXCEPTION_NAME = "System.ArgumentNullException";

        private const string ARGUMENT_NULL_EXCEPTION_NAMES_EXPECTED_MESSAGE =
            "Argument cannot be null or nothing.\r\nParameter name: names";

        private const string ARGUMENT_NULL_EXCEPTION_HANDLER = "ArgumentNullExceptionHandler";
        private object TestObject { get; set; }

        public override void Setup()
        {
            TestObject = new object();
        }

        public override void CheckCoverage()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {".ctor", 1},
                                       {"ValidateMembersNoCoverage", 2},
                                       {"ValidateMethods", 1},
                                       {"ValidateProperties", 1}
                                   };

            CoverageAnalyzer.ValidateMembersNoCoverage<CoverageAnalyzer>(knownMembers, true);
        }

        public void ArgumentNullExceptionHandler(Exception exception)
        {
            if (exception.GetType() != typeof (ArgumentNullException))
            {
                const string invalidExceptionType = "This handler can only be used with an ArgumentNullException";
                throw new InvalidOperationException(invalidExceptionType);
            }
            var argumentNullException = (ArgumentNullException) exception;
            Assert.AreEqual("names", argumentNullException.ParamName);
        }

        [Test]
        public void ConstructorTest()
        {
            new CoverageAnalyzer(typeof (object));
        }

        [Test]
        public void ValidateAllMembers()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {".ctor", 1},
                                       {"Foo", 1},
                                       {"Bar", 1},
                                       {"Baz", 1},
                                       {"Qux", 1},
                                       {"CompareTo", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClass));

            bool result = coverageAnalyzer.ValidateMembers(knownMembers);
            Assert.IsTrue(result);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            ExpectedExceptionName = ARGUMENT_NULL_EXCEPTION_NAME,
            ExpectedMessage = ARGUMENT_NULL_EXCEPTION_NAMES_EXPECTED_MESSAGE,
            MatchType = MessageMatch.Exact,
            Handler = ARGUMENT_NULL_EXCEPTION_HANDLER)]
        public void ValidateMembersDictionaryArgumentNullExceptionTest()
        {
            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMembers(null);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMembersExtraMemberTest()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {"Equals", 2},
                                       {"GetHashCode", 1},
                                       {"GetType", 1},
                                       {"ToString", 1},
                                       {"ReferenceEquals", 1},
                                       {"IsNull", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMembers(knownMembers);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMembersMemberCountIncorrectTest()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {"Equals", 2},
                                       {"GetHashCode", 2},
                                       {"GetType", 1},
                                       {"ToString", 1},
                                       {"ReferenceEquals", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMembers(knownMembers);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMembersMemberMissingTest()
        {
            var knownMembers = new Dictionary<string, int>
                                   {{"Equals", 2}, {"GetType", 1}, {"ToString", 1}, {"ReferenceEquals", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMembers(knownMembers);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMembersStaticExtraMethodTest()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {"Equals", 2},
                                       {"NoneExistingMethod", 1},
                                       {"GetHashCode", 2},
                                       {"GetType", 1},
                                       {"ToString", 1},
                                       {"ReferenceEquals", 1}
                                   };

            bool result = CoverageAnalyzer.ValidateMembersNoCoverage<object>(knownMembers, false);

            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMembersStaticMethodCountIncorrectTest()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {"Equals", 2},
                                       {"GetHashCode", 2},
                                       {"GetType", 1},
                                       {"ToString", 1},
                                       {"ReferenceEquals", 1}
                                   };

            bool result = CoverageAnalyzer.ValidateMembersNoCoverage<object>(knownMembers, false);

            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMembersStaticMethodMissingTest()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {".ctor", 1},
                                       {"Foo", 1},
                                       {"Bar", 1},
                                       {"Baz", 1},
                                       {"Qux", 1}
                                   };
            bool result = CoverageAnalyzer.ValidateMembersNoCoverage<TestClass>(knownMembers, false);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMembersStaticTest()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {".ctor", 1},
                                       {"Foo", 1},
                                       {"Bar", 1},
                                       {"Baz", 1},
                                       {"Qux", 1},
                                       {"CompareTo", 1}
                                   };
            bool result = CoverageAnalyzer.ValidateMembersNoCoverage<TestClass>(knownMembers, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateMethodsChildClassTest()
        {
            var knownMethods = new Dictionary<string, int> {{"Foo", 1}, {"Bar", 1}, {"CompareTo", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClass));

            bool result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            ExpectedExceptionName = ARGUMENT_NULL_EXCEPTION_NAME,
            ExpectedMessage = ARGUMENT_NULL_EXCEPTION_NAMES_EXPECTED_MESSAGE,
            MatchType = MessageMatch.Exact,
            Handler = ARGUMENT_NULL_EXCEPTION_HANDLER)]
        public void ValidateMethodsDictionaryArgumentNullExceptionTest()
        {
            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMethods(null);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMethodsExtraMethodTest()
        {
            var knownMethods = new Dictionary<string, int>
                                   {
                                       {"Equals", 2},
                                       {"GetHashCode", 1},
                                       {"GetType", 1},
                                       {"ToString", 1},
                                       {"ReferenceEquals", 1},
                                       {"IsNull", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMethodsMethodCountIncorrectTest()
        {
            var knownMethods = new Dictionary<string, int>
                                   {
                                       {"Equals", 2},
                                       {"GetHashCode", 2},
                                       {"GetType", 1},
                                       {"ToString", 1},
                                       {"ReferenceEquals", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMethodsMethodMissingTest()
        {
            var knownMethods = new Dictionary<string, int>
                                   {{"Equals", 2}, {"GetType", 1}, {"ToString", 1}, {"ReferenceEquals", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMethodsOverrideTest()
        {
            var knownMethods = new Dictionary<string, int> {{"Foo", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClassTwo));

            bool result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateMethodsTest()
        {
            var knownMethods = new Dictionary<string, int>
                                   {
                                       {"Equals", 2},
                                       {"GetHashCode", 1},
                                       {"GetType", 1},
                                       {"ToString", 1},
                                       {"ReferenceEquals", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateMethods(knownMethods);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidatePropertiesChildClassTest()
        {
            var knownProperties = new Dictionary<string, int> {{"Baz", 1}, {"Quux", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClassTwo));

            bool result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidatePropertiesExtraPropertyTest()
        {
            var knownProperties = new Dictionary<string, int> {{"Baz", 1}, {"NoneExistingProperty", 1}, {"Qux", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClass));

            bool result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidatePropertiesPropertyCountIncorrectTest()
        {
            var knownProperties = new Dictionary<string, int> {{"Baz", 2}, {"Qux", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClass));

            bool result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidatePropertiesPropertyMissingTest()
        {
            var knownProperties = new Dictionary<string, int> {{"Qux", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClass));

            bool result = coverageAnalyzer.ValidateProperties(knownProperties);

            Assert.IsFalse(result);
        }

        [Test]
        public void ValidatePropertiesTest()
        {
            var knownProperties = new Dictionary<string, int> {{"Baz", 1}, {"Qux", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClass));

            bool result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsTrue(result);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            ExpectedExceptionName = ARGUMENT_NULL_EXCEPTION_NAME,
            ExpectedMessage = ARGUMENT_NULL_EXCEPTION_NAMES_EXPECTED_MESSAGE,
            MatchType = MessageMatch.Exact,
            Handler = ARGUMENT_NULL_EXCEPTION_HANDLER)]
        public void ValidatePropertyDictionaryArgumentNullExceptionTest()
        {
            var coverageAnalyzer = new CoverageAnalyzer(typeof (object));

            bool result = coverageAnalyzer.ValidateProperties(null);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidatePropertyOverrideTest()
        {
            var knownProperties = new Dictionary<string, int> {{"Baz", 1}, {"Quux", 1}};

            var coverageAnalyzer = new CoverageAnalyzer(typeof (TestClassTwo));

            bool result = coverageAnalyzer.ValidateProperties(knownProperties);
            Assert.IsTrue(result);
        }
    }
}