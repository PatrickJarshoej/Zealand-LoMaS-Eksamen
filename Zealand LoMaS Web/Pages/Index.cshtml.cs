using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Service;

namespace Zealand_LoMaS_Web.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private AdminService _adminService;
        private TeacherService _teacherService;
        private InstitutionService _institutionService;
        private TransportService _transportService;
        private readonly ILogger<IndexModel> _logger;

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int InstitutionID { get; set; }
        public double Hours { get; set; }
        public TimeSpan WeeklyHours { get; set; }
        public bool HasCar { get; set; } = false;
        public int Location { get; set; }
        public List<int> AdminIDs { get; set; }
        public string Pass { get; set; }
        public string Pass2 { get; set; }
        public int AdminID { get; set; } = 0;
        public int TeacherID { get; set; } = 0;
        public bool FailedToLogIn { get; set; } = false;
        public bool NeedToRefresh { get; set; } = false;
        public bool FailedToChangePass { get; set; } = false;
        public string CookieID { get; set; } = "0";
        public string CookieIsAdmin { get; set; } = "false";
        public string InstituteRegion { get; set; }
        public string InstituteCity { get; set; }
        public int InstitutePostal { get; set; }
        public string InstituteRoadName { get; set; }
        public string InstituteRoadNumber { get; set; }
        public List<int> InstitutionIDs { get; set; }
        public Teacher Teacher { get; set; }
        public Institution Institution { get; set; }
        public List<Institution> Institutions { get; set; }
        public List<Transport> Transports { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Admin> Admins { get; set; }
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
            if (HttpContext.Request.Cookies["UserID"] != "0" && HttpContext.Request.Cookies["UserStatus"] == "true")
            {
                Institutions = _institutionService.GetAll();
                Teachers = _teacherService.GetAll();
                Admins = _adminService.GetAll();
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
            _adminService.Create(Email, FirstName, LastName, InstitutionIDs);
        }
        public void OnPostCreateTeacher()
        {
            Institution i = _institutionService.GetByID(InstitutionID);

            string Region = i.Location.Region;
            string City = i.Location.City;
            int PostalCode = i.Location.PostalCode;
            string RoadName = i.Location.RoadName;
            string RoadNumber = i.Location.RoadNumber;
            _teacherService.CreateTeacher(InstitutionID, Email, FirstName, LastName, WeeklyHours, HasCar, Region, City, PostalCode, RoadName, RoadNumber, AdminIDs);
            OnGet();
            Email = "";
            FirstName = "";
            LastName = "";
            InstitutionID = 0;

        }
        public IActionResult OnPostEditTeacherProfile()
        {
            return RedirectToPage("/EditTeacher", new { TeacherID = HttpContext.Request.Cookies["UserID"] });
        }
        public IActionResult OnPostEditTeacher()
        {
            return RedirectToPage("/EditTeacher", new { teacherID = TeacherID });
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
        public void OnPostEditAdministrator()
        {
            _adminService.Update(AdminID, Email, FirstName, LastName, InstitutionIDs);
            AdminID = 0;
            Email = "";
            FirstName = "";
            LastName = "";
            InstitutionIDs = new List<int>();
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
