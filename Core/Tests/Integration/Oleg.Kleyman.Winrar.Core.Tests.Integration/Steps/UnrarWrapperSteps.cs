using System;
using System.Globalization;
using System.IO;
using Oleg.Kleyman.Core;
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
            var pathBuilder = new PathBuilder();
            var destinationBuilder = new DestinationPathBuilder(pathBuilder);
            Wrapper = new UnrarWrapper(UnrarDll, destinationBuilder);
        }

        [When(@"I call the the Open method with (.*) archive path for (Extract|List)")]
        public void WhenICallTheTheOpenMethod(string path, OpenMode mode)
        {
            Handle = Wrapper.Open(Path.GetFullPath(path), mode);
        }

        [When(@"I call the Close method")]
        public void WhenICallTheCloseMethod()
        {
            Status = Wrapper.Close(Handle);
            Handle = IntPtr.Zero;
        }

        [AfterTestRun]
        public static void Teardown()
        {
            if (Handle != IntPtr.Zero)
            {
                Wrapper.Close(Handle);
            }
        }
    }
}
