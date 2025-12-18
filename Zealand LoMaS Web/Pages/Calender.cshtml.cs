using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;
using System.Diagnostics;
using Microsoft.Identity.Client;


namespace Zealand_LoMaS_Web.Pages
{
    [BindProperties]
    public class CalenderModel : PageModel
    {
        private TeacherService _teacherService;
        private AClassService _aClassService;

        public Teacher Teacher { get; set; }
        public int TeacherID { get; set; } = 0;

        public CalenderModel(TeacherService teacherService, AClassService acs)
        {
            _teacherService = teacherService;
            _aClassService = acs;

        }

        /// <summary>
        /// This method requires a teacherID to work.
        /// as such it runs an if statement where it first checks if you are a teacher and if so it sets your coockie to the teacherID and gets the remaining values from the database and stores it in a Teacher object.
        /// if an admin enters the page if it dies not have a parameter of teacherID with it, it will be returned to the index page again.
        /// This also has a saveguard for if an admin refreshes the page by looking for whether there currently is a bound property called TeacherID
        /// </summary>
        /// <param name="teacherID"></param>
        public void OnGet(int teacherID)
        {
            if(HttpContext.Request.Cookies["UserStatus"] == "false" && HttpContext.Request.Cookies["UserID"] != "0")
            {
                Teacher = _teacherService.GetByID((Convert.ToInt32(HttpContext.Request.Cookies["UserID"])));
                TeacherID = Convert.ToInt32(HttpContext.Request.Cookies["UserID"]);
            }
            else if ((HttpContext.Request.Cookies["UserStatus"] == "true" && teacherID != 0) || TeacherID != 0)
            {
                TeacherID = teacherID;
                Teacher = _teacherService.GetByID(TeacherID);
            }
            else
            {
                RedirectToPage("/Index");
            }
        }
        
    }

}
