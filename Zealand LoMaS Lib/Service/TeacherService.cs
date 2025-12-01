using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;
using Microsoft.Data.SqlClient;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Service
{
    public class TeacherService
    {

        private ITeacherRepo _teacherRepo;
        public TeacherService(ITeacherRepo tRepo)
        {
            _teacherRepo = tRepo;
        }

        public List<Teacher> GetAll()
        {
            return _teacherRepo.GetAll();
        }
        public Teacher GetByID(int ID)
        {
            return _teacherRepo.GetByID(ID);
        }

        public void CreateTeacher(int institutionID, string email, string firstName, string lastName, TimeSpan weeklyHours, bool hasCar, string region, string city, int postalCode, string roadName, string roadNumber, List<int> adminIDs)
        {
            try
            {
                Address address = new Address(region, city, postalCode, roadName, roadNumber);

                Teacher teacher = new Teacher(0, institutionID, email, firstName, lastName, weeklyHours, hasCar, address, adminIDs);
                string password = Argon2.Hash("NotAdmin");
                _teacherRepo.Add(teacher, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in TeacherService CreateTeacher()");
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public int VerifyLogIn(string email, string password)
        {
            string hashedpassword = Argon2.Hash(password);
            int teacherID = _teacherRepo.GetTeacherIDByEmail(email);
            string StoredPassword = _teacherRepo.GetPasswordByEmail(email);
            if (hashedpassword == StoredPassword && teacherID !=0)
            {
                return teacherID;
            }
            else
            {
                teacherID = 0;
                return teacherID;
            }
        }
        public void Update(Teacher t)
        {
            _teacherRepo.Update(t);
        }
    }
}
