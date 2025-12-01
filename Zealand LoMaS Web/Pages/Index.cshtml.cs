using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Service;

namespace Zealand_LoMaS_Web.Pages
{
    public class IndexModel : PageModel
    {
        private AdminService _adminService;
        private TeacherService _teacherService;
        private InstitutionService _institutionService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Pass { get; set; }
        [BindProperty]
        public int AdminID { get; set; } = 0;
        [BindProperty]
        public int TeacherID { get; set; } = 0;
        [BindProperty]
        public bool FailedToLogIn { get; set; } = false;
        [BindProperty]
        public bool NeedToRefresh { get; set; } = false;
        [BindProperty]
        public string CookieID { get; set; } = "0";
        [BindProperty]
        public string CookieIsAdmin { get; set; } = "false";
        [BindProperty]
        public string InstituteRegion { get; set; }
        [BindProperty]
        public string InstituteCity { get; set; }
        [BindProperty]
        public int InstitutePostal { get; set; }
        [BindProperty]
        public string InstituteRoadName { get; set; }
        [BindProperty]
        public string InstituteRoadNumber { get; set; }
        [BindProperty]
        public Teacher Teacher { get; set; }
        [BindProperty]
        public Institution Institution{ get; set; }



        public IndexModel(ILogger<IndexModel> logger, AdminService adminService, TeacherService teacherService, InstitutionService institutionService)
        {
            _logger = logger;
            _adminService = adminService;
            _teacherService = teacherService;
            _institutionService = institutionService;
        }

        public void OnGet()
        {
            NeedToRefresh = false;
            if (HttpContext.Request.Cookies["UserID"] != "0" && HttpContext.Request.Cookies["UserStatus"] == "false")
            {
                Teacher = _teacherService.GetByID(Convert.ToInt32(HttpContext.Request.Cookies["UserID"]));
                Institution = _institutionService.GetByID(Teacher.InstitutionID);
            }
        }

        public void OnPost()
        {
            Debug.WriteLine("Du kører den forkerte on post");
        }
        public void OnPostLogIn()
        {
            AdminID = _adminService.LogIn(Email, Pass);
            TeacherID = _teacherService.LogIn(Email, Pass);
            if (AdminID != 0)
            {
                Debug.WriteLine("Admin er logged in");
                CookieID = Convert.ToString(AdminID);
                CookieIsAdmin = "true";
                NeedToRefresh = true;

            }
            else if (TeacherID != 0)
            {
                Debug.WriteLine("Lærer er logged in");
                CookieID = Convert.ToString(TeacherID);
                NeedToRefresh = true;
            }
            else
            {
                Debug.WriteLine("Did not log in");
                FailedToLogIn = true;
            }
        }
        public void OnPostCreateInstitute()
        {
            _institutionService.Create(InstituteRegion, InstituteCity, InstitutePostal, InstituteRoadName, InstituteRoadNumber);
        }

        public IActionResult OnPostEditTeacher()
        {
            return RedirectToPage("/EditTeacher", new { TeacherID = HttpContext.Request.Cookies["UserID"] });

        }
    }
}
