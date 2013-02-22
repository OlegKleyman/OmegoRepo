using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration.Steps
{
    [Binding]
    public class OpenCloseSteps
    {
        [Then(@"I should receive a greater than (.*) IntPtr handle back")]
        public void ThenIShouldReceiveAGreaterThanIntPtrHandleBack(int minimumHandle)
        {
            Assert.That(UnrarWrapperSteps.Handle.ToInt32(), Is.GreaterThan(0));
        }

        [Then(@"It should return a success value back")]
        public void ThenItShouldReturnASuccessValueBack()
        {
            Assert.That(UnrarWrapperSteps.Status, Is.EqualTo(RarStatus.Success));
        }
    }
}
