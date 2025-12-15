using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Repo.Interfaces
{
    public interface IAdminRepo
    {
        public void Add(Admin adminObject, string Password);
        public List<Admin> GetAll();
        public Admin GetByID(int adminID);
        public void Update(Admin adminObject);
        public void DeleteByID(int adminID);
        public string GetPasswordByEmail(string Email);
        public int GetAdminIDByEmail(string Email);
        public string GetPasswordByAdminID(int AdminID);
        public void UpdatePassword(int adminID, string Password);
    }
}
