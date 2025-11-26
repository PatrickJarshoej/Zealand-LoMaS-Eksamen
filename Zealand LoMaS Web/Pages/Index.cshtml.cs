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
        public bool IsLoggedInAdmin { get; set; } = false;
        [BindProperty]
        public bool IsLoggedInteacher { get; set; } = false;
        [BindProperty]
        public bool FailedToLogIn { get; set; } = false;


        public IndexModel(ILogger<IndexModel> logger, AdminService adminService, TeacherService teacherService)
        {
            _logger = logger;
            _adminService = adminService;
            _teacherService = teacherService;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            Debug.WriteLine("Du kører den forkerte on post");
        }
        public void OnPostLogIn()
        {
            Console.WriteLine(Email);
            Console.WriteLine(Pass);
            IsLoggedInAdmin = _adminService.CheckLogIn(Email, Pass);
            IsLoggedInteacher = _teacherService.CheckLogIn(Email, Pass);
            if (IsLoggedInAdmin == true)
            {
                Console.WriteLine("Admin er logged in");
            }
            else if (IsLoggedInteacher == true)
            {
                Console.WriteLine("Lærer er logged in");
            }
            else
            {
                Console.WriteLine("Did not log in");
                FailedToLogIn = true;
            }
            //User user = _userService.CheckPassword(Userid, Pass);
            //if (user.UserID == 0)
            //{
            //    Debug.WriteLine("Error in Username or password");
            //    IsLoggedIn = false;
            //    Debug.WriteLine($"{user.FirstName} is logged in? {IsLoggedIn}");
            //    return RedirectToPage("/Index");
            //}
            //else
            //{
            //    IsLoggedIn = true;
            //    Debug.WriteLine($"{user.FirstName} is logged in? {IsLoggedIn}");
            //    return RedirectToPage("/Profile", user);
            //}
        }
    }
}
