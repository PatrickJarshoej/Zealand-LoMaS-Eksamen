using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zealand_LoMaS_Lib.Repo.Interfaces
{
    public interface IAdminRepo
    {
        public string GetPasswordByEmail(string Email);
        public int GetAdminIDByEmail(string Email);
        public string GetPasswordByAdminID(int AdminID);
        public void UpdatePassword(int adminID, string Password);
    }
}
