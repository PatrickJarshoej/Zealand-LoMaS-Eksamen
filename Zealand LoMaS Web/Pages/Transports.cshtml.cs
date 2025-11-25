using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Service;


namespace Zealand_LoMaS_Web.Pages
{
    public class TransportModel : PageModel
    {
        TransportService _transportService;
        public List<Transport> Transports { get; set; }
        [BindProperty]
        public int TeacherID { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public int InstituteToID { get; set; }
        [BindProperty]
        public int InstituteFromID { get; set; }
        public TransportModel(TransportService ts)
        {
            
        }

        public void OnGet()
        {
        }
        public IActionResult OnPostCreate()
        {
            _transportService.Create(TeacherID, Date, InstituteFromID, InstituteToID);
            return RedirectToPage("/Domiciles");
        }
    }

}
