using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration.Steps
{
    [Binding]
    public class GetFilesSteps
    {
        protected IEnumerable<ArchiveMember> Members { get; set; }

        [When(@"I call the GetFiles method")]
        public void WhenICallTheGetFilesMethod()
        {
            Members = UnrarWrapperSteps.Wrapper.GetFiles(UnrarWrapperSteps.Handle);
        }

        [Then(@"I should get the following list back")]
        public void ThenIShouldGetTheFollowingListBack(Table table)
        {
            var count = 0;
            foreach (var row in table.Rows)
            {
                Assert.That(Members.ElementAt(count).HighFlags, Is.EqualTo(Enum.Parse(typeof(HighMemberFlags), row["HighFlags"])));
                Assert.That(Members.ElementAt(count).LastModificationDate, Is.EqualTo(DateTime.Parse(row["LastModificationDate"])));
                Assert.That(Members.ElementAt(count).LowFlags, Is.EqualTo(Enum.Parse(typeof(LowMemberFlags), row["LowFlags"])));
                Assert.That(Members.ElementAt(count).Name, Is.EqualTo(row["Name"]));
                Assert.That(Members.ElementAt(count).PackedSize, Is.EqualTo(long.Parse(row["PackedSize"])));
                Assert.That(Members.ElementAt(count).UnpackedSize, Is.EqualTo(long.Parse(row["UnpackedSize"])));
                Assert.That(Members.ElementAt(count).Volume, Is.EqualTo(row["Volume"]));
                count++;
            }
        }
    }
}