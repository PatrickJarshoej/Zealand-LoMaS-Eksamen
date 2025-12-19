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
        /// <summary>
        /// Retrieves a list of teachers based on the specified SQL command.
        /// </summary>
        /// <remarks>The method executes the provided SQL command and reads the resulting data to
        /// construct a list of  <see cref="Teacher"/> objects
        /// to populate  the <see cref="Teacher"/> object </remarks>
        /// <param name="command">The <see cref="SqlCommand"/> to execute for retrieving teacher data </param>
        /// <param name="connection">The <see cref="SqlConnection"/> used to execute the command and retrieve related data </param>
        /// <returns>A list of <see cref="Teacher"/> objects populated with data retrieved from the database </returns>
        private List<Teacher> GetTeachersByCommand(SqlCommand command, SqlConnection connection)
        {//This is a single method that all our get methods can use, saves a lot of code
            var teachers = new List<Teacher>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Teacher teacher = new Teacher
                    (
                        (int)reader["TeacherID"],
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
        /// <summary>
        /// Retrieves the address of a teacher based on the specified teacher ID.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="connection"/> parameter is an open and valid SQL
        /// connection before calling this method. The method does not handle closing the connection.</remarks>
        /// <param name="id">The unique identifier of the teacher whose address is to be retrieved.</param>
        /// <param name="connection">An open <see cref="SqlConnection"/> used to execute the database query.</param>
        /// <returns>An <see cref="Address"/> object containing the teacher's address details if found; otherwise, an empty object.</returns>
        private Address GetAddress(int id, SqlConnection connection)
        {
            Address address = new();
            try
            {
                var command = new SqlCommand("SELECT * FROM TeacherAddress WHERE TeacherID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
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
            return address;
        }
        /// <summary>
        /// Retrieves a list of administrator IDs associated with the specified teacher ID
        /// </summary>
        /// <remarks>Ensure that the <paramref name="connection"/> is properly opened before calling this
        /// method</remarks>
        /// <param name="id">The unique identifier of the teacher whose associated administrators are to be retrieved.</param>
        /// <param name="connection">An open <see cref="SqlConnection"/> used to execute the database query.</param>
        /// <returns>A list of integers representing the IDs of administrators associated with the specified teacher or eturns an
        /// empty list if no administrators are found.</returns>
        private List<int> GetAdmins(int id, SqlConnection connection)
        {
            List<int> adminIDs = new List<int>();
            try
            {
                var command = new SqlCommand("SELECT * FROM MapAdministratorsTeachers WHERE TeacherID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                //connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        adminIDs.Add((int)reader["AdministratorID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAdmin() in TeacherRepo");
                Debug.WriteLine($"Error: {ex.Message}");
            }
            return adminIDs;
        }
        /// <summary>
        /// Adds a new teacher to the database along with their associated details, such as address, administrators,
        /// institution, and password
        /// </summary>
        /// <remarks>This method inserts the teacher's basic information into the database and retrieves
        /// the generated teacher ID. The ID is then used to insert related data, such as the teacher's address,
        /// administrators, institution, and password, into their respective tables. The method ensures that all related
        /// data is added in a single operation</remarks>
        /// <param name="teacher">The <see cref="Teacher"/> object containing the teacher's details to be added</param>
        /// <param name="password">The password to associate with the teacher for authentication purposes</param>
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
                    //Since the database generates the id and we need to use it as a foreign key
                    //We use ExecuteScalar() to pull out the ID element from the inputted row
                    AddTeacherAddress(teacher, teacherID, connection); //Then we use this ID as our foreign key
                    AddTeacherAdmins(teacher, teacherID, connection);
                    AddTeacherInstitution(teacher, teacherID, connection);
                    AddTeacherPassword(teacherID, password, connection);
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
        /// <summary>
        /// Inserts the address information of a teacher into the database
        /// </summary>
        /// <remarks>This method assumes that the <paramref name="connection"/> is already open and valid
        /// The address details are retrieved from the <see cref="Teacher.Address"/> property and inserted into the
        /// "TeacherAddress" table </remarks>
        /// <param name="teacher">The <see cref="Teacher"/> object containing the address details to be added </param>
        /// <param name="tID">The unique identifier of the teacher, which is used as a foreign key in the database </param>
        /// <param name="connection">An open <see cref="SqlConnection"/> to the database where the address will be inserted </param>
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
        }
        /// <summary>
        /// Associates the specified teacher with a list of administrators in the database
        /// </summary>
        /// <remarks>This method inserts into the "MapAdministratorsTeachers" table, mapping the
        /// specified teacher to each administrator in the <see cref="Teacher.AdminIDs"/> collection </remarks>
        /// <param name="teacher">The <see cref="Teacher"/> object containing the list of administrator IDs to associate with the teacher </param>
        /// <param name="tID">The unique identifier of the teacher to be associated with the administrators </param>
        /// <param name="connection">An open <see cref="SqlConnection"/> to the database where the admins will be inserted </param>
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
        }

        /// <summary>
        /// Associates a teacher with multiple classes by inserting into the MapTeacherClasses table. This method is
        /// not currently in use, it was originally assumed to be used when creating a teacher but after closer consideration
        /// we decided it would be seperate. However as the method still theoretically functions we kept is so i can be used
        /// once <see cref="AClass"/> has been fully implemented
        /// </summary>
        /// <remarks>This method inserts for each class ID in the teacher's <see cref="Teacher.ClassIDs"/> collection
        /// into the MapTeacherClasses table, associating the teacher with the
        /// specified classes </remarks>
        /// <param name="teacher">The teacher object containing the list of class IDs to associate </param>
        /// <param name="tID">The unique identifier of the teacher to be associated with the classes </param>
        /// <param name="connection">An open <see cref="SqlConnection"/> to the database where the classes will be inserted </param>
        private void AddTeacherClasses(Teacher teacher, int tID, SqlConnection connection)
        {   /*
                This was created along with creater teacher but is no longer in use as 
                adding classes to a teacher should be done after creating the teacher, 
                not at the same time, but we never got around to doing this as it wasn't a priority
             */
            
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

        /// <summary>
        /// Adds the competencies of a teacher to the database by inserting them into the MapCompetenciesTeachers table. This method is
        /// not currently in use, it was originally assumed to be used when creating a teacher but after closer consideration
        /// we decided it should be seperate. However as the method still theoretically functions we kept is so i can be used
        /// once <see cref="Competency"/> has been fully implemented
        /// </summary>
        /// <remarks>This method iterates through the competencies of the specified teacher and inserts
        /// each competency into the database
        /// <param name="teacher">The <see cref="Teacher"/> object containing the competencies to be added</param>
        /// <param name="tID">The unique identifier of the teacher to associate with the competencies</param>
        /// <param name="connection">An open <see cref="SqlConnection"/> used to execute the database commands</param>
        private void AddTeacherCompetencies(Teacher teacher, int tID, SqlConnection connection)
        {   //Same as the method above
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

        /// <summary>
        /// Adds a password for the specified teacher to the database.
        /// </summary>
        /// <remarks>This method inserts into the "TeacherPasswords" table with the specified
        /// teacher ID and password </remarks>
        /// <param name="teacherID">The unique identifier of the teacher for whom the password is being added. Must correspond to an existing
        /// teacher in the database </param>
        /// <param name="password">The password to associate with the teacher </param>
        /// <param name="connection">An open <see cref="SqlConnection"/> to the database where the password will be inserted </param>
        private void AddTeacherPassword(int teacherID, string password, SqlConnection connection)
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
        }

        /// <summary>
        /// Associates a teacher with an institution by inserting a mapping record into the database.
        /// </summary>
        /// <remarks>This method inserts a record into the "MapInstitutionsTeachers" table, linking the
        /// specified teacher to an institution </remarks>
        /// <param name="teacher">The <see cref="Teacher"/> object containing the institution information to associate with the teacher.</param>
        /// <param name="teacherID">The unique identifier of the teacher to be associated with the institution.</param>
        /// <param name="connection">An open <see cref="SqlConnection"/> to the database where the map is located </param>
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
        }

        /// <summary>
        /// Deletes a teacher from the database based on the specified teacherID.
        /// </summary>
        /// <remarks>This method executes a SQL DELETE command to remove the teacher record with the
        /// specified ID from the "Teachers" table. If the specified ID does not exist in the database, nothing will
        /// be deleted, and no exception will be thrown. The database uses On Delete Cascade so we only need to delete from this one table </remarks>
        /// <param name="id">The unique identifier of the teacher to delete </param>
        public void DeleteByID(int id)
        {
            //Debug.WriteLine(id);
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("Delete FROM Teachers WHERE TeacherID=@TeacherID", connection);
                    command.Parameters.AddWithValue("@TeacherID", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in DeleteByID() in TeacherRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }
            }
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
        /// <summary>
        /// Retrieves a teacher by their unique identifier using GetTeachersByCommand() 
        /// </summary>
        /// <param name="id">The unique identifier of the teacher to retrieve.</param>
        /// <returns>A <see cref="Teacher"/> object representing the teacher with the specified identifier. If no teacher is
        /// found, the returned object may be uninitialized or contain default values.</returns>
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
                    //Here we use the GetTeachersByCommand, but it returns a list so we only grab the first index
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

        /// <summary>
        /// This method updates all the properties of a teacher object
        /// </summary>
        /// <remarks> This method also updates all of the mapped values such as admins and institution 
        /// the <see cref="Teacher.WeeklyHours"/> are stored as a decimal(10,2) in the database so we need to convert it into
        /// a type of floating point number, in this case a double
        /// </remarks>
        /// <param name="teacher">This is the edited teacher</param>
        public void Update(Teacher teacher)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("UPDATE Teachers SET Email=@Email, FirstName=@FirstName, LastName=@LastName, HasCar=@HasCar, WeeklyHours=@WeeklyHours WHERE TeacherID = @TeacherID", connection);
                    command.Parameters.AddWithValue("@TeacherID", teacher.TeacherID);
                    command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
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

        /// <summary>
        /// This method updates all associated admins
        /// </summary>
        /// <remarks> When updating Maps we decided it was easier and quicker to just delete all the existing elements 
        /// for the user and the recreate them based of the list sent down </remarks>
        /// <param name="teacherID"> The unique identifier of the teacher to update </param>
        /// <param name="adminIDs"> The list of administrators to add</param>
        /// <param name="connection"> An open <see cref="SqlConnection"/> to the database where the map is located </param>
        private void UpdateAdminIDs(int teacherID, List<int> adminIDs, SqlConnection connection)
        {
            Debug.WriteLine("TeacherID: "+teacherID);
            try
            {   
                var command = new SqlCommand("DELETE FROM MapAdministratorsTeachers WHERE TeacherID=@TeacherID", connection);
                command.Parameters.AddWithValue("@TeacherID", teacherID);
                command.ExecuteNonQuery();
                
                foreach (var adminID in adminIDs)
                {
                    var command2 = new SqlCommand("INSERT INTO MapAdministratorsTeachers (AdministratorID, TeacherID) VALUES (@AdminID, @TeacherID)", connection);
                    command2.Parameters.AddWithValue("@TeacherID", teacherID);
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

        /// <summary>
        /// This method updates the associated institution
        /// </summary>
        /// <remarks> When updating Maps we decided it was easier and quicker to just delete all the existing elements 
        /// for the user and the recreate them based of the value sent down </remarks>
        /// <param name="teacher"> The teacher to update </param>
        /// <param name="connection"> An open <see cref="SqlConnection"/> to the database where the map is located </param>
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

        /// <summary>
        /// This method updates the associated address
        /// </summary>
        /// <remarks> When updating Maps we decided it was easier and quicker to just delete all the existing elements 
        /// for the user and the recreate them based of the value sent down </remarks>
        /// <param name="teacher"> The teacher to update </param>
        /// <param name="connection"> An open <see cref="SqlConnection"/> to the database where the map is located </param>
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
                Debug.WriteLine("Running UpdatePassword in TeacherRepo");
                Debug.WriteLine("TeacherID: " + teacherID);
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
