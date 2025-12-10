using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;
using System.Diagnostics;
using Microsoft.Identity.Client;


namespace Zealand_LoMaS_Web.Pages
{
    [BindProperties]
    public class EditTeacherModel : PageModel
    {
        TeacherService _teacherService;
        InstitutionService _institutionService;
        TransportService _transportService;
        public List<Transport> Transports { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherID { get; set; }
        public int InstitutionID { get; set; }
        public int InstitutionToID { get; set; }
        public int InstitutionFromID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public double Hours { get; set; }
        public TimeSpan WeeklyHours { get; set; }
        public bool HasCar { get; set; } = false;

        public int Location { get; set; }

        public string Region { get; private set; }
        public string City { get; private set; }
        public int PostalCode { get; private set; }
        public string RoadName { get; private set; }
        public string RoadNumber { get; private set; }
        //public List<int> AdminIDs { get; set; }
        public string AdminIDs { get; set; }
        public List<Institution> Institutions { get; set; }
        public Institution Institution { get; set; }
        public int TempTransportID { get; set; }



        public EditTeacherModel(TeacherService ts, InstitutionService iS, TransportService transportService)
        {
            _teacherService = ts;
            _institutionService = iS;
            _transportService = transportService;
        }

        public void OnGet(int teacherID)
        {

            TeacherID = teacherID;
            Transports = _transportService.GetByTeacherID(teacherID);
            Teacher = _teacherService.GetByID(teacherID);
            //Teachers = _teacherService.GetAll();
            Institutions = _institutionService.GetAll();
            if (Teacher.AdminIDs != null)
            {
                foreach (var a in Teacher.AdminIDs)
                {
                    AdminIDs += a.ToString() + ", ";
                }
            }
        }
        
        public IActionResult OnPostSave()
        {

            Institutions = _institutionService.GetAll();
            WeeklyHours = TimeSpan.FromHours(Hours);
            Console.WriteLine(Location);
            Institution = _institutionService.GetByID(Location);
            Region = Institution.Location.Region;
            City = Institution.Location.City;
            PostalCode = Institution.Location.PostalCode;
            RoadName = Institution.Location.RoadName;
            RoadNumber = Institution.Location.RoadNumber;

            Debug.WriteLine(FirstName);
            _teacherService.Update(TeacherID, InstitutionID, Email, FirstName, LastName, WeeklyHours, HasCar, Region, City, PostalCode, RoadName, RoadNumber, AdminIDs);
            return RedirectToPage("/Index");
        }
        public void OnPostCreate()
        {
            Debug.WriteLine(InstitutionFromID);
            _transportService.Create(TeacherID, DateTime.Now, InstitutionFromID, InstitutionToID);
            OnGet(TeacherID);
        }
    }

}
