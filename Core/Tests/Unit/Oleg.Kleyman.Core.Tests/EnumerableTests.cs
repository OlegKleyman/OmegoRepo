using System.Collections.Generic;
using NUnit.Framework;
using Oleg.Kleyman.Core.Linq;
using Oleg.Kleyman.Tests.Core;
using Enumerable = System.Linq.Enumerable;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class EnumerableTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckCoverage()
        {
            var knownMembers = new Dictionary<string, int>
                                   {
                                       {"Distinct", 1}
                                   };

            var coverageAnalyzer = new CoverageAnalyzer(typeof (Enumerable));

            bool result = coverageAnalyzer.ValidateMembers(knownMembers);
            if (!result)
            {
                const string membersNotCoveredMessage = "All members not covered";
                Assert.Inconclusive(membersNotCoveredMessage);
            }
        }

        [Test]
        public void DistinctTest()
        {
            var values = new[] {"Test1", "Test2", "Test3", "Test4", "Test2", "Test5", "Test3", "Test2"};
            IEnumerable<string> distinctValues = values.Distinct((x, y) => x == y);
            string[] indexedValues = Enumerable.ToArray(distinctValues);
            Assert.AreEqual(5, indexedValues.Length);
            Assert.AreEqual("Test1", indexedValues[0]);
            Assert.AreEqual("Test2", indexedValues[1]);
            Assert.AreEqual("Test3", indexedValues[2]);
            Assert.AreEqual("Test4", indexedValues[3]);
            Assert.AreEqual("Test5", indexedValues[4]);
        }
    }
}