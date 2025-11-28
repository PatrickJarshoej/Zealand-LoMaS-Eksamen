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
    public class TransportModel : PageModel
    {
        TransportService _transportService;
        TeacherService _teacherService;
        InstitutionService _institutionService;
        public List<Transport> Transports { get; set; }
        [BindProperty]
        public List<Teacher> Teachers { get; set; }
        [BindProperty]
        public List<Institution> Institutions { get; set; }
        [BindProperty]
        public int TeacherID { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public int InstituteToID { get; set; }
        [BindProperty]
        public int TempID { get; set; }
        [BindProperty]
        public Transport SpecificTransport { get; set; }
        [BindProperty]
        public bool Edit { get; set; }
        [BindProperty]
        public int InstituteFromID { get; set; }
        public TransportModel(TransportService ts, TeacherService teacherService,InstitutionService iS)
        {
            _transportService = ts;
            Transports = ts.GetAll();
            _teacherService = teacherService;
            Teachers=_teacherService.GetAll();
            _institutionService = iS;
            Institutions=_institutionService.GetAll();
        }

        public void OnGet(int transportID)
        {
            SpecificTransport=_transportService.GetByID(transportID);
            TempID = transportID;
        }
        public void OnPostEdit()
        {
            Debug.WriteLine("Temp Domicile ID: " + TempID);
            Edit = true;
            SpecificTransport = _transportService.GetByID(TempID);
        }
    }

}
