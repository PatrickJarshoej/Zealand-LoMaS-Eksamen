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
    public class EditAdministratorModel : PageModel
    {
        AdminService _adminService;
        InstitutionService _institutionService;
        public Admin Admin { get; set; }
        public int AdminID { get; set; }
        public List<int> InstitutionIDs { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<int> TeacherIDs { get; set; }
        public List<Institution> Institutions { get; set; }
        public Institution Institution { get; set; }



        public EditAdministratorModel(AdminService adminService, InstitutionService iS)
        {
            _adminService = adminService;
            _institutionService = iS;
        }

        public void OnGet(int adminID)
        {
            AdminID = adminID;
            Admin = _adminService.GetByID(adminID);
            //Teachers = _teacherService.GetAll();
            Institutions = _institutionService.GetAll();
        }
        
        public IActionResult OnPostSave()
        {

            Institutions = _institutionService.GetAll();

            //_adminService.Update(TeacherID, InstitutionID, Email, FirstName, LastName, WeeklyHours, HasCar, Region, City, PostalCode, RoadName, RoadNumber, AdminIDs);
            return RedirectToPage("/Index");
        }
    }

}
