using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class InstitutionRelation
    {
        public int InstitutionRelationID { get; private set; }
        public TimeSpan Time { get; private set; }

        public double Cost { get; private set; }
        public List<int> InstitutionIDs { get; private set; }


        public InstitutionRelation() { }
        public InstitutionRelation(TimeSpan time, double cost, List<int> institutionIDs)
        {
            InstitutionIDs = institutionIDs;
            Time = time;
            Cost = cost;
        }
       

    }
}
