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
                    Debug.WriteLine("Hello, World!");
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
            Debug.WriteLine("Get InstitutionID!");
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
            Debug.WriteLine("Get Address!");

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
        public void Add(Teacher teacher)
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
                    AddTeacherPassword(teacher, teacherID, connection);
                    //AddTeacherClasses(teacher, teacherID, connection);
                    //AddTeacherCompetencies(teacher, teacherID, connection);
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
                Debug.WriteLine("Error"+ ex.Message);
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

        private void AddTeacherPassword(Teacher teacher, int teacherID, SqlConnection connection)
        {
            try
            {
                string password = "teacherDefault";
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
            throw new NotImplementedException();
        }

        public Teacher GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<Teacher> GetByInstitutionID(int institutionID)
        {
            throw new NotImplementedException();
        }

        public Teacher GetByTransportID(int transportID)
        {
            throw new NotImplementedException();
        }

        public void Update(Teacher t)
        {
            throw new NotImplementedException();
        }

        public bool CheckLogIn(string Email, string Password)
        {
            Console.WriteLine("Repo");
            bool TeacherIsLoginCorrect = false;
            using (var connection = new SqlConnection(_connectionString))
            {
                Console.WriteLine("Repo1");
                //var command = new SqlCommand("SELECT * FROM Administrators WHERE Email = @Email AND (select AdministratorID FROM Administrators WHERE Email = @Email) = ALL (Select AdministratorID FROM AdministratorPasswords WHERE Password = @Password)", connection);
                var command = new SqlCommand("SELECT * from Teachers WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", Email);
                //command.Parameters.AddWithValue("@Password", Password);
                //var command2 = new SqlCommand("SELECT * FROM AdministratorPassword WHERE Password = @Password and AdministratorID = @AdministratorID", connection);
                var command2 = new SqlCommand("SELECT TeacherID from TeacherPasswords WHERE Password = @Password and TeacherID = @TeacherID", connection);
                command2.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                try
                {

                    Console.WriteLine(Email);
                    Console.WriteLine(Password);
                    using (var reader = command.ExecuteReader())
                    {

                        int teacherID = 0;
                        if (reader.Read())
                        {
                            teacherID = (int)reader["TeacherID"];
                        }
                        //AdminIsLoginCorrect = true;
                        //var admin = new Admin((int)reader["AdministratorID"]);
                        Console.WriteLine("hep1");
                        Console.WriteLine(teacherID);
                        command2.Parameters.AddWithValue("@TeacherID", teacherID);
                        using (var reader2 = command2.ExecuteReader())
                        {
                            string Passw = "no Password";
                            if (reader.Read())
                            {
                                Passw = (string)reader["Password"];
                            }
                            if (Passw != "no Password")
                            {
                                TeacherIsLoginCorrect = true;

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in TeacherRepo CheckLogIn");
                    Debug.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            return TeacherIsLoginCorrect;
        }
    }
}
