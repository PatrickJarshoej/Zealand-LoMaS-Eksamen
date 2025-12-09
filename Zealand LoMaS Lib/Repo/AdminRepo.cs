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
        private List<Admin> GetAdminsByCommand(SqlCommand command, SqlConnection connection)
        {
            var admins = new List<Admin>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Admin admin = new Admin
                    (
                        (int)reader["AdministratorID"],
                        (string)reader["FirstName"],
                        (string)reader["LastName"],
                        (string)reader["Email"],
                        GetInstitutions((int)reader["AdministratorID"], connection)
                    );
                    admins.Add(admin);
                }
            }
            return admins;
        }
        public List<int> GetInstitutions(int adminID, SqlConnection connection)
        {
            List<int> institutionIDs = new();
            try
            {
                var command = new SqlCommand("SELECT * FROM MapInstitutionsAdministrators WHERE AdministratorID = @ID", connection);
                command.Parameters.AddWithValue("@ID", adminID);
                //connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        institutionIDs.Add((int)reader["InstitutionID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetInstitutions in AdminRepo");
                Debug.WriteLine($"Error: {ex.Message}");
            }

            return institutionIDs;
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
                command.Parameters.AddWithValue("@InstitutionID", adminObject.InstitutionIDs);
                var command2 = new SqlCommand("INSERT INTO AdministratorPasswords (AdministratorID, Password) VALUES ((SELECT AdministratorID FROM Administrators WHERE Email = @Email), @Password)", connection);
                command2.Parameters.AddWithValue("@Email", adminObject.Email);
                command2.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();

                    command2.ExecuteNonQuery();
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
            List<Admin> admins = new();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Administrators", connection);
                    connection.Open();
                    admins = GetAdminsByCommand(command, connection);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in AdminRepo");
                    Debug.WriteLine("Error: " + ex);
                }
                finally { connection.Close(); }

            }
            return admins;
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
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("Update Administrators SET Firstname = @FirstName, LastName = @LastName, Email = @Email WHERE AdministratorID = @AdministratorID", connection);
                command.Parameters.AddWithValue("@Firstname", adminObject.FirstName);
                command.Parameters.AddWithValue("@Lastname", adminObject.LastName);
                command.Parameters.AddWithValue("@Email", adminObject.Email);
                command.Parameters.AddWithValue("@AdministratorID", adminObject.AdministratorID);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo Update");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void DeleteByID(int adminID)
        {
            throw new NotImplementedException();
        }
    }
}
