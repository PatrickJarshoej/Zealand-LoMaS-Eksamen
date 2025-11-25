using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Repo
{
    public class AdminRepo : IAdminRepo
    {
        private string _connectionString;
        public AdminRepo()
        {
            _connectionString = "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
        public bool CheckLogIn(string Email, string Password)
        {
            Console.WriteLine("Repo");
            bool AdminIsLoginCorrect = false;
            using (var connection = new SqlConnection(_connectionString))
            {
            Console.WriteLine("Repo1");
                var command = new SqlCommand("SELECT AdministratorID from Administrators WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", Email);
                var command2 = new SqlCommand("SELECT AdministratorID from AdministratorPassword WHERE Password = @Password and AdministratorID = @AdministratorID", connection);
                command2.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                try
                {

                    Console.WriteLine(Email);
                    Console.WriteLine(Password);
                    using (var reader = command.ExecuteReader())
                    {
                        int AdminID = (int)reader["AdministratorID"];
                        Console.WriteLine("hep1");
                        Console.WriteLine(AdminID);
                        command2.Parameters.AddWithValue("@AdministratorID", AdminID);
                        using (var reader2 = command2.ExecuteReader())
                        {
                            Console.WriteLine("Hep2");
                            AdminIsLoginCorrect = true;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo CheckLogIn");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return AdminIsLoginCorrect;
        }
    }
}
