using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Repo
{
    public class TeacherRepo : ITeacherRepo
    {
        private string _connectionString;
        public TeacherRepo()
        {
            _connectionString = "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }

        private List<Teacher> GetTeachersByCommand(SqlCommand command, SqlConnection connection)
        {
            var teachers = new List<Teacher>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //Debug.WriteLine("Hello, World!");
                    Teacher teacher = new Teacher
                    (
                        (int)reader["TeacherID"],
                        //0,
                        GetInstitutionID((int)reader["TeacherID"], connection),
                        (string)reader["Email"],
                        (string)reader["FirstName"],
                        (string)reader["LastName"],
                        TimeSpan.FromHours(Decimal.ToDouble((decimal)reader["WeeklyHours"])),
                        (bool)reader["HasCar"],
                        GetAddress((int)reader["TeacherID"], connection),
                        GetAdmins((int)reader["TeacherID"], connection)
                    );
                    teachers.Add(teacher);
                }
            }
            return (teachers);
        }
        private int GetInstitutionID(int teacherID, SqlConnection connection)
        {
            //Debug.WriteLine("Get InstitutionID!");
            int institutionID = 0;

            try
            {
                var command = new SqlCommand("SELECT InstitutionID FROM MapInstitutionsTeachers WHERE TeacherID = @ID", connection);
                command.Parameters.AddWithValue("@ID", teacherID);
                using (var reader2 = command.ExecuteReader())
                {

                    if (reader2.Read())
                    {
                        institutionID = (int)reader2["InstitutionID"];

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetInstitutionID() in TeacherRepo");
                Debug.WriteLine($"Error: {ex.Message}");
            }
            return institutionID;
        }
        private Address GetAddress(int id, SqlConnection connection)
        {
            Address address = null;

            try
            {
                var command = new SqlCommand("SELECT * FROM TeacherAddress WHERE TeacherID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);

                //connection.Open();
                using (var reader3 = command.ExecuteReader())
                {
                    if (reader3.Read())
                    {
                        address = new Address
                        (
                            (string)reader3["Region"],
                            (string)reader3["City"],
                            (int)reader3["PostalCode"],
                            (string)reader3["RoadName"],
                            (string)reader3["RoadNumber"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAddress() in TeacherRepo");
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
            }


            return address;
        }
        private List<int> GetAdmins(int id, SqlConnection connection)
        {
            List<int> adminIDs = new List<int>();
            try
            {
                var command = new SqlCommand("SELECT * FROM MapAdministratorsTeachers WHERE TeacherID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                //connection.Open();
                using (var reader4 = command.ExecuteReader())
                {
                    while (reader4.Read())
                    {
                        adminIDs.Add((int)reader4["AdministratorID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAdmin() in TeacherRepo");
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
            }

            return adminIDs;
        }
        public void Add(Teacher teacher, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO Teachers (Email, FirstName, LastName, WeeklyHours, HasCar) OUTPUT Inserted.TeacherID VALUES (@Email, @FirstName, @LastName, @WeeklyHours, @HasCar);", connection);
                    command.Parameters.AddWithValue("@Email", teacher.Email);
                    command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                    command.Parameters.AddWithValue("@LastName", teacher.LastName);
                    command.Parameters.AddWithValue("@WeeklyHours", teacher.WeeklyHours.TotalHours);
                    command.Parameters.AddWithValue("@HasCar", teacher.HasCar);
                    connection.Open();
                    int teacherID = (int)command.ExecuteScalar();

                    AddTeacherAddress(teacher, teacherID, connection);
                    AddTeacherAdmins(teacher, teacherID, connection);
                    AddTeacherInstitution(teacher, teacherID, connection);
                    AddTeacherPassword(teacher, teacherID, password, connection);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in AddTeacher() in TeacherRepo");
                    Debug.WriteLine("Error" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void AddTeacherAddress(Teacher teacher, int tID, SqlConnection connection)
        {
            try
            {
                var command2 = new SqlCommand("INSERT INTO TeacherAddress (TeacherID, Region, City, PostalCode, RoadName, RoadNumber) VALUES (@ID, @Region, @City, @PostalCode, @RoadName, @RoadNumber);", connection);
                command2.Parameters.AddWithValue("@ID", tID);
                command2.Parameters.AddWithValue("@Region", teacher.Address.Region);
                command2.Parameters.AddWithValue("@City", teacher.Address.City);
                command2.Parameters.AddWithValue("@PostalCode", teacher.Address.PostalCode);
                command2.Parameters.AddWithValue("@RoadName", teacher.Address.RoadName);
                command2.Parameters.AddWithValue("@RoadNumber", teacher.Address.RoadNumber);
                command2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddTeacherAddress() in TeacherRepo");
                Debug.WriteLine("Error" + ex.Message);
            }
            finally
            {

            }
        }

        private void AddTeacherAdmins(Teacher teacher, int tID, SqlConnection connection)
        {
            try
            {
                foreach (var adminID in teacher.AdminIDs)
                {
                    var command3 = new SqlCommand("INSERT INTO MapAdministratorsTeachers (TeacherID, AdministratorID) VALUES (@TeacherID, @AdminID);", connection);
                    command3.Parameters.AddWithValue("@TeacherID", tID);
                    command3.Parameters.AddWithValue("@AdminID", adminID);
                    command3.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddTeacherAdmins() in TeacherRepo");
                Debug.WriteLine("Error" + ex.Message);
            }
            finally
            {
            }
        }

        private void AddTeacherClasses(Teacher teacher, int tID, SqlConnection connection)
        {
            try
            {
                foreach (var classID in teacher.ClassIDs)
                {
                    var command4 = new SqlCommand("INSERT INTO MapTeacherClasses (TeacherID, ClassID) VALUES (@TeacherID, @ClassID);", connection);
                    command4.Parameters.AddWithValue("@TeacherID", tID);
                    command4.Parameters.AddWithValue("@ClassID", classID);
                    command4.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddTeacherClasses() in TeacherRepo");
                Debug.WriteLine("Error" + ex.Message);
            }
            finally
            {
            }
        }

        private void AddTeacherCompetencies(Teacher teacher, int tID, SqlConnection connection)
        {
            try
            {
                foreach (var competency in teacher.Competencies)
                {
                    var command5 = new SqlCommand("INSERT INTO MapCompetenciesTeachers (TeacherID, Name, Description, DateTaken) VALUES (@TeacherID, @Name, @Description, @DateTaken);", connection);
                    command5.Parameters.AddWithValue("@TeacherID", tID);
                    command5.Parameters.AddWithValue("@Name", competency.Name);
                    command5.Parameters.AddWithValue("@Description", competency.Description);
                    command5.Parameters.AddWithValue("@DateTaken", competency.DateTaken);
                    command5.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddCompetencies() in TeacherRepo");
                Debug.WriteLine("Error" + ex.Message);
            }
            finally
            {
            }
        }

        private void AddTeacherPassword(Teacher teacher, int teacherID, string password, SqlConnection connection)
        {
            try
            {
                
                var command6 = new SqlCommand("INSERT INTO TeacherPasswords (TeacherID, Password) VALUES (@TeacherID, @Password)", connection);
                command6.Parameters.AddWithValue("@TeacherID", teacherID);
                command6.Parameters.AddWithValue("@Password", password);
                command6.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddTeacherPassword() in TeacherRepo");
                Debug.WriteLine("Error" + ex.Message);
            }
            finally
            {
            }
        }
        private void AddTeacherInstitution(Teacher teacher, int teacherID, SqlConnection connection)
        {
            try
            {
                var command7 = new SqlCommand("INSERT INTO MapInstitutionsTeachers (TeacherID, InstitutionID) VALUES (@TeacherID, @InstitutionID);", connection);
                command7.Parameters.AddWithValue("@TeacherID", teacherID);
                command7.Parameters.AddWithValue("@InstitutionID", teacher.InstitutionID);
                command7.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddTeacherInstitution() in TeacherRepo");
                Debug.WriteLine("Error" + ex.Message);
            }
            finally
            {
            }
        }

        public void ChangePassword(int id, string newPass, string oldPass)
        {
            throw new NotImplementedException();
        }

        public bool CheckPassword(int id, string pass)
        {
            throw new NotImplementedException();
        }

        public void DeleteByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<Teacher> GetAll()
        {
            var teachers = new List<Teacher>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Teachers", connection);
                    connection.Open();
                    teachers = GetTeachersByCommand(command, connection);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in TeacherRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return teachers;
        }

        public List<Teacher> GetByAdminID(int adminID)
        {
            throw new NotImplementedException();
        }

        public Teacher GetByClassID(int classID)
        {
            Teacher teacher = new();
            int teacherID = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT TeacherID FROM Classes WHERE ClassID = @ID", connection);
                    command.Parameters.AddWithValue("@ID", classID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        teacherID = (int)reader["TeacherID"];
                    }
                    teacher = GetByID(teacherID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByClassID() in TeacherRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return teacher;
        }

        public Teacher GetByID(int id)
        {
            Teacher teacher = new();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Teachers WHERE TeacherID = @ID", connection);
                    command.Parameters.AddWithValue("@ID", id);
                    connection.Open();
                    teacher = GetTeachersByCommand(command, connection)[0];
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in TeacherRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return teacher;
        }

        public List<Teacher> GetByInstitutionID(int institutionID)
        {
            throw new NotImplementedException();
        }

        public Teacher GetByTransportID(int transportID)
        {
            Teacher teacher = new();
            int teacherID = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT TeacherID FROM Transports WHERE TransportID = @ID", connection);
                    command.Parameters.AddWithValue("@ID", transportID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        teacherID = (int)reader["TeacherID"];
                    }
                    teacher = GetByID(teacherID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByTransportID() in TeacherRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return teacher;
        }

        public void Update(Teacher teacher)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("UPDATE Teachers SET Email=@Email, FirstName=@FirstName, LastName=@LastName, HasCar=@HasCar, WeeklyHours=@WeeklyHours WHERE TeacherID = @TeacherID", connection);
                    command.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
                    //command.Parameters.AddWithValue("@InstitutionID", teacher.InstitutionID);
                    command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                    //Debug.WriteLine("Firs Name: " + teacher.FirstName);
                    command.Parameters.AddWithValue("@LastName", teacher.LastName);
                    command.Parameters.AddWithValue("@Email", teacher.Email);
                    command.Parameters.AddWithValue("@WeeklyHours", teacher.WeeklyHours.TotalHours);
                    command.Parameters.AddWithValue("@HasCar", teacher.HasCar);
                    connection.Open();
                    command.ExecuteNonQuery();
                    UpdateAdminIDs(teacher.TeacherID, teacher.AdminIDs, connection);
                    UpdateTeacherInstitution(teacher, connection);
                    UpdateTeacherAddress(teacher, connection);

                }
                catch (Exception ex) 
                {
                    Debug.WriteLine("There was an error in Update() in TeacherRepo");
                    Debug.WriteLine("Error: " + ex);
                }
                finally 
                {
                    connection.Close();
                }
            }
        }

        private void UpdateAdminIDs(int teacherID, List<int> adminIDs, SqlConnection connection)
        {
            Debug.WriteLine("TeacherID: "+teacherID);
            try
            {
                var command = new SqlCommand("DELETE FROM MapAdministratorsTeachers WHERE TeacherID=@TeacherID", connection);
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                command.ExecuteNonQuery();
                
                var command2 = new SqlCommand("INSERT INTO MapAdministratorsTeachers (AdministratorID, TeacherID) VALUES (@AdminID, @TeacherID)", connection);
                command2.Parameters.AddWithValue("@TeacherID", teacherID);
                foreach (var adminID in adminIDs)
                {
                    command2.Parameters.AddWithValue("@AdminID", adminID);
                    command2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UpdateAdminIDs() in TeacherRepo");
                Debug.WriteLine("Error: " + ex);
            }
        }
        private void UpdateTeacherInstitution(Teacher teacher, SqlConnection connection)
        {
            try
            {
                var command = new SqlCommand("DELETE FROM MapInstitutionsTeachers WHERE TeacherID=@TeacherID", connection);
                command.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
                command.ExecuteNonQuery();

                var command2 = new SqlCommand("INSERT INTO MapInstitutionsTeachers (InstitutionID, TeacherID) VALUES (@InstitutionID, @TeacherID)", connection);
                command2.Parameters.AddWithValue("@InstitutionID", teacher.InstitutionID);
                command2.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
                command2.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UpdateTeacherInstitution() in TeacherRepo");
                Debug.WriteLine("Error: " + ex);
            }
        }
        private void UpdateTeacherAddress(Teacher teacher, SqlConnection connection)
        {
            try
            {
                var command = new SqlCommand("DELETE FROM TeacherAddress WHERE TeacherID=@TeacherID", connection);
                command.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
                command.ExecuteNonQuery();

                var command2 = new SqlCommand("INSERT INTO TeacherAddress (TeacherID, Region, City, PostalCode, RoadName, RoadNumber) VALUES (@TeacherID, @Region, @City, @PostalCode, @RoadName, @RoadNumber)", connection);
                command2.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
                //Debug.WriteLine("Region: " + teacher.Address.Region);
                command2.Parameters.AddWithValue("@Region", teacher.Address.Region);
                command2.Parameters.AddWithValue("@City", teacher.Address.City);
                command2.Parameters.AddWithValue("@PostalCode", teacher.Address.PostalCode);
                command2.Parameters.AddWithValue("@RoadName", teacher.Address.RoadName);
                command2.Parameters.AddWithValue("@RoadNumber", teacher.Address.RoadNumber);
                command2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UpdateTeacherAddress() in TeacherRepo");
                Debug.WriteLine("Error: " + ex);
            }
        }

        //public int GetLogIn(string Email, string Password)
        //{
        //    int teacherID = 0;
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        //var command = new SqlCommand("SELECT * FROM Administrators WHERE Email = @Email AND (select AdministratorID FROM Administrators WHERE Email = @Email) = ALL (Select AdministratorID FROM AdministratorPasswords WHERE Password = @Password)", connection);
        //        var command = new SqlCommand("SELECT TeacherID FROM Teachers WHERE Email = @Email", connection);
        //        command.Parameters.AddWithValue("@Email", Email);
        //        connection.Open();
        //        try
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {

        //                if (reader.Read())
        //                {
        //                    teacherID = (int)reader["teacherID"];
        //                }
        //                return teacherID;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine("There is a fault in TeacherRepo GetTeacherIDByEmail");
        //            Debug.WriteLine(ex);
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //    }
        //    return teacherID;
        //}

        public int GetTeacherIDByEmail(string Email)
        {
            int teacherID = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT TeacherID FROM Teachers WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", Email);
                connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            teacherID = (int)reader["TeacherID"];
                        }
                        return teacherID;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in teacherRepo GetTeacherByEmail");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return teacherID;
        }
        public string GetPasswordByEmail(string Email)
        {
            string teacherPass = "0";
            Debug.WriteLine(Email);
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT Password FROM TeacherPasswords WHERE TeacherID = (Select TeacherID FROM Teachers WHERE Email = @Email)", connection);
                command.Parameters.AddWithValue("@Email", Email);
                connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            teacherPass = (string)reader["Password"];
                        }
                        return teacherPass;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in TeacherRepo GetPasswordByEmail");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return teacherPass;
        }

        public string GetPasswordByteacherID(int teacherID)
        {
            string teacherPassword = "0";
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT Password FROM TeacherPasswords WHERE TeacherID = @TeacherID", connection);
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            teacherPassword = (string)reader["Password"];
                        }
                        return teacherPassword;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AdminRepo GetPasswordByTeacherID");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return teacherPassword;
        }

        public void UpdatePassword(int teacherID, string Password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("Update TeacherPasswords SET Password = @Password WHERE TeacherID = @TeacherID", connection);
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                command.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in TeacherRepo UpdatePassword");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
