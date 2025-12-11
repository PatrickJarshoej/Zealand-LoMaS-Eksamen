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
    public class TransportModel : PageModel
    {
        TransportService _transportService;
        TeacherService _teacherService;
        InstitutionService _institutionService;
        public List<Transport> Transports { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Institution> Institutions { get; set; }
        public int TeacherID { get; set; }
        public DateTime Date { get; set; }
        public int InstituteToID { get; set; }
        public int TempID { get; set; }
        public Transport SpecificTransport { get; set; }
        public bool Edit { get; set; }
        public int InstituteFromID { get; set; }
        public double TimeMinute { get; set; }
        public double TimeHours { get; set; }
        public double Cost { get; set; }
        public TransportModel(TransportService ts, TeacherService teacherService,InstitutionService iS)
        {
            _transportService = ts;

            _teacherService = teacherService;

            _institutionService = iS;

        }

        public void OnGet(int transportID)
        {
            SpecificTransport=_transportService.GetByID(transportID);
            TempID = transportID;
            Institutions = _institutionService.GetAll();
            Teachers = _teacherService.GetAll();
            Transports = _transportService.GetAll();
        }
        public IActionResult OnPostDelete()
        {
            _transportService.DeleteByID(TempID);
            return RedirectToPage("Index");
        }
        public void OnPostEdit()
        {
            TimeSpan theTimeSpan=TimeSpan.FromHours(TimeHours)+TimeSpan.FromMinutes(TimeMinute);
            Debug.WriteLine(Cost);
            _transportService.Update(TempID, Date, Cost, theTimeSpan);
            OnGet(TempID);
        }
    }

}
