using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///     Represents an XPath behavior.
    /// </summary>
    public sealed class XPathBehaviorAttribute : Attribute, IOperationBehavior
    {
        /// <summary>
        ///     Initializes an <see cref="XPathBehaviorAttribute" /> object.
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

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription,
                                                      DispatchOperation dispatchOperation)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param><param name="clientOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Formatter = new XPathFormatter(clientOperation.Formatter, XpathExpression);
        }

        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription,
                                                     BindingParameterCollection bindingParameters)
        {
        }

        #endregion

        /// <summary>
        ///     Gets the XPath expression used by this object.
        /// </summary>
        public string XpathExpression { get; private set; }
    }
}