using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration.Steps
{
    [Binding]
    public class GetFilesSteps
    {
        internal static IEnumerable<ArchiveMember> Members { get; set; }

        [When(@"I call the GetFiles method")]
        public void WhenICallTheGetFilesMethod()
        {
            Members = UnrarWrapperSteps.Wrapper.GetFiles(UnrarWrapperSteps.Handle);
        }

        [Then(@"I should get the following list back")]
        public void ThenIShouldGetTheFollowingListBack(Table table)
        {
            var members = table.CreateSet<ArchiveMember>().ToArray();

            for (var count = 0; count < members.Length; count++)
            {
                Assert.That(Members.ElementAt(count).HighFlags, Is.EqualTo(members[count].HighFlags));
                Assert.That(Members.ElementAt(count).LastModificationDate, Is.EqualTo(members[count].LastModificationDate));
                Assert.That(Members.ElementAt(count).LowFlags, Is.EqualTo(members[count].LowFlags));
                Assert.That(Members.ElementAt(count).Name, Is.EqualTo(members[count].Name));
                Assert.That(Members.ElementAt(count).PackedSize, Is.EqualTo(members[count].PackedSize));
                Assert.That(Members.ElementAt(count).UnpackedSize, Is.EqualTo(members[count].UnpackedSize));
                Assert.That(Members.ElementAt(count).Volume, Is.EqualTo(members[count].Volume));
            }
        }
    }
}