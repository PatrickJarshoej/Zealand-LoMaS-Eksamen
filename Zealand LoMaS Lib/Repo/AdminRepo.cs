using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Zealand_LoMaS_Lib.Model;

namespace Zealand_LoMaS_Lib.Repo
{
    public class AdminRepo : IAdminRepo
    {
        private string _connectionString;
        public AdminRepo()
        {
            _connectionString = "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
        public string GetPasswordByEmail(string Email)
        {
            string adminPass = "0";
            using (var connection = new SqlConnection(_connectionString))
            {
                //var command = new SqlCommand("SELECT * FROM Administrators WHERE Email = @Email AND (select AdministratorID FROM Administrators WHERE Email = @Email) = ALL (Select AdministratorID FROM AdministratorPasswords WHERE Password = @Password)", connection);
                var command = new SqlCommand("SELECT Password FROM AdministratorPasswords WHERE AdministratorID = (Select AdministratorID from Administrators where Email = @Email);", connection);
                command.Parameters.AddWithValue("@Email", Email);
                connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            adminPass = (string)reader["Password"];
                        }
                        return adminPass;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo GetPasswordByEmail");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return adminPass;
        }
        public int GetAdminIDByEmail(string Email)
        {
            int adminID = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                //var command = new SqlCommand("SELECT * FROM Administrators WHERE Email = @Email AND (select AdministratorID FROM Administrators WHERE Email = @Email) = ALL (Select AdministratorID FROM AdministratorPasswords WHERE Password = @Password)", connection);
                var command = new SqlCommand("SELECT AdministratorID FROM Administrators WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", Email);
                connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            adminID = (int)reader["AdministratorID"];
                        }
                        return adminID;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo GetAdminByEmail");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return adminID;
        }
    }
}
