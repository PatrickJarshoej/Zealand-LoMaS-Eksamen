using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void CreateTeacher(int institutionID, string email, string firstName, string lastName, TimeSpan weeklyHours, bool hasCar, string region, string city, int postalCode, string roadName, string roadNumber, List<int> adminIDs)
        {
            try
            {
                Address address = new Address(region, city, postalCode, roadName, roadNumber);

                Teacher teacher = new Teacher(0, institutionID, email, firstName, lastName, weeklyHours, hasCar, address, adminIDs);

                _teacherRepo.Add(teacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in TeacherService CreateTeacher()");
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public int LogIn(string Email, string Password)
        {
            int TeacherLoggedIn = _teacherRepo.GetLogIn(Email, Password);
            return TeacherLoggedIn;
        }
    }
}
