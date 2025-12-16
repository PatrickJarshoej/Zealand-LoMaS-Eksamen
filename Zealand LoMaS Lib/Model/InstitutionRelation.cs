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

        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionRelation"/> class, this is a default constructor needed for the razorpage.
        /// </summary>
        public InstitutionRelation() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionRelation"/> class with the specified time, cost, and
        /// institution identifiers.
        /// </summary>
        /// <param name="time">The duration associated with the relation.</param>
        /// <param name="cost">The cost value associated with the relation.</param>
        /// <param name="institutionIDs">A list of institution identifiers involved in the relation. Cannot be null.</param>
        public InstitutionRelation(TimeSpan time, double cost, List<int> institutionIDs)
        {
            InstitutionIDs = institutionIDs;
            Time = time;
            Cost = cost;
        }
       

    }
}
