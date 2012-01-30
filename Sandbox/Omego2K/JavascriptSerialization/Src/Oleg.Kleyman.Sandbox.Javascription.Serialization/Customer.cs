using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oleg.Kleyman.Sandbox.Javascription.Serialization
{
    [DataContract]
    public class Customer
    {
        [DataMember(Name = "Blah")]
        public string Name { get; set; }

        [DataMember(Name = "Age")]
        public int? Age { get; set; }
        public bool AgeSpecified { get { return Age.HasValue; } }

        [DataMember]
        public Testing Bar { get; set; }

        [DataMember]
        public ConsoleColor Color { get; set; }

        [DataMember]
        public List<Testing> Rad { get; set; }
    }

    [DataContract]
    public class Testing
    {
        [DataMember]
        public string Foo { get; set; }
    }
}
