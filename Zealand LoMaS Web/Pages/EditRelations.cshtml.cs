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

        public InstitutionRelation TheRelation {  get; set; }

        public List<int> Ids { get; set; }
        public double Cost { get; set; }
        public TimeSpan Time {  get; set; }
        public Institution TheInstitution { get; set; }

        public int TimeNumber { get; set; }

        public bool Edit { get; set; }
        
        public EditRelationsModel(InstitutionRelationService rs)
        {
            _institutionRelationService = rs;
        }

        public void OnGet(List<int> IDs)
        {
            Debug.WriteLine(IDs.Count);
            TheRelation = _institutionRelationService.GetByIDs(IDs[0], IDs[1]);
        }
        public void OnPostEdit()
        {

        }
    }

}
