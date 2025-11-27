using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Service
{
    public class AdminService
    {
        private IAdminRepo _adminRepo;

        public AdminService(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }
        public int LogIn(string Email, string Password)
        {
            int adminID = _adminRepo.GetLogIn(Email, Password);
            return adminID;
        }
    }
}
