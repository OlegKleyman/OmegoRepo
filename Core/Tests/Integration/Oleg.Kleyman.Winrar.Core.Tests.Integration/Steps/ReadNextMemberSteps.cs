using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration.Steps
{
    [Binding]
    public class ReadNextMemberSteps
    {
        [When(@"I call the GetNextMember method")]
        public void WhenICallTheGetNextMemberMethod()
        {
            var member = UnrarWrapperSteps.Wrapper.GetNextMember(UnrarWrapperSteps.Handle);
            GetFilesSteps.Members = new List<ArchiveMember>
                {
                    member
                };
        }
    }
}
