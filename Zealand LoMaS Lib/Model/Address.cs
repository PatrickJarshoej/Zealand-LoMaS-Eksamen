using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class Address
    {
        public string Region { get; private set; }
        public string City { get; private set; }
        public int PostalCode { get; private set; }
        public string RoadName { get; private set; }    
        public string RoadNumber { get; private set; }

        public Address() { }
        public Address(string region, string city, int postalCode, string roadName, string roadNumber)
        {
            Region = region;
            City = city;
            PostalCode = postalCode;
            RoadName = roadName;
            RoadNumber = roadNumber;
        }
    }
}
