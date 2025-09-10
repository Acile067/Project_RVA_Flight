using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RVA_Flight.Common.Entities
{
    [DataContract]
    public class Airplane
    {
        private string code;
        private string name;
        private int capacity;
        private int yearOfManufacture;

        [DataMember]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
        [DataMember]
        public int YearOfManufacture
        {
            get { return yearOfManufacture; }
            set { yearOfManufacture = value; }
        }
    }
}
