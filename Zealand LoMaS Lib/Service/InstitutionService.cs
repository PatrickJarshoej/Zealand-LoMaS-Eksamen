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
        public void Create(List<int> adminIDs, List<int> classIDs, string region, string city, int postalCode, string roadName, string RoadNumber)
        {
            throw new NotImplementedException();
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
            return _institutionRepo.GetByID(id);
        }

        public void Update(Institution institution)
        {
            throw new NotImplementedException();
        }
    }
}
