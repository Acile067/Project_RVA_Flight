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
    public class Airplane
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Capacity { get; set; }
        [DataMember]
        public int YearOfManufacture { get; set; }
        public Airplane() { }
    }
}
