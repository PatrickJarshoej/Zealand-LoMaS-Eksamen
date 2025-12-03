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
                var command = new SqlCommand("SELECT Password FROM AdministratorPasswords WHERE AdministratorID = (Select AdministratorID from Administrators where Email = @Email)", connection);
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

        public string GetPasswordByAdminID(int AdminID)
        {
            string adminPassword = "0";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT Password FROM AdministratorPasswords WHERE AdministratorID = @AdministratorID", connection);
                command.Parameters.AddWithValue("@AdministratorID", AdminID);
                connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            adminPassword = (string)reader["Password"];
                        }
                        return adminPassword;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo GetPasswordByAdminID");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return adminPassword;
        }

        public void UpdatePassword(int adminID, string Password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("Update AdministratorPasswords SET Password = @Password WHERE AdministratorID = @AdministratorID", connection);
                command.Parameters.AddWithValue("@AdministratorID", adminID);
                command.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo UpdatePassword");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void Add(Admin adminObject, string Password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Administrators (FirstName, LastName, Email, InstitutionID) VALUES (@FirstName, @LastName, @Email, @InstitutionID)", connection);
                command.Parameters.AddWithValue("@FirstName", adminObject.FirstName);
                command.Parameters.AddWithValue("@LastName", adminObject.LastName);
                command.Parameters.AddWithValue("@Email", adminObject.Email);
                command.Parameters.AddWithValue("@InstitutionID", adminObject.InstitutionID);
                var command2 = new SqlCommand("INSERT INTO AdministratorPasswords (AdministratorID, Password) VALUES ((SELECT AdministratorID FROM Administrators WHERE Email = @Email), @Password)");
                command2.Parameters.AddWithValue("@Email", adminObject.Email);
                command2.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                    command2.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo Add()");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Admin> GetAll()
        {
            throw new NotImplementedException();
        }

        public int GetByID(int adminID)
        {
            throw new NotImplementedException();
        }

        public List<Admin> GetByInstitutionID(int instituitonID)
        {
            throw new NotImplementedException();
        }

        public List<Admin> GetAdminsByTeacherID(int instituitonID)
        {
            throw new NotImplementedException();
        }

        public string GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void Update(Admin adminObject)
        {
            throw new NotImplementedException();
        }

        public void DeleteByID(int adminID)
        {
            throw new NotImplementedException();
        }
    }
}
