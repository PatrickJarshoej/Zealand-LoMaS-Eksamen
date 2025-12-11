using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class AClass
    {
        public int ClassID { get; private set; }
        public int TeacherID { get; private set; }
        public int AdministratorID { get; private set; }
        public int InstitutionID { get; private set; }
        public DateTime ClassStart { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string ClassSubject { get; private set; }
        public string ClassDesciption { get; private set; }

        public AClass()
        {

        }
        public AClass(int teacherID, int administratorID, int institutionID, DateTime classStart, TimeSpan duration, string classSubject, string classDesciption)
        {
            TeacherID = teacherID;
            AdministratorID = administratorID;
            InstitutionID = institutionID;
            ClassStart = classStart;
            Duration = duration;
            ClassSubject = classSubject;
            ClassDesciption = classDesciption;
        }
        public AClass(int classID, int teacherID, int administratorID, int institutionID, DateTime classStart, TimeSpan duration, string classSubject, string classDesciption )
        {
            ClassID = classID;
            TeacherID = teacherID;
            AdministratorID = administratorID;
            InstitutionID = institutionID;
            ClassStart = classStart;
            Duration = duration;
            ClassSubject = classSubject;
            ClassDesciption = classDesciption;
        }
    }
}
