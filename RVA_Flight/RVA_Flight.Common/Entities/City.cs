using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RVA_Flight.Common.Entities
{
    [DataContract]
    public class City
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Country { get; set; }
        public City() { }
    }
}
