using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Repo.Interfaces
{
    /// <summary>
    /// Defines methods for managing and retrieving relationships between institutions.
    /// </summary>
    /// <remarks>Implementations of this interface provide functionality to add, update, and query institution
    /// relationships.</remarks>
    public interface IInstitutionRelationRepo
    {
        void Add(InstitutionRelation institutionRelation);

        List<InstitutionRelation> GetAll();


        InstitutionRelation GetByInstitutionIDs(int id1, int id2);
        List<InstitutionRelation> GetByInstitutionID(int id);
        void Update(InstitutionRelation institutionRelation);

    }
}
