using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;


namespace Zealand_LoMaS_Web.Pages
{
    [BindProperties]
    public class EditRelationsModel : PageModel
    {

        InstitutionRelationService _institutionRelationService;
        InstitutionService _institutionService;

        public InstitutionRelation TheRelation {  get; set; }

        public List<int> Ids { get; set; } = new List<int>();
        public double Cost { get; set; }
        public TimeSpan Time {  get; set; }
        public Institution Institution1 { get; set; }
        public Institution Institution2 { get; set; }

        public int ID1 {  get; set; }
        public int ID2 { get; set; }


        public int TimeNumber { get; set; }

        public bool Edit { get; set; }
        
        public EditRelationsModel(InstitutionRelationService rs, InstitutionService iS)
        {
            _institutionRelationService = rs;
            _institutionService = iS;

        }

        public void OnGet(List<int> IDs)
        {
            Ids.Add(IDs[0]);
            Ids.Add(IDs[1]);
            ID1 = IDs[0];
            ID2= IDs[1];
            TheRelation = _institutionRelationService.GetByIDs(IDs[0], IDs[1]);
            Institution1=_institutionService.GetByID(IDs[0]);
            Institution2 = _institutionService.GetByID(IDs[1]);
        }

        public IActionResult OnPostSave()
        {

            Ids.Add(ID1);
            Ids.Add(ID2);
            OnGet(Ids);
            _institutionRelationService.Update(Ids[0], Ids[1],Cost, Time);
            //Admins = "";
            return RedirectToPage("/Index");
        }
    }

}
