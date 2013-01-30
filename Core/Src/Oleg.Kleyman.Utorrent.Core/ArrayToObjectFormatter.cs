using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oleg.Kleyman.Utorrent.Core
{
    public class ArrayToObjectFormatter : IClientMessageFormatter
    {
        public OperationDescription Operation { get; set; }
        public IClientMessageFormatter Formatter { get; set; }

        public ArrayToObjectFormatter(OperationDescription operation, IClientMessageFormatter formatter)
        {
            Operation = operation;
            Formatter = formatter;
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            return Formatter.SerializeRequest(messageVersion, parameters);
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            var deserializedMessage = message.GetBody<XElement>();
            var finalElement = new XElement("root");
            foreach (var node in deserializedMessage.DescendantNodes())
            {
                var elem = node as XElement;
                if (elem != null)
                {
                    var typeAttribute = elem.Attribute("type");
                    if (typeAttribute != null && typeAttribute.Value == "array")
                    {
                        elem.SetAttributeValue("type", null);
                    }
                }
            }

            var serializer = new DataContractSerializer(Operation.Messages[1].Body.ReturnValue.Type);

            var result = serializer.ReadObject(deserializedMessage.CreateReader(), false);
            
            return result;
        }
    }
}