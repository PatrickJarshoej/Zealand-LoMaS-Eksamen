using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Zealand_LoMaS_Lib.Service;

namespace Zealand_LoMaS_Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AdminService _adminService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Pass { get; set; }
        //[BindProperty]
        //public bool IsLoggedIn { get; set; } = false;


        public IndexModel(ILogger<IndexModel> logger, AdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            Debug.WriteLine("Du kører den forkerte on post");
        }
        public void  OnPostLogIn()
        {
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
