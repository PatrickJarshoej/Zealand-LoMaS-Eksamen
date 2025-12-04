using System.Diagnostics;
using System.Reflection;
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
        private TransportService _transportService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string lastName { get; set; }
        [BindProperty]
        public string Pass { get; set; }
        [BindProperty]
        public string Pass2 { get; set; }
        [BindProperty]
        public int AdminID { get; set; } = 0;
        [BindProperty]
        public int TeacherID { get; set; } = 0;
        [BindProperty]
        public bool FailedToLogIn { get; set; } = false;
        [BindProperty]
        public bool NeedToRefresh { get; set; } = false;
        [BindProperty]
        public bool FailedToChangePass { get; set; } = false;
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
        public int InstitutionID { get; set; }
        [BindProperty]
        public Teacher Teacher { get; set; }
        [BindProperty]
        public Institution Institution { get; set; }
        [BindProperty]
        public List<Institution> Institutions { get; set; }
        [BindProperty]
        public List<Transport> Transports { get; set; }
        [BindProperty]
        public bool ChangePasswordModalShow { get; set; } = false;



        public IndexModel(ILogger<IndexModel> logger, AdminService adminService, TeacherService teacherService, InstitutionService institutionService, TransportService tS)
        {
            _logger = logger;
            _adminService = adminService;
            _teacherService = teacherService;
            _institutionService = institutionService;
            _transportService = tS;
        }

        public void OnGet()
        {
            NeedToRefresh = false;
            if (HttpContext.Request.Cookies["UserID"] != "0" && HttpContext.Request.Cookies["UserStatus"] == "false")
            {
                Teacher = _teacherService.GetByID(Convert.ToInt32(HttpContext.Request.Cookies["UserID"]));
                Institution = _institutionService.GetByID(Teacher.InstitutionID);
                Institutions = _institutionService.GetAll();
                Transports = _transportService.GetByTeacherID(Teacher.TeacherID);
                TeacherID = Teacher.TeacherID;
            }
        }

        public void OnPost()
        {
            Debug.WriteLine("Du kører den forkerte on post");
        }
        public void OnPostLogIn()
        {
            AdminID = _adminService.VerifyLogIn(Email, Pass);
            TeacherID = _teacherService.VerifyLogIn(Email, Pass);
            if (AdminID != 0)
            {
                Debug.WriteLine("Admin er logged in");
                CookieID = Convert.ToString(AdminID);
                CookieIsAdmin = "true";
                OnGet();
                NeedToRefresh = true;

            }
            else if (TeacherID != 0)
            {
                Debug.WriteLine("Lærer er logged in");
                CookieID = Convert.ToString(TeacherID);
                OnGet();
                NeedToRefresh = true;
                //RedirectToPage();
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
        public void OnPostCreateAdmin()
        {
            _adminService.Create(Email, FirstName, lastName, InstitutionID);
        }
        public IActionResult OnPostEditTeacherProfile()
        {
            return RedirectToPage("/EditTeacher", new { TeacherID = HttpContext.Request.Cookies["UserID"] });
        }
        public IActionResult OnPostEditTeacherCompetencies()
        {
            return RedirectToPage("/EditTeacher", new { TeacherID = HttpContext.Request.Cookies["UserID"] });
        }
        public IActionResult OnPostEditTeacherClasses()
        {
            return RedirectToPage("/EditTeacher", new { TeacherID = HttpContext.Request.Cookies["UserID"] });
        }
        public void OnPostHashAdminPassword()
        {
            _adminService.HashThePassword(AdminID);
        }
        public void OnPostHashTeacherPassword()
        {
            _teacherService.HashThePassword(TeacherID);
        }
        public IActionResult OnPostLogOut()
        {
            
            HttpContext.Response.Cookies.Append("UserID", "0");
            HttpContext.Response.Cookies.Append("UserStatus", "false");
            //NeedToRefresh = true;
            return RedirectToPage();
        }
        public void OnPostChangePassword()
        {
            Debug.WriteLine("Running ChangePassword");
            OnGet(); //We need to run this or it will forget who the teacher is...
            //To create a new password they need to write it twice as a safety measure
            if (Pass == Pass2)
            {
                _teacherService.ChangePass(TeacherID, Pass);
                Debug.WriteLine("Passwords match");
                ChangePasswordModalShow = true;
            }
            else
            {
                Debug.WriteLine("Passwords do not match");
                FailedToChangePass = true;
                ChangePasswordModalShow = true;
            }
        }
    }
}
