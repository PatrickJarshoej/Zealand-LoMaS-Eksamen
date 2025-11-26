using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class Institution
    {
        public int InstitutionID { get; private set; }
        public Address Location { get; private set; }
        public List<int> AdminIDs { get; private set; }
        public List<int> ClassIDs { get; private set; }


        public Institution() { } 
        public Institution(int institutionID, Address location, List<int> adminIDs, List<int> classIDs)
        {
            InstitutionID = institutionID;
            Location = location;
            AdminIDs = adminIDs;
            ClassIDs = classIDs;
        }
    }
}
