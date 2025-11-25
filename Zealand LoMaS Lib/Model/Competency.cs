using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class Competency
    {
        public int CompetencyID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateOnly DateTaken { get; private set; }

        public Competency() { }
        public Competency(int competencyID, string name, string description, DateOnly dateTaken)
        {
            CompetencyID = competencyID;
            Name = name;
            Description = description;
            DateTaken = dateTaken;
        }
    }
}
