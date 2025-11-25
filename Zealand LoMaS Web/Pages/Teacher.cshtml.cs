using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;


namespace Zealand_LoMaS_Web.Pages
{
    [BindProperties]
    public class TeacherModel : PageModel
    {
        TeacherService _teacherService;
        public int TeacherID { get; set; }
        public int InstitutionID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public TimeSpan WeeklyHours { get; set; }
        public bool HasCar { get; set; } = false;


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
        }
        public IActionResult OnPostCreate()
        {
            _teacherService.CreateTeacher(InstitutionID, Email, FirstName, LastName, WeeklyHours, HasCar, Region, City, PostalCode, RoadName, RoadNumber, AdminIDs);
            return RedirectToPage("/Index");
        }
    }

}
