using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Service
{
    public class AClassService
    {
        private IAdminRepo _adminRepo;
        private IInstitutionRepo _institutionRepo;
        private ITeacherRepo _teacherRepo;
        private IAClassRepo _aClassRepo;

        public AClassService(IAdminRepo adminRepo, IInstitutionRepo institutionRepo, ITeacherRepo teacherRepo, IAClassRepo aClassRepo)
        {
            _adminRepo = adminRepo;
            _institutionRepo = institutionRepo;
            _teacherRepo = teacherRepo;
            _aClassRepo = aClassRepo;
        }

        /// <summary>
        /// This method is used in the creation of an AClass object. It calls an ADD method in the IAClassRepo.
        /// </summary>
        /// <param name="teacherID"></param>
        /// <param name="adminID"></param>
        /// <param name="institutionID"></param>
        /// <param name="classStart"></param>
        /// <param name="duration"></param>
        /// <param name="classSubject"></param>
        /// <param name="classDescription"></param>
        public void Create(int teacherID,int adminID,int institutionID,DateTime classStart,TimeSpan duration,string classSubject, string classDescription)
        {
            AClass Lesson = new(teacherID, adminID, institutionID, classStart, duration, classSubject, classDescription);
            _aClassRepo.Create(Lesson);
        }

    }
}
