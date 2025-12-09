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
                //Debug.WriteLine(teacher.ToString());
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
            int teacherID = _teacherRepo.GetTeacherIDByEmail(email);
            string StoredPassword = _teacherRepo.GetPasswordByEmail(email);
            if (Argon2.Verify( StoredPassword, password))
            {
                return teacherID;
            }
            else
            {
                teacherID = 0;
                return teacherID;
            }
        }
        public void Update(int teacherID, int institutionID, string email, string firstName, string lastName, TimeSpan weeklyHours, bool hasCar, string region, string city, int postalCode, string roadName, string roadNumber, string admins)
        {
            Address address = new(region, city, postalCode, roadName, roadNumber);
            List<int> transportIDs = new();
            List<int> classIDs = new();
            List<Competency> competencies = new();
            List<int> adminIDs = new();
            if (admins != "")
            {
                List<string> aID = admins.Split(',').ToList<string>();
                foreach (var i in aID)
                {
                    if (i != " ")
                    {
                        adminIDs.Add(Convert.ToInt32(i));
                    }
                }
            }
            else
            {
                Debug.WriteLine("No admins found");
            }


            Teacher t = new(teacherID, institutionID, email, firstName, lastName, weeklyHours, hasCar, address, adminIDs, transportIDs, classIDs, competencies);
            _teacherRepo.Update(t);
        }
        public void HashThePassword(int teacherID)
        {
            string pass = _teacherRepo.GetPasswordByteacherID(teacherID);
            if (pass != "0")
            {
                pass = Argon2.Hash(pass);
                _teacherRepo.UpdatePassword(teacherID, pass);
            }
            else
            {
                Debug.WriteLine("Failed to find password");
            }
        }

        public void ChangePass(int id, string pass)
        {
            string hashPass = Argon2.Hash(pass);
            //Debug.WriteLine("ChangePass In Service");
            _teacherRepo.UpdatePassword(id, hashPass);
        }
    }
}
