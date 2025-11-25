using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;


namespace Zealand_LoMaS_Web.Pages
{
    public class TeacherModel : PageModel
    {
        TeacherService _teacherService;
        [BindProperty]
        public int TeacherID { get; set; }
        [BindProperty]
        public List<Teacher> Teachers { get; set; }

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

            _teacherService.CreateTeacher(
                institutionID: 1,
                firstName: "Test",
                lastName: "Teacher",
                weeklyHours: new TimeSpan(37, 0, 0),
                hasCar: true,
                region: "Region Zealand",
                city: "Køge",
                postalCode: 4600,
                roadName: "Testvej",
                roadNumber: "1",
                adminIDs: new List<int> { 1, 2 }
                );
            return RedirectToPage("/Index");
        }
    }

}
