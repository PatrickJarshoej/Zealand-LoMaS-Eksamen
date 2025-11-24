using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Model
{
    public class Transport
    {
        public int TransportID { get; private set; }
        public int TeacherID { get; private set; }
        public TimeSpan TransportHours { get; private set; }
        public double TransportCost { get; private set; }
        public DateTime TheDate { get; private set; }
        public int InstitueFromID { get; private set; }
        public int InstitueToID { get; private set; }

        public Transport() 
        { 
            TransportID=0; 
            TeacherID=0;
            TransportHours=TimeSpan.Zero;
            TransportCost=0;
            TheDate=DateTime.Now;
            InstitueFromID=0;
            InstitueToID=0;
        }
        public Transport(int teacherID, TimeSpan transportHours, double transportCost, DateTime theDate, int institueFromID, int institueToID, int transportID=0)
        {
            TransportID = transportID;
            TeacherID = teacherID;
            TransportHours = transportHours;
            TransportCost = transportCost;
            TheDate = theDate;
            InstitueFromID = institueFromID;
            InstitueToID = institueToID;
        }
    }
}
