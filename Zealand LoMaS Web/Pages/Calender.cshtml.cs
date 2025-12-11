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



        public CalenderModel(TeacherService teacherService, AClassService acs)
        {
            _teacherService = teacherService;
            _aClassService = acs;

        }

        public void OnGet()
        {

        }
        
    }

}
