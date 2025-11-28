using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;
using System.Diagnostics;


namespace Zealand_LoMaS_Web.Pages
{
    [BindProperties]
    public class TeacherModel : PageModel
    {
        TeacherService _teacherService;
        InstitutionService _institutionService;
        public List<Teacher> Teachers { get; set; }
        public int TeacherID { get; set; }
        public int InstitutionID { get; set; }
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
        public List<int> AdminIDs { get; set; }
        public List<Institution> Institutions { get; set; }


        public TeacherModel(TeacherService ts, InstitutionService iS)
        {
            _teacherService = ts;
            _institutionService = iS;
        }

        public void OnGet()
        {
            Teachers = _teacherService.GetAll();
            Institutions = _institutionService.GetAll();
        }
        public IActionResult OnPostCreate()
        {
            //We need to pull up the full list of institutions again because it forgets our existing list the moment a button is pressed
            Institutions = _institutionService.GetAll();
            WeeklyHours = TimeSpan.FromHours(Hours);
            Console.WriteLine(Location);
            Region = Institutions[Location].Location.Region;
            City = Institutions[Location].Location.City;
            PostalCode = Institutions[Location].Location.PostalCode;
            RoadName = Institutions[Location].Location.RoadName;
            RoadNumber = Institutions[Location].Location.RoadNumber;

            
            //Debug.WriteLine("Weekly Hours: " + WeeklyHours.TotalHours);
            //Debug.WriteLine("Weekly: " + WeeklyHours.ToString());
            //Debug.WriteLine($"Region: {Region} City: {City} PostalCode: {PostalCode} RoadName: {RoadName} RoadNumber: {RoadNumber}");
            _teacherService.CreateTeacher(InstitutionID, Email, FirstName, LastName, WeeklyHours, HasCar, Region, City, PostalCode, RoadName, RoadNumber, AdminIDs);
            return RedirectToPage("/Index");
        }
        public IActionResult OnPostEdit()
        {



            return RedirectToPage("/EditTeacher", new { TeacherID = TeacherID });
        }
    }

}
