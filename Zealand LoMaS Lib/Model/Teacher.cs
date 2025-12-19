using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class Teacher
    {
        public int TeacherID { get; private set; }
        public int InstitutionID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public TimeSpan WeeklyHours { get; private set; }
        public bool HasCar { get; private set; }
        public Address Address { get; private set; }
        public List<int> AdminIDs { get; private set; }
        public List<int> ClassIDs { get; private set; }
        public List<int> TransportIDs { get; private set; }
        public List<Competency> Competencies { get; private set; }


        public Teacher() { } //Required for the razorpage to load
        public Teacher(int teacherID, int institutionID, string email, string firstName, string lastName, TimeSpan weeklyHours, bool hasCar, Address address, List<int> adminIDs, List<int> classIDs, List<int> transportIDs, List<Competency> competencies)
        {
            TeacherID = teacherID;
            InstitutionID = institutionID;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            WeeklyHours = weeklyHours;
            HasCar = hasCar;
            Address = address;
            AdminIDs = adminIDs;
            ClassIDs = classIDs;
            TransportIDs = transportIDs;
            Competencies = competencies;
        }

        public Teacher(int teacherID, int institutionID, string email, string firstName, string lastName, TimeSpan weeklyHours, bool hasCar, Address address, List<int> adminIDs)
        {
            TeacherID = teacherID;
            InstitutionID = institutionID;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            WeeklyHours = weeklyHours;
            HasCar = hasCar;
            Address = address;
            AdminIDs = adminIDs;
        }
        public override string ToString() //Very helpful to quickly check what values were not set properly
        {
            return $"Name: {FirstName} {LastName}, Email: {Email}, Address: {Address.City}, Institution: {InstitutionID}, Admins: {AdminIDs[0]}";
        }
    }
}
