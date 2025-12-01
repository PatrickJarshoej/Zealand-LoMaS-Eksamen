using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;
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
        public int VerifyLogIn(string email, string password)
        {
            string hashedpassword = Argon2.Hash(password);
            int adminID = _adminRepo.GetAdminIDByEmail(email);
            string StoredPassword = _adminRepo.GetPasswordByEmail(email);
            if (hashedpassword == StoredPassword)
            {
                return adminID;
            }
            else
            {
                adminID = 0;
                return adminID;
            }
        }
        public void HashThePassword(int adminID)
        {
            string pass = _adminRepo.GetPasswordByAdminID(adminID);
            if (pass != "0")
            {
                pass = Argon2.Hash(pass);
                _adminRepo.UpdatePassword(adminID, pass);
            }
            else
            {
                Debug.WriteLine("Failed to find password");
            }
        }
    }
}
