using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;



namespace Zealand_LoMaS_Lib.Repo
{
    public class InstitutionRepo : IInstitutionRepo
    {
        private string _connectionString;
        public InstitutionRepo()
        {
            _connectionString = "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
        private List<Institution> GetInstitutionsByCommand(SqlCommand command, SqlConnection connection)
        {
            var institutions = new List<Institution>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var institution = new Institution
                    (
                        (int)reader["InstitutionID"],
                        new Address((string)reader["Region"], (string)reader["City"], (int)reader["PostalCode"], (string)reader["RoadName"], (string)reader["RoadNumber"]),
                        GetAdminsByInstitution((int)reader["InstitutionID"], connection),
                        GetClassesByInstitution((int)reader["InstitutionID"], connection)
                    );
                    institutions.Add(institution);
                }
            }
            return (institutions);
        }

        private List<int> GetAdminsByInstitution(int institutionID, SqlConnection connection)
        {
            List<int> admins = new();
            var command = new SqlCommand("SELECT * FROM MapInstitutionsAdministrators WHERE InstitutionID = @ID", connection);
            command.Parameters.AddWithValue("@ID", institutionID);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    admins.Add((int)reader["AdministratorID"]);
                }
            }
            return admins;
        }
        private List<int> GetClassesByInstitution(int institutionID, SqlConnection connection)
        {
            List<int> classes = new();
            var command = new SqlCommand("SELECT * FROM MapInstitutionsClasses WHERE InstitutionID = @ID", connection);
            command.Parameters.AddWithValue("@ID", institutionID);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    classes.Add((int)reader["ClassID"]);
                }
            }
            return classes;
        }

        public void Add(Institution institution)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO Institutions (Region, City, PostalCode, RoadName, RoadNumber) VALUES (@Region, @City, @PostalCode, @RoadName, @RoadNumber);", connection);
                    command.Parameters.AddWithValue("@Region", institution.Location.Region);
                    command.Parameters.AddWithValue("@City", institution.Location.City);
                    command.Parameters.AddWithValue("@PostalCode", institution.Location.PostalCode);
                    command.Parameters.AddWithValue("@RoadName", institution.Location.RoadName);
                    command.Parameters.AddWithValue("@RoadNumber", institution.Location.RoadNumber);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in Add() in InstitutionRepo");
                    Debug.WriteLine("Error" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void DeleteByID(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {

                    var command = new SqlCommand("Cascade Delete * FROM Institution WHERE InstitutionID=@InstitutionID", connection);
                    command.Parameters.AddWithValue("@InstitutionID", id);
                    connection.Open();
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in DeleteByID() in InstitutionRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }

            }
        }
        public List<Institution> GetAll()
        {
            var institutions = new List<Institution>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Institutions", connection);
                    connection.Open();
                    institutions = GetInstitutionsByCommand(command, connection);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in InstitutionRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }
            }
            return institutions;
        }
        private List<int> GetInstituteIDByAdminID(int adminID)
        {
            var institutionIDs = new List<int>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM MapInstitutionsAdministrators WHERE AdministratorID = @AdministratorID", connection);
                    command.Parameters.AddWithValue("@AdministratorID", adminID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var institutionID = 0;
                            institutionID = (int)reader["InstitutionID"];
                            institutionIDs.Add(institutionID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetInstituteIDByAdminID in InstitutionRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return institutions;
        }

        public Institution GetByAdminID(int id)
        {
            var institution = new Institution();
            int institutionID = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    
                    var command = new SqlCommand("SELECT InstitutionID FROM MapInstitutionsAdministrators WHERE AdminID=@AdminID", connection);
                    command.Parameters.AddWithValue("@AdminID", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            institutionID = (int)reader["InstitutionID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in InstitutionRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }
                institution = GetByID(institutionID);

            }
            return institution;
        }
            return institutionIDs;
        }
        private void DeleteByIDs(List<int> instituteIDs)
        {
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        var command = new SqlCommand("DELETE FROM MapInstitutionsAdministrators WHERE AdministratorID = @AdministratorID", connection);
                        connection.Open();
                        for (int i = 0; i < instituteIDs.Count; i++)
                        {
                            command.Parameters.AddWithValue("@AdministratorID", instituteIDs[i]);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error in DeleteByIDs in InstitutionRepo");
                        Debug.WriteLine($"Error: {ex}");
                    }
                    finally { connection.Close(); }
                }
            }
        }
        public void UpdateMapAdminInstitute(int adminID, List<int> newInstituteIDs)
        {
            List<int> instituteIDs = GetInstituteIDByAdminID(adminID);
            DeleteByIDs(instituteIDs);
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO MapInstitutionsAdministrators (AdministratorID, InstitutionID) VALUES (@AdministratorID, @InstitutionID)", connection);
                    command.Parameters.AddWithValue("@AdministratorID", adminID);
                    connection.Open();
                    for (int i = 0; i < newInstituteIDs.Count; i++)
                    {
                        command.Parameters.AddWithValue("@InstitutionID", newInstituteIDs[i]);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in UpdateMapAdminInstitude in InstitutionRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }




            }
        }
        public Institution GetByID(int id)
        {
            var institution = new Institution();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Institutions WHERE InstitutionID=@InstitutionID", connection);
                    command.Parameters.AddWithValue("@InstitutionID", id);
                    connection.Open();
                    institution = GetInstitutionsByCommand(command, connection)[0];
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in InstitutionRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return institution;
        }
        public void Update(Institution institution)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    Debug.WriteLine(institution.Location.Region);
                    var command = new SqlCommand("UPDATE Institutions SET Region=@Region, City=@City, PostalCode=@PostalCode, RoadName=@RoadName, RoadNumber=@RoadNumber WHERE InstitutionID = @InstitutionID", connection);
                    command.Parameters.AddWithValue("@InstitutionID", institution.InstitutionID);
                    command.Parameters.AddWithValue("@Region", institution.Location.Region);
                    command.Parameters.AddWithValue("@City", institution.Location.City);
                    command.Parameters.AddWithValue("@RoadName", institution.Location.RoadName);
                    command.Parameters.AddWithValue("@RoadNumber", institution.Location.RoadNumber);
                    command.Parameters.AddWithValue("@PostalCode", institution.Location.PostalCode);
                    connection.Open();
                    command.ExecuteNonQuery();

                    UpdateAdminIDs(institution.InstitutionID, institution.AdminIDs, connection);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error in Update() in InstitutionRepo");
                    Debug.WriteLine("Error: " + ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            //throw new NotImplementedException();
        }
        private void UpdateAdminIDs(int institutionID, List<int> adminIDs, SqlConnection connection)
        {
            try
            {
                var command = new SqlCommand("DELETE FROM MapInstitutionsAdministrators WHERE InstitutionID=@InstitutionID", connection);
                command.Parameters.AddWithValue("@InstitutionID", institutionID);
                command.ExecuteNonQuery();

                var command2 = new SqlCommand("INSERT INTO MapInstitutionsAdministrators (AdministratorID, InstitutionID) VALUES (@AdminID, @InstitutionID)", connection);
                command2.Parameters.AddWithValue("@InstitutionID", institutionID);
                SqlParameter adminParam = command2.Parameters.AddWithValue("@AdminID", null);

                foreach (var adminID in adminIDs)
                {
                    adminParam.Value = adminID;
                    command2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UpdateAdminIDs() in InstitutionRepo");
                Debug.WriteLine("Error: " + ex);
            }
        }
        public Institution GetByAdminID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
