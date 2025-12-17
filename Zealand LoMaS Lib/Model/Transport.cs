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
        public int InstituteFromID { get; private set; }
        public int InstituteToID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transport"/> class with default values for all properties,this is a default constructor needed for the razorpage
        /// </summary>
        /// <remarks>All numeric and identifier properties are set to zero, <c>TransportHours</c> is set
        /// to <see cref="TimeSpan.Zero"/>, <c>TheDate</c> is set to the current date and time, and <c>TransportCost</c>
        /// is set to zero. Use this constructor when you need a <see cref="Transport"/> object with default
        /// initialization before setting specific property values.</remarks>
        public Transport() 
        { 
            TransportID=0; 
            TeacherID=0;
            TransportHours=TimeSpan.Zero;
            TransportCost=0;
            TheDate=DateTime.Now;
            InstituteFromID=0;
            InstituteToID=0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Transport"/> class with the specified teacher, date, source and
        /// destination institutes, optional transport cost, and optional transport ID.
        /// </summary>
        /// <param name="teacherID">The unique identifier of the teacher associated with the transport.</param>
        /// <param name="theDate">The date on which the transport occurs.</param>
        /// <param name="institueFromID">The unique identifier of the institute from which the transport originates.</param>
        /// <param name="institueToID">The unique identifier of the institute to which the transport is destined.</param>
        /// <param name="transportCost">The cost of the transport. The default is 0.</param>
        /// <param name="transportID">The unique identifier for the transport. The default is 0, which may indicate a new or unsaved transport.</param>
        public Transport(int teacherID, DateTime theDate, int institueFromID, int institueToID, double transportCost=0, int transportID=0)
        {
            TransportID = transportID;
            TeacherID = teacherID;
            TransportHours = TimeSpan.Zero;
            TheDate = theDate;
            InstituteFromID = institueFromID;
            InstituteToID = institueToID;
            TransportCost = transportCost;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Transport"/> class with the specified teacher, date, origin and
        /// destination institutes, transport duration, cost, and optional transport identifier.
        /// </summary>
        /// <param name="teacherID">The unique identifier of the teacher associated with the transport.</param>
        /// <param name="theDate">The date on which the transport occurs.</param>
        /// <param name="institueFromID">The unique identifier of the originating institute.</param>
        /// <param name="institueToID">The unique identifier of the destination institute.</param>
        /// <param name="transportHours">The duration of the transport.</param>
        /// <param name="transportCost">The cost of the transport. The default value is 0.</param>
        /// <param name="transportID">The unique identifier for the transport. The default value is 0, which may indicate a new or unsaved
        /// transport record.</param>
        public Transport(int teacherID, DateTime theDate, int institueFromID, int institueToID, TimeSpan transportHours, double transportCost = 0, int transportID = 0)
        {
            TransportID = transportID;
            TeacherID = teacherID;
            TransportHours = transportHours;
            TheDate = theDate;
            InstituteFromID = institueFromID;
            InstituteToID = institueToID;
            TransportCost = transportCost;
        }
    }
}
