using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_LoMaS_Lib.Service;

namespace Zealand_LoMaS_Web.Pages
{
    public class IndexModel : PageModel
    {
        private AdminService _adminService;
        private TeacherService _teacherService;
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
        public string CookieID { get; set; } = "0";
        [BindProperty]
        public string CookieIsAdmin { get; set; } = "false";



        public IndexModel(ILogger<IndexModel> logger, AdminService adminService, TeacherService teacherService)
        {
            _logger = logger;
            _adminService = adminService;
            _teacherService = teacherService;
        }

        public void OnGet()
        {
            //HttpContext.Request.Cookies("UserID", CookieID);
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

            }
            else if (TeacherID != 0)
            {
                Debug.WriteLine("Lærer er logged in");
                CookieID = Convert.ToString(TeacherID);
            }
            else
            {
                Debug.WriteLine("Did not log in");
                FailedToLogIn = true;
            }
        }
    }
}
