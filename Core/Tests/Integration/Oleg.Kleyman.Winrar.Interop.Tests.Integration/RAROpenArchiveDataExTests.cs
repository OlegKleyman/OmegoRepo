using System;
using NUnit.Framework;
using Oleg.Kleyman.Tests.Core;

namespace Oleg.Kleyman.Winrar.Interop.Tests.Integration
{
    [TestFixture]
    public class RarOpenArchiveDataExTests : TestsBase
    {
        public override void Setup()
        {
        }

        [Test]
        public void DisposeShouldResetPointers()
        {
            var archiveData = new RAROpenArchiveDataEx();
            archiveData.UserData = new IntPtr(1337);
            archiveData.Dispose();
            Assert.That(archiveData.UserData, Is.EqualTo(IntPtr.Zero));
        }

        [Test]
        public void UserDataPropertyShouldBeSet()
        {
            var archiveData = new RAROpenArchiveDataEx();
            archiveData.UserData = new IntPtr(1337);
            Assert.That(archiveData.UserData, Is.EqualTo(new IntPtr(1337)));
        }
    }
}