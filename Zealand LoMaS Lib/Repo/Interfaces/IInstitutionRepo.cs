using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Repo.Interfaces
{
    public interface IInstitutionRepo
    {
        void Add(Institution institution);

        List<Institution> GetAll();

        Institution GetByID(int id);

        Institution GetByAdminID(int id);

        void Update(Institution institution);

        void DeleteByID(int id);
        public void UpdateMapAdminInstitute(int adminID, List<int> newInstituteIDs);
    }
}
