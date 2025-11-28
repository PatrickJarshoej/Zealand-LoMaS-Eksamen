using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Repo.Interfaces
{
    public interface ITeacherRepo
    {
        public void Add(Teacher t, string password);
        public List<Teacher> GetAll();
        public Teacher GetByID(int id);
        public void Update(Teacher t);
        public void DeleteByID(int id);
        public Teacher GetByTransportID(int transportID);
        public List<Teacher> GetByAdminID(int adminID);
        public List<Teacher> GetByInstitutionID(int institutionID);
        public Teacher GetByClassID(int classID);
        public void ChangePassword(int id, string newPass, string oldPass);
        public bool CheckPassword(int id, string pass);
        public string GetPasswordByEmail(string Email);
        public int GetTeacherIDByEmail(string Email);
    }
}
