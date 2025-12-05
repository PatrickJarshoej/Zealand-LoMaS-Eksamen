using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class Admin
    {
        public int AdministratorID { get; private set; }
        public List<int> InstitutionIDs { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }


        public Admin()
        {

        }
        public Admin(int administratorID)
        {
            AdministratorID = administratorID;
        }
        public Admin(string firstName, string lastName, string email, List<int> institutionIDs)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            InstitutionIDs = institutionIDs;
        }
        public Admin(int adminID,string firstName, string lastName, string email, List<int> institutionIDs)
        {
            AdministratorID = adminID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            InstitutionIDs = institutionIDs;
        }
    }
}
