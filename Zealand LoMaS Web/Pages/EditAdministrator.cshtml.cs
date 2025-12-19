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
        public List<Institution> Institutions { get; set; }
        public string InstitutionsIDs { get; set; }



        public EditAdministratorModel(AdminService adminService, InstitutionService iS)
        {
            _adminService = adminService;
            _institutionService = iS;
        }

        /// <summary>
        /// This method is used whenever you enter this webpage.
        /// it requires the parameter for the administrator that is in need of editing.
        /// once it has this information it will get the current Values stored in the database for this User and put them into the bound properties.
        /// </summary>
        /// <param name="adminID"></param>
        public void OnGet(int adminID)
        {
            AdminID = adminID;
            Admin = _adminService.GetByID(adminID);
            FirstName = Admin.FirstName;
            LastName = Admin.LastName;
            Email = Admin.Email;
            InstitutionIDs = Admin.InstitutionIDs;
            InstitutionsIDs = "";
            if (Admin.InstitutionIDs != null)
            {
                foreach (var a in Admin.InstitutionIDs)
                {
                    InstitutionsIDs += a.ToString() + ", ";
                }
            }
            Debug.WriteLine("OnGet: " + AdminID);


        }

        /// <summary>
        /// This method calls the adminService update method to update the given administrator with the current Bound propperties.
        /// Afterwards you are redirected to the index page again.
        /// </summary>
        public IActionResult OnPostSave()
        {
            Debug.WriteLine("Save: " + AdminID);
            _adminService.Update(AdminID, FirstName, LastName, Email, InstitutionsIDs);
            return RedirectToPage("/Index");
        }
    }

}
