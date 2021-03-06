﻿using NUnit.Framework;

namespace Oleg.Kleyman.Tests.Core.Tests
{
    [TestFixture]
    public class MockDirectoryTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void DeleteTest()
        {
            var mockDirectory = new MockDirectory(@"C:\test");
            mockDirectory.Delete();
            Assert.Pass("Test passes at this point");
        }

        [Test]
        public void ExistsTest()
        {
            var mockDirectory = new MockDirectory(@"C:\test");
            Assert.IsTrue(mockDirectory.Exists);
        }

        [Test]
        public void FullNameTest()
        {
            var mockDirectory = new MockDirectory(@"C:\test");
            Assert.AreEqual(@"C:\test", mockDirectory.FullName);
        }

        [Test]
        public void NameTest()
        {
            var mockDirectory = new MockDirectory(@"C:\test");
            Assert.AreEqual("test", mockDirectory.Name);
        }
    }
}