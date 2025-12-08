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
    public class EditInstitutionModel : PageModel
    {
        TeacherService _teacherService;
        InstitutionService _institutionService;
        public int InstitutionID { get; set; }
        public int Location { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string RoadName { get; set; }
        public string RoadNumber { get; set; }
        public List<int> AdminIDs { get; set; }
        public List<Institution> Institutions { get; set; }
        public Institution Institution { get; set; }
        public string Admins { get; set; }


        public EditInstitutionModel(TeacherService ts, InstitutionService iS)
        {
            _teacherService = ts;
            _institutionService = iS;
        }

        public void OnGet(int institutionID)
        {
            Institution = _institutionService.GetByID(institutionID);
            Admins = "";
            if (Institution.AdminIDs != null)
            {
                foreach(var a in Institution.AdminIDs)
                {
                    Admins += a.ToString() + ", ";
                }
            }
        }
        
        //public IActionResult OnPostSave()
        public void OnPostSave()
        {
            //Debug.WriteLine("Region: " + Region);
            _institutionService.Update(InstitutionID, Region, PostalCode, City, RoadName, RoadNumber, Admins, new List<int>());
            OnGet(InstitutionID);
            Admins = "";
            //return RedirectToPage("/Index");
        }
    }

}
