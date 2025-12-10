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
            //Debug.WriteLine("ID: " + id);
            _institutionRepo.DeleteByID(id);
        }

        public List<Institution> GetAll()
        {
            return _institutionRepo.GetAll();
        }

        public Institution GetByAdminID(int id)
        {
            throw new NotImplementedException();
        }

        public Institution GetByID(int id)
        {
            Debug.WriteLine("ID: " + id);
            //return null;
            return _institutionRepo.GetByID(id);
        }

        public void Update(int institutionID, string region, int postalCode, string city, string roadName, string roadNumber, string admins, List<int> classIDs)
        {
            List<int> adminIDs = new();
            if (admins != "")
            {
                List<string> aID = admins.Split(',').ToList<string>();
                foreach (var i in aID)
                {
                    if (i != " ")
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
