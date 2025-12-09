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
        InstitutionRelationService _relationService;
        public List<InstitutionRelation> InstitutionRelations { get; set; }
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
        public Institution OtherInstitution { get; set; }
        public string Admins { get; set; }


        public EditInstitutionModel(TeacherService ts, InstitutionService iS, InstitutionRelationService rS)
        {
            _teacherService = ts;
            _institutionService = iS;
            _relationService = rS;
        }

        public void OnGet(int institutionID)
        {
            Institution = _institutionService.GetByID(institutionID);
            Institutions= _institutionService.GetAll();
            var OtherInstitutions = Institutions.Where(x => x != Institution).OrderBy(x=>x.InstitutionID).ToList();
            //Admins = "";
            if (Institution.AdminIDs != null)
            {
                foreach(var a in Institution.AdminIDs)
                {
                    Admins += a.ToString() + ", ";
                }
            }
            InstitutionRelations=_relationService.GetByID(institutionID);
            
            
        }
        
        public IActionResult OnPostSave()
        {
            _institutionService.Update(InstitutionID, Region, PostalCode, City, RoadName, RoadNumber, Admins, new List<int>());
            //Admins = "";
            return RedirectToPage("/Index");
        }
    }

}
