using System;
using System.IO;
using Oleg.Kleyman.Winrar.Interop;
using TechTalk.SpecFlow;

namespace Oleg.Kleyman.Winrar.Core.Tests.Integration.Steps
{
    [Binding]
    public class UnrarWrapperSteps
    {
        internal static NativeMethods UnrarDll { get; set; }
        internal static IUnrarWrapper Wrapper { get; set; }
        internal static IntPtr Handle { get; set; }
        internal static RarStatus Status { get; set; }

        [Given(@"I have an instance of the NativeMethods object")]
        public void GivenIHaveAnInstanceOfTheNativeMethodsObject()
        {
            UnrarDll = new NativeMethods();
        }

        [Given(@"I instantiate an UnrarWrapper object")]
        public void GivenIInstantiateAnUnrarWrapperObject()
        {
            Wrapper = new UnrarWrapper(UnrarDll);
        }

        [When(@"I call the the Open method with (.*) archive path")]
        public void WhenICallTheTheOpenMethod(string path)
        {
            Handle = Wrapper.Open(Path.GetFullPath(path), OpenMode.List);
        }

        [When(@"I call the Close method")]
        public void WhenICallTheCloseMethod()
        {
            Status = Wrapper.Close(Handle);
        }
    }
}
