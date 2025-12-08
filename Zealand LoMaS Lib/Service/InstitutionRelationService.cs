using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Service
{
    public class InstitutionRelationService
    {
        private IInstitutionRelationRepo _institutionRelationRepo;
        public InstitutionRelationService(IInstitutionRelationRepo institutionRelationRepo) 
        { 
        _institutionRelationRepo = institutionRelationRepo;
        }
        public void Create(int id1, int id2, double cost, TimeSpan time)
        {
            List<int> ids= new List<int>();
            ids.Add(id1);
            ids.Add(id2);
            ids.Sort();
            InstitutionRelation institutionRelation = new InstitutionRelation
                (
                time,
                cost,
                ids
                );
            _institutionRelationRepo.Add(institutionRelation);
        }

        public void DeleteByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<InstitutionRelation> GetAll()
        {
            return _institutionRelationRepo.GetAll();
        }

        public Institution GetByAdminID(int id)
        {
            throw new NotImplementedException();
        }

        public InstitutionRelation GetByIDs(int id1, int id2)
        {
            var institutionRelations=GetAll();
            var institutionRelation = new InstitutionRelation();
            foreach (var ir in institutionRelations)
            {
                if (ir.InstitutionIDs.Contains(id1) && ir.InstitutionIDs.Contains(id2) )
                { 
                    institutionRelation=ir;
                }
            }
            return institutionRelation;
        }

        public void Update(Institution institution)
        {
            throw new NotImplementedException();
        }
    }
}
