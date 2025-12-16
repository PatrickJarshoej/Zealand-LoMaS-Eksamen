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
        public void Create(int teacherID,int adminID,int institutionID,DateTime classStart,TimeSpan duration,string classSubject, string classDescription)
        {
            AClass Lesson = new(teacherID, adminID, institutionID, classStart, duration, classSubject, classDescription);
            _aClassRepo.Add(Lesson);
        }

    }
}
