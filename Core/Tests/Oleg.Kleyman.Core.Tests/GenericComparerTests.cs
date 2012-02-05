using System;
using System.Collections.Generic;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class GenericComparerTests
    {
        private const string PARAMETER_NAME_Y = "y";
        private const string EXPECTED_ARGUMENT_NULL_EXCEPTION_NAME = "System.ArgumentNullException";
        private const string EXPECTED_INVALID_OPERATION_EXCEPTION_NAME = "System.InvalidOperationException";
        private const string PARAMETER_NAME_X = "x";
        private const string COMPARER_NAME = "ComparerHandler ";
        private const string CANNOT_BE_NULL_EXCEPTION_MESSAGE = "Cannot be null";
        private const string PARAMETER_NAME_EXCEPTION_MESSAGE = "\r\nParameter name: ";

        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckCoverage()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {".ctor", 1},
                                       {"Equals", 1},
                                       {"GetHashCode", 1},
                                       {"CompareHandler", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (EqualityComparer<>));

            bool result = coverageAnalyzer.ValidateMembers(knownMembers);
            if (!result)
            {
                const string membersNotCoveredMessage = "All members not covered";
                Assert.Inconclusive(membersNotCoveredMessage);
            }
        }

        [Test]
        public void ConstructorTest()
        {
            var compareHandler = new Func<double, double, bool>((x, y) => true);
            var comparer = new EqualityComparer<double>(compareHandler);
            Assert.AreEqual(compareHandler, comparer.CompareHandler);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException),
            ExpectedExceptionName = EXPECTED_INVALID_OPERATION_EXCEPTION_NAME,
            ExpectedMessage = COMPARER_NAME + CANNOT_BE_NULL_EXCEPTION_MESSAGE,
            MatchType = MessageMatch.Exact)]
        public void EqualsComparerNullTest()
        {
            var comparer = new EqualityComparer<object>(null);
            comparer.Equals(3.9999, 3.9999);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            ExpectedExceptionName = EXPECTED_ARGUMENT_NULL_EXCEPTION_NAME,
            ExpectedMessage = CANNOT_BE_NULL_EXCEPTION_MESSAGE + PARAMETER_NAME_EXCEPTION_MESSAGE + PARAMETER_NAME_X,
            MatchType = MessageMatch.Exact)]
        public void EqualsFirstArgumentNullExceptionTest()
        {
            var comparer = new EqualityComparer<object>(null);
            comparer.Equals(null, 3.9999);
        }

        [Test]
        public void EqualsNegativePositiveResultsAreNotEqualTest()
        {
            var compareHandler = new Func<double, double, bool>((x, y) => Math.Abs(x - y) < double.Epsilon);
            var comparer = new EqualityComparer<double>(compareHandler);
            bool result = comparer.Equals(-3.9999, 3.9999);
            Assert.IsFalse(result);
        }

        [Test]
        public void EqualsNegativeResultsAreEqualTest()
        {
            var compareHandler = new Func<double, double, bool>((x, y) => Math.Abs(x - y) < double.Epsilon);
            var comparer = new EqualityComparer<double>(compareHandler);
            bool result = comparer.Equals(-3.9999, -3.9999);
            Assert.IsTrue(result);
        }

        [Test]
        public void EqualsNegativeResultsAreNotEqualTest()
        {
            var compareHandler = new Func<double, double, bool>((x, y) => Math.Abs(x - y) < double.Epsilon);
            var comparer = new EqualityComparer<double>(compareHandler);
            bool result = comparer.Equals(-3.9999, -3.9998);
            Assert.IsFalse(result);
        }

        [Test]
        public void EqualsResultsAreEqualTest()
        {
            var compareHandler = new Func<double, double, bool>((x, y) => Math.Abs(x - y) < double.Epsilon);
            var comparer = new EqualityComparer<double>(compareHandler);
            bool result = comparer.Equals(3.9999, 3.9999);
            Assert.IsTrue(result);
        }

        [Test]
        public void EqualsResultsAreNotEqualTest()
        {
            var compareHandler = new Func<double, double, bool>((x, y) => Math.Abs(x - y) < double.Epsilon);
            var comparer = new EqualityComparer<double>(compareHandler);
            bool result = comparer.Equals(3.9999, 3.9998);
            Assert.IsFalse(result);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            ExpectedExceptionName = EXPECTED_ARGUMENT_NULL_EXCEPTION_NAME,
            ExpectedMessage = CANNOT_BE_NULL_EXCEPTION_MESSAGE + PARAMETER_NAME_EXCEPTION_MESSAGE + PARAMETER_NAME_Y,
            MatchType = MessageMatch.Exact)]
        public void EqualsSecondArgumentNullExceptionTest()
        {
            var comparer = new EqualityComparer<object>(null);
            comparer.Equals(3.9999, null);
        }

        [Test]
        public void GetHashCodeDoubleTest()
        {
            var comparer = new EqualityComparer<double>(null);
            int result = comparer.GetHashCode(1.12);
            Assert.AreEqual(558479977, result);
        }

        [Test]
        public void GetHashCodeIntTest()
        {
            var comparer = new EqualityComparer<int>(null);
            int result = comparer.GetHashCode(1);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetHashCodeReferenceTest()
        {
            var comparer = new EqualityComparer<object>(null);
            int result = comparer.GetHashCode(new MockObject());
            Assert.AreEqual(1337, result);
        }
    }
}