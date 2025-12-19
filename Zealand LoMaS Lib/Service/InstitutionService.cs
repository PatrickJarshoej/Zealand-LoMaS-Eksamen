using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Service
{
    public class InstitutionService
    {
        private IInstitutionRepo _institutionRepo;
        public InstitutionService(IInstitutionRepo institutionRepo) 
        { 
        _institutionRepo = institutionRepo;
        }
        public void Create(string region, string city, int postal, string roadName, string roadNumber)
        {
            Address address = new Address(region, city, postal, roadName, roadNumber);
            Institution institute = new Institution(address);
            _institutionRepo.Add(institute);
        }

        public void DeleteByID(int id)
        {
            _institutionRepo.DeleteByID(id);
        }

        public List<Institution> GetAll()
        {
            return _institutionRepo.GetAll();
        }


        public Institution GetByID(int id)
        {
            return _institutionRepo.GetByID(id);
        }

        public void Update(int institutionID, string region, int postalCode, string city, string roadName, string roadNumber, string admins, List<int> classIDs)
        {
            List<int> adminIDs = new();
            if (admins != "")
            { //On the web page admins is a string so here we have to split it
                List<string> aID = admins.Split(',').ToList<string>();  //Split is a built in method that turns a string into an array of strings
                foreach (var i in aID)
                {
                    if (i != " ") //We need to check the string isn't just a space or the Convert.ToInt32(); will crash as " " is not a valid int
                    {
                        adminIDs.Add(Convert.ToInt32(i));
                    }
                }
            }
            else
            {
                Debug.WriteLine("No admins found");
            }
            Address location = new(region, city, postalCode, roadName, roadNumber);

            Institution institution = new(institutionID, location, adminIDs, classIDs);

            _institutionRepo.Update(institution);
        }
    }
}
