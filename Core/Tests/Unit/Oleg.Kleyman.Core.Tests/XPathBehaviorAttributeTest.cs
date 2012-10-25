using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Oleg.Kleyman.Core.Tests
{
    [TestFixture]
    public class XPathBehaviorAttributeTest
    {
        private Mock<IClientMessageFormatter> MockFormatter { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            MockFormatter = new Mock<IClientMessageFormatter>();
        }

        [Test]
        public void FormatterShouldBeSetCorrectlyWhenCallingApplyClientBehavior()
        {
            var operation = GetClientOperation();
            var xpathBehaviorAttribute = new XPathBehaviorAttribute("<html><div>key</div></html>");
            xpathBehaviorAttribute.ApplyClientBehavior(null, operation);
            Assert.IsInstanceOf<XPathFormatter>(operation.Formatter);
            var formatter = (XPathFormatter)operation.Formatter;
            Assert.AreEqual("<html><div>key</div></html>", formatter.XpathExpression);
            Assert.AreSame(MockFormatter.Object, formatter.Formatter);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ApplyDispatchBehaviorShouldThrowNotSupportedException()
        {
            IOperationBehavior attribute = new XPathBehaviorAttribute(null);
            attribute.ApplyDispatchBehavior(null, null);
        }

        private ClientOperation GetClientOperation()
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var clientRuntime = Activator.CreateInstance(typeof(ClientRuntime), flags, null, new object[] { "", "", },
                                                         CultureInfo.InvariantCulture);
            var operation = new ClientOperation((ClientRuntime)clientRuntime, "", "");
            operation.Formatter = MockFormatter.Object;
            return operation;
        }
    }
}
