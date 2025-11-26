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


        public TeacherModel(TeacherService ts)
        {
            _teacherService = ts;
            //Teachers = _teacherService.GetAll();
        }

        public void OnGet()
        {
            Teachers = _teacherService.GetAll();
        }
        public IActionResult OnPostCreate()
        {
            WeeklyHours = TimeSpan.FromHours(Hours);
            switch (Location)
            {
                case 1:
                    Region = "Region Sjælland";
                    City = "Roskilde";
                    PostalCode = 4000;
                    RoadName = "Maglegårdsvej";
                    RoadNumber = "2";
                    break;
                case 2:
                    Region = "Region Hovedstaden";
                    City = "Køge";
                    PostalCode = 4600;
                    RoadName = "Lyngbyvej";
                    RoadNumber = "21";
                    break;
                case 3:
                    Region = "Region Sjælland";
                    City = "Næstved";
                    PostalCode = 4700;
                    RoadName = "Femøvej";
                    RoadNumber = "3";
                    break;
                case 4:
                    Region = "Region Sjælland";
                    City = "Slagelse";
                    PostalCode = 4200;
                    RoadName = "Bredahlsgade";
                    RoadNumber = "1A";
                    break;
                case 5:
                    Region = "Region Sjælland";
                    City = "Holbæk";
                    PostalCode = 4300;
                    RoadName = "Anders Larsensvej";
                    RoadNumber = "7";
                    break;
                case 6:
                    Region = "Region Hovedstaden";
                    City = "Nødebo";
                    PostalCode = 3480;
                    RoadName = "Nødebovej";
                    RoadNumber = "77A";
                    break;
                case 7:
                    Region = "Region Hovedstaden";
                    City = "Nykøbing F.";
                    PostalCode = 4800;
                    RoadName = "Bispegade";
                    RoadNumber = "5";
                    break;
                default:
                    Region = "Fiske land";
                    City = "Aquapolis";
                    PostalCode = 420;
                    RoadName = "Fishy Tast";
                    RoadNumber = "69";
                    break;
            }
            Debug.WriteLine("Weekly Hours: " + WeeklyHours.TotalHours);
            Debug.WriteLine("Weekly: " + WeeklyHours.ToString());
            //_teacherService.CreateTeacher(InstitutionID, Email, FirstName, LastName, WeeklyHours, HasCar, Region, City, PostalCode, RoadName, RoadNumber, AdminIDs);
            return RedirectToPage("/Index");
        }
    }

}
