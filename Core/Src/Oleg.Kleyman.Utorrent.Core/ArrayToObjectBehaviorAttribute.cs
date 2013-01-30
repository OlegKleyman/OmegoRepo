using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Oleg.Kleyman.Utorrent.Core
{
    public class ArrayToObjectBehaviorAttribute : Attribute, IOperationBehavior
    {
        void IOperationBehavior.Validate(OperationDescription operationDescription)
        {
        }

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            throw new NotSupportedException();
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Formatter = new ArrayToObjectFormatter(operationDescription, clientOperation.Formatter);
        }

        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }
    }
}