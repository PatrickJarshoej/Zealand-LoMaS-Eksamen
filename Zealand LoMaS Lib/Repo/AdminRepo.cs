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
        /// <summary>
        /// Retrieves a list of admins based on the specified SQL command.
        /// </summary>
        /// <remarks>The method executes the provided SQL command and reads the resulting data to
        /// construct a list of  <see cref="Admin"/> objects </remarks>
        /// <param name="command">The <see cref="SqlCommand"/> to execute for retrieving teacher data.</param>
        /// <param name="connection">The <see cref="SqlConnection"/> used to execute the command and retrieve related data.</param>
        /// <returns>A list of <see cref="Admin"/> objects populated with data retrieved from the database.</returns>
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
        /// <summary>
        /// Gets a list of all <see cref="Institution"/> connected to the given <see cref="Admin"/>
        /// </summary>
        /// <param name="adminID"> The unique identifier of the admin whose associated Institutions you would like to get</param>
        /// <param name="connection"> An open <see cref="SqlConnection"/> connected to the database</param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is used to get the passwords from the AdministratorPasswords table in the database.
        /// in order for it to only use one sql-command it runs 2 select statements in one command. This causes the only input needed to just email.
        /// Afterwards it returns a string value of the password found in the table.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is used to get the administratorID stored in the administrator table from the database using only email.
        /// This returns an int value with the ID stored in it
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is used to retrieve a password from the AdministratorPassword table in the database using an administratorID.
        /// </summary>
        /// <param name="AdminID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method add's a newly created admin object into the database.
        /// It does this using 3 sql statements and a Scalar funktion. The tables used is the Administrators table, AdministratorPasswords and MapInstitutionsAdministrators.
        /// </summary>
        /// <param name="adminObject"></param>
        /// <param name="Password"></param>
        public void Add(Admin adminObject, string Password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    var command = new SqlCommand("INSERT INTO Administrators (FirstName, LastName, Email) OUTPUT Inserted.AdministratorID VALUES (@FirstName, @LastName, @Email)", connection);
                    command.Parameters.AddWithValue("@FirstName", adminObject.FirstName);
                    command.Parameters.AddWithValue("@LastName", adminObject.LastName);
                    command.Parameters.AddWithValue("@Email", adminObject.Email);
                    int AdminID = (int)command.ExecuteScalar();

                    var command2 = new SqlCommand("INSERT INTO AdministratorPasswords (AdministratorID, Password) VALUES (@AdministratorID, @Password)", connection);
                    command2.Parameters.AddWithValue("@AdministratorID", AdminID);
                    command2.Parameters.AddWithValue("@Password", Password);
                    command2.ExecuteNonQuery();

                    var command3 = new SqlCommand("INSERT INTO MapInstitutionsAdministrators (InstitutionID, AdministratorID) VALUES (@InstitutionID, @AdministratorID)", connection);
                    command3.Parameters.AddWithValue("@AdministratorID", AdminID);
                    command3.Parameters.AddWithValue("@InstitutionID", adminObject.InstitutionIDs[0]);
                    command3.ExecuteNonQuery();


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

        /// <summary>
        /// This method retrieves a list of objects that contains all the administrators in the administrator table.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// thhis method gets an admin based on an int ID value.
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns></returns>
        public Admin GetByID(int adminID)
        {
            Debug.WriteLine("Repo: " + adminID);
            Admin admin = new();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Administrators WHERE AdministratorID = @AdministratorID", connection);
                    command.Parameters.AddWithValue("@AdministratorID", adminID);
                    connection.Open();
                    admin = GetAdminsByCommand(command, connection)[0];


                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo GetByID");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return admin;
        }

        /// <summary>
        /// This method is used to update an already created Admin in the Administrators table in the database.
        /// </summary>
        /// <param name="adminObject"></param>
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

        /// <summary>
        /// This method deletes an administrator from the administrators table using an int ID value.
        /// </summary>
        /// <param name="adminID"></param>
        public void DeleteByID(int adminID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("Delete FROM Administrators WHERE AdministratorID=@AdministratorID", connection);
                    command.Parameters.AddWithValue("@AdministratorID", adminID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in DeleteByID() in AdminRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }
            }
        }
    }
}
