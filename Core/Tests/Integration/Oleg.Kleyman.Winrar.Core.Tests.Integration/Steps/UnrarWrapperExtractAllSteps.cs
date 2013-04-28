using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Tests.Core;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration.Steps
{
    [Binding]
    public class UnrarWrapperExtractAllSteps
    {
        private IEnumerable<IFileSystemMember> Members { get; set; }

        [When(@"I call extract all")]
        public void WhenICallExtractAll()
        {
            Members = UnrarWrapperSteps.Wrapper.ExtractAll(UnrarWrapperSteps.Handle, new FileSystemMemberFactory(new FileSystem()), @"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing");
        }

        [Then(@"I should receive the following list of FileSystemMembers back")]
        public void ThenIShouldReceiveTheFollowingListOfFileSystemMembersBack(Table table)
        {
            var members = table.CreateSet<MockFileSystemMember>().ToArray();
            members.ForEach(member => member.FullName = Path.GetFullPath(member.FullName));

            Assert.That(Members.Count(), Is.EqualTo(members.Length));

            for (var x = 0; x < members.Length; x++)
            {
                Assert.That(Members.ElementAt(x).Attributes, Is.EqualTo(members[x].Attributes));
                Assert.That(Members.ElementAt(x).Exists, Is.EqualTo(members[x].Exists));
                Assert.That(Members.ElementAt(x).FullName, Is.EqualTo(members[x].FullName));
            }
        }

        [AfterScenario("extraction")]
        public static void Teardown()
        {
            var fullPath = Path.GetFullPath(@"..\..\..\..\..\..\Common\Test\Oleg.Kleyman.Winrar.Core.Tests.Integration\Testing");
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }
        }
    }
}
