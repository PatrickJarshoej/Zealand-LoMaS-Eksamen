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
        private List<Institution> GetInstitutionsByCommand(SqlCommand command)
        {
            var institutions = new List<Institution>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var institution = new Institution
                    (
                        (int)reader["InstitutionID"],
                        //we need a map reader
                        new Address((string)reader["Region"], (string)reader["City"], (int)reader["PostalCode"], (string)reader["RoadName"], (string)reader["RoadNumber"]),
                        new List<int>(),
                        new List<int>()
                    );
                    institutions.Add(institution);
                }
            }
            return (institutions);
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
            throw new NotImplementedException();
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
                    institutions = GetInstitutionsByCommand(command);
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

        public Institution GetByAdminID(int id)
        {
            throw new NotImplementedException();
        }

        public Institution GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Institution institution)
        {
            throw new NotImplementedException();
        }
    }
}
