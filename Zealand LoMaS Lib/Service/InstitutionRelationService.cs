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

        /// <summary>
        /// Service for managing relationships between institutions.
        /// </summary>
        /// <remarks>
        /// Initializes the service and ensures all institution relationships exist upon construction.
        /// </remarks>
        /// <param name="institutionRelationRepo">Repository for institution relation data access.</param>
        /// <param name="institutionRepo">Repository for institution data access.</param>
        public InstitutionRelationService(IInstitutionRelationRepo institutionRelationRepo, IInstitutionRepo institutionRepo) 
        { 
            _institutionRelationRepo = institutionRelationRepo;
            _institutionRepo=institutionRepo;
            EnsureRelationsExist();
        }
        /// <summary>
        /// Creates a relationship between two institutions using <see cref="IInstitutionRelationRepo.Add(InstitutionRelation)"/>.
        /// </summary>
        /// <param name="id1">The first institution's unique identifier.</param>
        /// <param name="id2">The second institution's unique identifier.</param>
        /// <param name="time">The travel time between institutions. Defaults to <see cref="TimeSpan.Zero"/>.</param>
        /// <param name="cost">The travel cost between institutions. Defaults to 0.</param>
        /// <remarks>
        /// The institution IDs are sorted to ensure canonical representation, preventing duplicate relationships.
        /// </remarks>
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
        /// <summary>
        /// Ensures that every institution has a relationship with every other institution.
        /// </summary>
        /// <remarks>
        /// Uses the binomial coefficient C(n,2) = n*(n-1)/2 to determine the expected number of relationships.
        /// The method efficiently checks for missing relationships and creates them with default values.
        /// Uses <see cref="IInstitutionRepo.GetAll"/> to retrieve all institutions and 
        /// service method <see cref="GetByID(int)"/> to retrieve all relationships for a specific institution.
        /// service method <see cref="GetAll"/> to retrieve all relationships.
        /// </remarks>
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
        /// <summary>
        /// Retrieves all institution relationships using <see cref="IInstitutionRelationRepo.GetAll"/>.
        /// </summary>
        /// <returns>A list of all <see cref="InstitutionRelation"/> objects.</returns>
        public List<InstitutionRelation> GetAll()
        {
            return _institutionRelationRepo.GetAll();
        }
        /// <summary>
        /// Retrieves the relationship between two specified institutions using <see cref="IInstitutionRelationRepo.GetByInstitutionIDs(int, int)"/>.
        /// </summary>
        /// <param name="id1">The first institution's unique identifier.</param>
        /// <param name="id2">The second institution's unique identifier.</param>
        /// <returns>The <see cref="InstitutionRelation"/> between the two institutions.</returns>
        /// <remarks>
        /// Institution IDs are sorted to ensure canonical representation, matching the ordering 
        /// used in <see cref="Create(int, int, TimeSpan, double)"/> for consistent lookup.
        /// </remarks>
        public InstitutionRelation GetByIDs(int id1, int id2)
        {
            var ids = new List<int> { id1, id2 };
            ids.Sort();
            return _institutionRelationRepo.GetByInstitutionIDs(ids[0], ids[1]);
        }
        /// <summary>
        /// Retrieves all relationships involving the specified institution using <see cref="IInstitutionRelationRepo.GetByInstitutionID(int)"/>.
        /// </summary>
        /// <param name="id">The institution's unique identifier.</param>
        /// <returns>A list of <see cref="InstitutionRelation"/> objects where the specified institution participates.</returns>
        /// <remarks>
        /// Returns relationships regardless of whether the institution is the "from" or "to" institution in the relationship.
        /// </remarks>
        public List<InstitutionRelation> GetByID(int id)
        {
            return _institutionRelationRepo.GetByInstitutionID(id);
        }
        /// <summary>
        /// Updates the relationship between two specified institutions using <see cref="IInstitutionRelationRepo.Update(InstitutionRelation)"/>.
        /// </summary>
        /// <param name="id1">The first institution's unique identifier.</param>
        /// <param name="id2">The second institution's unique identifier.</param>
        /// <param name="cost">The updated travel cost between institutions.</param>
        /// <param name="time">The updated travel time between institutions.</param>
        /// <remarks>
        /// Institution IDs are sorted to ensure canonical representation before delegating to the repository.
        /// This maintains consistency with <see cref="Create(int, int, TimeSpan, double)"/> and <see cref="GetByIDs(int, int)"/>.
        /// </remarks>
        public void Update(int id1, int id2, double cost, TimeSpan time )
        {
            var ids = new List<int>();
            ids.Add(id1);
            ids.Add(id2);
            ids.Sort();
            var institutionRelation = new InstitutionRelation(time, cost, ids);
            _institutionRelationRepo.Update(institutionRelation);
        }
    }
}
