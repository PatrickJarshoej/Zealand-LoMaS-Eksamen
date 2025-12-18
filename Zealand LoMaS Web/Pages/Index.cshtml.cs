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
        private AClassService _aClassService;
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
        public DateTime ClassStart { get; set; }
        public DateTime ClassDuration { get; set; }
        public string ClassSubject { get; set; }
        public string ClassDescription { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AdminService adminService, TeacherService teacherService, InstitutionService institutionService, TransportService tS, AClassService acs)
        {
            _logger = logger;
            _adminService = adminService;
            _teacherService = teacherService;
            _institutionService = institutionService;
            _transportService = tS;
            _aClassService = acs;
        }

        /// <summary>
        /// This method is used whenever the website is loaded and is the first method always used.
        /// The method checks whether the Cookies showing if you are a teacher or an admin are activated, and which of the either teachers or administrator you are.
        /// If both are null or "0" and "false" then the log in screen will be shown.
        /// </summary>
        public void OnGet()
        {
            NeedToRefresh = false;
            if (HttpContext.Request.Cookies["UserID"] != "0" && HttpContext.Request.Cookies["UserStatus"] == "false")
            {//User status false means it's a teacher and id != 0 means they are a valid teacher
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

        /// <summary>
        /// this method is called if you fail to call the correct on post you need.
        /// It then sends a debug message giving the developer information about the wrong usage of the OnPost method.
        /// </summary>
        public void OnPost()
        {   //We never actually want to run this OnPost
            Debug.WriteLine("Du kører den forkerte on post");
        }

        /// <summary>
        /// This method is used when an attempt at to log in is used.
        /// It runs the verify methods in both teacher and admin repository and if a value is returned that changes the ID values to something other than 0 it executes methods to create cookies for consistant log-in.
        /// </summary>
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

        /// <summary>
        /// This method uses properties set in the CSHTML code to send a create admin object request in the database.
        /// </summary>
        public void OnPostCreateAdmin()
        {
            InstitutionIDs.Add(InstitutionID);
            _adminService.Create(Email, FirstName, LastName, InstitutionIDs);
            OnGet();
        }
        public IActionResult OnPostCreateTeacher()
        {
            Institution i = _institutionService.GetByID(Location);
            string Region = i.Location.Region;
            string City = i.Location.City;
            int PostalCode = i.Location.PostalCode;
            string RoadName = i.Location.RoadName;
            string RoadNumber = i.Location.RoadNumber;
            WeeklyHours = TimeSpan.FromHours(Hours);
            _teacherService.CreateTeacher(InstitutionID, Email, FirstName, LastName, WeeklyHours, HasCar, Region, City, PostalCode, RoadName, RoadNumber, AdminIDs);
            OnGet();            //We have to manually run the OnGet since refreshing the page
            return RedirectToPage("/Index"); //by using RedirectToPage() doesn't run it automatically
        }
        public IActionResult OnPostEditTeacherProfile()
        {   //This is the method used when a teacher edits themselves which is why we just send over the cookie instead of having the button do it
            return RedirectToPage("/EditTeacher", new { TeacherID = HttpContext.Request.Cookies["UserID"] });
        }
        public IActionResult OnPostEditTeacher()
        {   //In hindsight both edits could have just used this one
            return RedirectToPage("/EditTeacher", new { teacherID = TeacherID });
        }
        public void OnPostDeleteTeacher()
        {
            _teacherService.DeleteByID(TeacherID);
            OnGet();//We have to run the OnGet() or it won't pull up all the data in the DB and all table would be empty
        }
        public void OnPostDeleteInstitution()
        {
            _institutionService.DeleteByID(InstitutionID);
            OnGet();
        }

        /// <summary>
        /// This method is used to delete an admin Using their adminID
        /// </summary>
        public void OnPostDeleteAdmin()
        {
            _adminService.DeleteByID(AdminID);
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

        /// <summary>
        /// This method was used when it was necessary to update the unhashed passwords from the start of the projects dummy data.
        /// It has remained in case it is ever necessary to either change the encryption method again or to fix a fault if an unhashed value enters the password area.
        /// </summary>
        public void OnPostHashAdminPassword()
        {
            _adminService.HashThePassword(AdminID);
        }

        /// <summary>
        /// This method was used when it was necessary to update the unhashed passwords from the start of the projects dummy data.
        /// It has remained in case it is ever necessary to either change the encryption method again or to fix a fault if an unhashed value enters the password area.
        /// </summary>
        public void OnPostHashTeacherPassword()
        {
            _teacherService.HashThePassword(TeacherID);
        }

        /// <summary>
        /// This method redirects the admin User to the EditAdministrator page where it is possible to edit the admin objects.
        /// </summary>
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

        /// <summary>
        /// This method Logs out the user bu nulling their Coockies to default values, thus resetting index to the log in screen once more.
        /// This method also redirects you back to index in order to refresh the page.
        /// </summary>
        public IActionResult OnPostLogOut()
        {   
            HttpContext.Response.Cookies.Append("UserID", "0"); //If you logout we set your ID to zero
            HttpContext.Response.Cookies.Append("UserStatus", "false");
            return RedirectToPage();
        }
        public void OnPostChangeTeacherPassword()
        {
            OnGet(); //We need to run this or it will forget who the teacher is...
            //To create a new password they need to write it twice as a safety measure
            if (Pass == Pass2)
            { //If the wrote the same password in both fields we update the password in the DB
                _teacherService.ChangePass(TeacherID, Pass);
                ChangePasswordModalShow = true; //When you use an OnPost method it refreshes, but i want the modal to stay on screen
            }
            else
            {   //If both fields are not the same
                FailedToChangePass = true; //Used to make the red error box
                ChangePasswordModalShow = true;
            }
        }

        /// <summary>
        /// This method is used to change the password for the an already created Admin profile.
        /// It uses the admin service layer to call the ChangePassword method, and also sets properties to either false or true. these properties are used to display error messages.
        /// </summary>
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

        /// <summary>
        /// This method is used to create AClass objects, these objects show the classes/lektions the institutions have.
        /// </summary>
        public void OnPostCreateAClass()
        {
            TimeSpan duration = ClassDuration - ClassStart;
            _aClassService.Create(TeacherID, AdminID, InstitutionID, ClassStart, duration, ClassSubject, ClassDescription);
            OnGet();
        }

        /// <summary>
        /// This method is used if you are an admin and wish to see a teachers calender.
        /// It returns the value of a teachers specific ID and redirects you to another page where the ID is used to display that teachers calender.
        /// </summary>
        public IActionResult OnPostSeeTeacherCalender()
        {
            return RedirectToPage("/Calender", new { teacherID = TeacherID });
        }
    }
}
