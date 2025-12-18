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

        /// <summary>
        /// This method is used to create an admin object and send it down to the repository-layer where it can be stored in the database.
        /// Additionally, it also creates a standard admin password that is hashed.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="institutionID"></param>
        public void Create(string email, string firstName, string lastName, List<int> institutionID)
        {
            Admin admin = new Admin(firstName, lastName, email, institutionID);
            string defaultPassword = Argon2.Hash("Admin");
            _adminRepo.Add(admin, defaultPassword);
        }

        /// <summary>
        /// This method gets a list of administrator objects from the repository-layer and returns the list to the UI-layer.
        /// </summary>
        /// <returns></returns>
        public List<Admin> GetAll()
        {
            return _adminRepo.GetAll();
        }

        /// <summary>
        /// This method is used when getting an admin objekt from the dataBase using an AdminID.
        /// </summary>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        public Admin GetByID(int AdminID)
        {
            Admin admin = _adminRepo.GetByID(AdminID);
            return admin;
        }

        /// <summary>
        /// This method is used to update the values of an administrator and its respektive maps in the database.
        /// It converts inputs from the UI arguments into understandable code the Repository can read.
        /// </summary>
        /// <param name="adminID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="institutionsIDs"></param>
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

        /// <summary>
        /// This method uses an adminID to delete an admin from the database that has that ID.
        /// </summary>
        /// <param name="adminID"></param>
        public void DeleteByID(int adminID)
        {
            _adminRepo.DeleteByID(adminID);
        }

        /// <summary>
        /// this method is used to change the current password in the database for a user.
        /// It requires the adminID of the current user and 2 password string that has to be identical.
        /// If the passwords differ the password will not be changed.
        /// </summary>
        /// <param name="adminID"></param>
        /// <param name="Password"></param>
        /// <param name="PasswordControl"></param>
        /// <returns></returns>
        public bool ChangePassword(int adminID, string Password, string PasswordControl)
        {
            bool WasItSuccess = false;
            if(Password == PasswordControl)
            {
                Debug.WriteLine("Password Succeeded verification");
                _adminRepo.UpdatePassword(adminID, Argon2.Hash(Password));
                return WasItSuccess = true;
            }
            else
            {
                Debug.WriteLine("Password failed verification");
                return WasItSuccess;
            }
        }

        /// <summary>
        /// This method uses af password string and email string to verify if they match the stored password for a given User in the database.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method was used to hash passwords from unhashed passwords in the database. it is no longer in use but kept in case it is ever needed again to update to another enryption method.
        /// </summary>
        /// <param name="adminID"></param>
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
