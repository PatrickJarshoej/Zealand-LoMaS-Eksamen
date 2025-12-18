using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <summary>
        /// Service for managing transport operations and business logic.
        /// </summary>
        /// <remarks>
        /// Coordinates transport data with institution relationship information for cost and time calculations.
        /// </remarks>
        /// <param name="transportRepo">Repository for transport data access.</param>
        /// <param name="relationRepo">Repository for institution relationship data access.</param>
        public TransportService(ITransportRepo transportRepo, IInstitutionRelationRepo relationRepo)
        {
            _transportRepo = transportRepo;
            _relationRepo = relationRepo;
        }
        /// <summary>
        /// Creates a new transport record using <see cref="ITransportRepo.Add(Transport)"/>.
        /// </summary>
        /// <param name="teacherID">The unique identifier of the teacher making the transport.</param>
        /// <param name="date">The date of the transport.</param>
        /// <param name="instituteFromID">The unique identifier of the institution where the transport originates.</param>
        /// <param name="instituteToID">The unique identifier of the institution where the transport is destined.</param>
        /// <remarks>
        /// Looks up the pre-existing institution relationship to determine travel time and cost.
        /// Institution relationships are guaranteed to exist for all pairs (ensured by <see cref="InstitutionRelationService.EnsureRelationsExist"/>).
        /// Institution IDs are sorted for canonical lookup while preserving transport direction.
        /// </remarks>
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
        /// <summary>
        /// Deletes a transport record using <see cref="ITransportRepo.DeleteByID(int)"/>.
        /// </summary>
        /// <param name="transportID">The unique identifier of the transport to delete.</param>
        public void DeleteByID(int transportID)
        {
            _transportRepo.DeleteByID(transportID);
        }
        /// <summary>
        /// Retrieves all transport records using <see cref="ITransportRepo.GetAll"/>.
        /// </summary>
        /// <returns>A list of all <see cref="Transport"/> objects.</returns>
        public List<Transport> GetAll()
        {
            return _transportRepo.GetAll();
        }
        /// <summary>
        /// Retrieves a transport record by its unique identifier using <see cref="ITransportRepo.GetByID(int)"/>.
        /// </summary>
        /// <param name="transportID">The unique identifier of the transport to retrieve.</param>
        /// <returns>The <see cref="Transport"/> object with the specified ID.</returns>
        public Transport GetByID(int transportID)
        {
            return _transportRepo.GetByID(transportID);
        }
        /// <summary>
        /// Retrieves all transports originating from the specified institution using <see cref="ITransportRepo.GetByInstitutionFromID(int)"/>.
        /// </summary>
        /// <param name="institutionID">The unique identifier of the institution where transports originate.</param>
        /// <returns>A list of <see cref="Transport"/> objects departing from the specified institution.</returns>
        public List<Transport> GetByInstitutionFromID(int institutionID)
        {
            return _transportRepo.GetByInstitutionFromID(institutionID);
        }
        /// <summary>
        /// Retrieves all transports destined for the specified institution using <see cref="ITransportRepo.GetByInstitutionToID(int)"/>.
        /// </summary>
        /// <param name="institutionID">The unique identifier of the institution where transports are destined.</param>
        /// <returns>A list of <see cref="Transport"/> objects arriving at the specified institution.</returns>
        public List<Transport> GetByInstitutionToID(int institutionID)
        {
            return _transportRepo.GetByInstitutionToID(institutionID);
        }
        /// <summary>
        /// Retrieves all transports for the specified teacher using <see cref="ITransportRepo.GetByTeacherID(int)"/>.
        /// </summary>
        /// <param name="teacherID">The unique identifier of the teacher.</param>
        /// <returns>A list of <see cref="Transport"/> objects associated with the specified teacher.</returns>
        public List<Transport> GetByTeacherID(int teacherID)
        {
           return _transportRepo.GetByTeacherID(teacherID);
        }
        /// <summary>
        /// Updates a transport record using <see cref="ITransportRepo.Update(Transport)"/>.
        /// </summary>
        /// <param name="transportID">The unique identifier of the transport to update.</param>
        /// <param name="date">The updated date of the transport.</param>
        /// <param name="newCost">The updated travel cost.</param>
        /// <param name="time">The updated travel time.</param>
        /// <remarks>
        /// Retrieves the existing transport to preserve teacher and institution references,
        /// then creates an updated transport object with the new values.
        /// </remarks>
        public void Update(int transportID, DateTime date, double newCost, TimeSpan time)
        {
            var earlierTransport=GetByID(transportID);
            var updatedTransport = new Transport(
                earlierTransport.TeacherID,
                date,
                earlierTransport.InstituteFromID,
                earlierTransport.InstituteToID,
                time,
                newCost,
                transportID
                );
            _transportRepo.Update( updatedTransport );
        }
    }
}
