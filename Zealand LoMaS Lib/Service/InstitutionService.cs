using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Update(Institution institution)
        {
            throw new NotImplementedException();
        }
    }
}
