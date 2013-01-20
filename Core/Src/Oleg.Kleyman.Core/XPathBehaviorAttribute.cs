using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents an XPath behavior.
    /// </summary>
    public sealed class XPathBehaviorAttribute : Attribute, IOperationBehavior
    {
        /// <summary>
        /// Gets the XPath expression used by this object.
        /// </summary>
        public string XpathExpression { get; private set; }

        /// <summary>
        /// Initializes an <see cref="XPathBehaviorAttribute"/> object.
        /// </summary>
        /// <param name="xpathExpression">The XPath expression to use.</param>
        public XPathBehaviorAttribute(string xpathExpression)
        {
            XpathExpression = xpathExpression;
        }

        #region Implementation of IOperationBehavior

        void IOperationBehavior.Validate(OperationDescription operationDescription)
        {
        }

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            throw new NotSupportedException();
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Formatter = new XPathFormatter(clientOperation.Formatter, XpathExpression);
        }

        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        #endregion
    }
}