using System;
using System.Globalization;
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

        [When(@"I call the the Open method with (.*) archive path for (.*)")]
        public void WhenICallTheTheOpenMethod(string path, string type)
        {
            OpenMode mode;
            if (string.Compare(type, "List", true, CultureInfo.InvariantCulture) == 0)
            {
                mode = OpenMode.List;
            }
            else if (string.Compare(type, "Extract", true, CultureInfo.InvariantCulture) == 0)
            {
                mode = OpenMode.Extract;
            }
            else
            {
                throw new InvalidOperationException(string.Format("The mode {0} is invalid.", type));
            }
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
