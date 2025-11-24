using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Repo.Interfaces
{
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
        List<Transport> GetByDate(DateTime date);
    }
}
