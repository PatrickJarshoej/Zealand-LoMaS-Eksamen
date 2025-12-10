using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Service
{
    public class TransportService
    {
        private ITransportRepo _transportRepo;
        private IInstitutionRelationRepo _relationRepo;
        public TransportService(ITransportRepo transportRepo, IInstitutionRelationRepo relationRepo)
        {
            _transportRepo = transportRepo;
            _relationRepo = relationRepo;
        }
        public void Create(int teacherID, DateTime date, int instituteFromID, int instituteToID)
        {
            List<int> ids= new List<int>();
            ids.Add(instituteToID);
            ids.Add(instituteFromID);
            ids.Sort();
            InstitutionRelation relation = _relationRepo.GetByInstitutionIDs(ids[0], ids[1]);
            Transport aTransport= new Transport(teacherID,date,instituteFromID,instituteToID, relation.Time, relation.Cost);
            _transportRepo.Add(aTransport);
        }
        public void DeleteByID(int transportID)
        {
            throw new NotImplementedException();
        }

        public List<Transport> GetAll()
        {
            return _transportRepo.GetAll();
        }

        public List<Transport> GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Transport GetByID(int transportID)
        {
            return _transportRepo.GetByID(transportID);
        }

        public List<Transport> GetByInstitutionFromID(int institutionID)
        {
            throw new NotImplementedException();
        }

        public List<Transport> GetByInstitutionToID(int institutionID)
        {
            throw new NotImplementedException();
        }

        public List<Transport> GetByTeacherID(int teacherID)
        {
           return _transportRepo.GetByTeacherID(teacherID);
        }

        public void Update(int transportID, DateTime date, double newCost)
        {
            var earlierTransport=GetByID(transportID);
            var updatedTransport = new Transport(
                earlierTransport.TeacherID,
                date,
                earlierTransport.InstitueFromID,
                earlierTransport.InstitueToID,
                newCost,
                transportID
                );
            _transportRepo.Update( updatedTransport );
        }
    }
}
