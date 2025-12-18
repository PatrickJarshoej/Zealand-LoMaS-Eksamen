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
                string password = Argon2.Hash("NotAdmin"); //We set a default Password for teachers
                _teacherRepo.Add(teacher, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in TeacherService CreateTeacher()");
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// This method uses af password string and email string to verify if they match the stored password for a given User in the database.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
            { //On the web page admins is a string so here we have to split it
                List<string> aID = admins.Split(',').ToList<string>(); //Split is a built in method that turns a string into an array of strings
                foreach (var i in aID)
                {
                    if (i != " ") //We need to check the string isn't just a space or the Convert.ToInt32(); will crash
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

        /// <summary>
        /// This method was used to hash passwords from unhashed passwords in the database. it is no longer in use but kept in case it is ever needed again to update to another enryption method.
        /// </summary>
        /// <param name="teacherID"></param>
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

        /// <summary>
        /// this method is used to change the current password in the database for a user.
        /// It requires the teacherID of the current user and 2 password string that has to be identical.
        /// If the passwords differ the password will not be changed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass"></param>
        public void ChangePass(int id, string pass)
        {
            string hashPass = Argon2.Hash(pass);
            _teacherRepo.UpdatePassword(id, hashPass);
        }

        /// <summary>
        /// This method uses an teacherID to delete an admin from the database that has that ID.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteByID(int id)
        {
            _teacherRepo.DeleteByID(id);
        }
    }
}
