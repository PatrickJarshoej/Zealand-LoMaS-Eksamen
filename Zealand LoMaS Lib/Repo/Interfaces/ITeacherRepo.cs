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
        public Teacher GetByClassID(int classID);
        public string GetPasswordByEmail(string Email);
        public int GetTeacherIDByEmail(string Email);
        public string GetPasswordByteacherID(int teacherID);
        public void UpdatePassword(int teacherID, string Password);
    }
}
