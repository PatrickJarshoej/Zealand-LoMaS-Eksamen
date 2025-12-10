using System;
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

        public Admin Admin { get; set; }
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
                Admin = _adminService.GetByID(Convert.ToInt32(HttpContext.Request.Cookies["UserID"]));
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
                //Debug.WriteLine("Admin er logged in");
                CookieID = Convert.ToString(AdminID);
                CookieIsAdmin = "true";
                OnGet();
                NeedToRefresh = true;

            }
            else if (TeacherID != 0)
            {
                //Debug.WriteLine("Lærer er logged in");
                CookieID = Convert.ToString(TeacherID);
                OnGet();
                NeedToRefresh = true;
            }
            else
            {
                //Debug.WriteLine("Did not log in");
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
        {   //This is the method used when a teacher edits themselves
            return RedirectToPage("/EditTeacher", new { TeacherID = HttpContext.Request.Cookies["UserID"] });
        }
        public IActionResult OnPostEditTeacher()
        {
            return RedirectToPage("/EditTeacher", new { teacherID = TeacherID });
        }
        public void OnPostDeleteTeacher()
        {
            _teacherService.DeleteByID(TeacherID);
            OnGet();
        }
        public void OnPostDeleteInstitution()
        {
            _institutionService.DeleteByID(InstitutionID);
            OnGet();
        }
        public IActionResult OnPostEditInstitution()
        {
            return RedirectToPage("/EditInstitution", new { institutionID = InstitutionID});
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
        public IActionResult OnPostEditAdminProfile()
        {
            return RedirectToPage("/EditAdministrator", new { adminID = AdminID });
        }
        //public void OnPostEditAdministrator()
        //{
        //    _adminService.Update(AdminID, Email, FirstName, LastName, InstitutionIDs);
        //    AdminID = 0;
        //    Email = "";
        //    FirstName = "";
        //    LastName = "";
        //    InstitutionIDs = new List<int>();
        //}
        public IActionResult OnPostLogOut()
        {   
            HttpContext.Response.Cookies.Append("UserID", "0"); //If you logout we set your ID to zero
            HttpContext.Response.Cookies.Append("UserStatus", "false");
            return RedirectToPage();
        }
        public void OnPostChangePassword()
        {
            OnGet(); //We need to run this or it will forget who the teacher is...
            //To create a new password they need to write it twice as a safety measure
            if (Pass == Pass2)
            { //If the wrote the same password in both fields we update the password in the DB
                _teacherService.ChangePass(TeacherID, Pass);
                ChangePasswordModalShow = true; //When you use an OnPost method it refreshes, but i wan't the modal to stay on screen
            }
            else
            {   //If both fields are not the same
                FailedToChangePass = true; //Used to make the red error box
                ChangePasswordModalShow = true;
            }
        }
        public void OnPostChangeAdminPassword()
        {
            OnGet();
            bool WasItSuccessfuld = _adminService.ChangePassword(Admin.AdministratorID, Pass, Pass2);
            if (WasItSuccessfuld == true)
            {
                FailedToChangePass = false;
                ChangePasswordModalShow = true;
            }
            else
            {
                FailedToChangePass = true;
                ChangePasswordModalShow = true;
            }
        }
    }
}
