using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Repo.Interfaces
{
    /// <summary>
    /// Defines methods for managing and retrieving <see cref="Transport"/> entities in a data store.
    /// </summary>
    /// <remarks>The <c>ITransportRepo</c> interface provides a contract for adding, updating, deleting, and
    /// querying <see cref="Transport"/> records. Implementations are responsible for handling data persistence and
    /// retrieval logic.</remarks>
    public interface ITransportRepo
    {
        
        void Add(Transport transport);
        List<Transport> GetAll();
        Transport GetByID(int transportID);
        void Update(Transport transport);
        void DeleteByID(int transportID);
        List<Transport> GetByTeacherID(int teacherID);
        List<Transport> GetByInstitutionFromID(int institutionID);
        List<Transport> GetByInstitutionToID(int institutionID);
    }
}
