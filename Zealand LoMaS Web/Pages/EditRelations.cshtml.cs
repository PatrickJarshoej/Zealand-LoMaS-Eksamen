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
        public double TimeMinute { get; set; }
        public double TimeHours { get; set; }
        public EditRelationsModel(InstitutionRelationService rs, InstitutionService iS)
        {
            _institutionRelationService = rs;
            _institutionService = iS;

        }
        /// <summary>
        /// Uses the ID's it gets, to then get the related Relation to the institution. 
        /// Also gets the related institutions in order to display the proper city names of those institutions
        /// </summary>
        /// <param name="IDs"></param>
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
        /// <summary>
        /// Edits the relations, then redirects you back to index, since you probably don't need to edit it twice.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostSave()
        {
            Ids.Add(ID1);
            Ids.Add(ID2);
            OnGet(Ids);
            Time = TimeSpan.FromMinutes(TimeMinute)+TimeSpan.FromHours(TimeHours);
            _institutionRelationService.Update(Ids[0], Ids[1],Cost, Time);
            return RedirectToPage("/Index");
        }
    }

}
