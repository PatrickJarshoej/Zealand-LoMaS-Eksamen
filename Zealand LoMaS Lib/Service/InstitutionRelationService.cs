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
        private IInstitutionRepo _institutionRepo;

        public InstitutionRelationService(IInstitutionRelationRepo institutionRelationRepo, IInstitutionRepo institutionRepo) 
        { 
            _institutionRelationRepo = institutionRelationRepo;
            _institutionRepo=institutionRepo;
            EnsureRelationsExist();

        }
        public void Create(int id1, int id2, TimeSpan time=default(TimeSpan), double cost=0)
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
        private void EnsureRelationsExist()
        {
            //gets all institutions
            var institutions = _institutionRepo.GetAll();
            //gets all relations
            var allRelations = GetAll();
            //counts
            int rAmount = allRelations.Count;
            int iAmount= institutions.Count;
            //this is the expected amount of relations, i derived via the binomial coefficient
            //where C(n,K)=n!/k!*(n-k)!, where n=amount of institutions and k, is 2 because they get paired up
            //this there fore simplifies to n*(n-1)/2
            int expectedRelations = iAmount * (iAmount - 1) / 2;
            if (allRelations.Count != expectedRelations)
            {
                for (int i = 0; i < iAmount; i++)
                { 
                    var institution1=institutions[i];
                    var relations = GetByID(institution1.InstitutionID);
                    //every institute should have exactly one relation with every other institute,
                    //so therefore, if the amount of relations equal the amount of institutes-1 we don't need to run it
                    if (relations.Count != (iAmount - 1))
                    {
                        //we only need to check each pair once, as we only expect one relation for each pair
                        for(int j=i+1; j<iAmount; j++)
                        {
                            var institution2 = institutions[j];
                            if (!relations.Any(r => r.InstitutionIDs.Contains(institution2.InstitutionID)))
                            {
                                Create(institution1.InstitutionID, institution2.InstitutionID, TimeSpan.Zero);
                                rAmount++;
                            }
                            if (rAmount == expectedRelations) { return; }
                        }
                    }
                }
            }
        }
        public List<InstitutionRelation> GetAll()
        {
            return _institutionRelationRepo.GetAll();
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
        public List<InstitutionRelation> GetByID(int id)
        {
            return _institutionRelationRepo.GetByInstitutionID(id);
        }
        public void Update(int id1, int id2, double cost, TimeSpan time )
        {
            var ids = new List<int>();
            ids.Add(id1);
            ids.Add(id2);
            var institutionRelation = new InstitutionRelation(time, cost, ids);
            _institutionRelationRepo.Update(institutionRelation);
        }
    }
}
