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
        private IInstitutionRepo _institutionRepo;

        public AdminService(IAdminRepo adminRepo, IInstitutionRepo institutionRepo)
        {
            _adminRepo = adminRepo;
            _institutionRepo = institutionRepo;
        }
        public void Create(string email, string firstName, string lastName, List<int> institutionID)
        {
            Admin admin = new Admin(firstName, lastName, email, institutionID);
            string defaultPassword = Argon2.Hash("Admin");
            _adminRepo.Add(admin, defaultPassword);
        }
        public List<Admin> GetAll()
        {
            return _adminRepo.GetAll();
        }
        public Admin GetByID(int AdminID)
        {
            Admin admin = _adminRepo.GetByID(AdminID);
            return admin;
        }
        public Admin GetByEmail(string email)
        {
            Admin admin = new();
            return admin;
        }
        public Admin GetByInstitutionID(int institutionID)
        {
            Admin admin = new();
            return admin;
        }
        public void Update(int adminID, string firstName, string lastName, string email, string institutionsIDs)
        {
            List<int> institutionIDs = new();
            if (institutionsIDs != "")
            {
                List<string> iID = institutionsIDs.Split(',').ToList<string>();
                foreach (var i in iID)
                {
                    if (i != " ")
                    {
                        institutionIDs.Add(Convert.ToInt32(i));

                    }
                }
            }
            Admin admin = new(adminID, firstName, lastName, email, institutionIDs);
            _institutionRepo.UpdateMapAdminInstitute(adminID, institutionIDs);
            _adminRepo.Update(admin);
        }
        public void DeleteByID(int AdminID)
        {
            throw new NotImplementedException();
        }
        public void ChangePassword(int adminID, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
        public int VerifyLogIn(string email, string password)
        {
            int adminID = _adminRepo.GetAdminIDByEmail(email);
            string StoredPassword = _adminRepo.GetPasswordByEmail(email);
            if (Argon2.Verify(StoredPassword, password))
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
