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
    public class InstitutionRelationRepo : IInstitutionRelationRepo
    {
        private string _connectionString;
        public InstitutionRelationRepo() 
        {
            _connectionString = "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
        private List<InstitutionRelation> GetInstitutionsRelationsByCommand(SqlCommand command)
        {
            var institutionRelations = new List<InstitutionRelation>();
            List<int> institutionIDs= new List<int>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    institutionIDs.Add((int)reader["InstituteFromID"]);
                    institutionIDs.Add((int)reader["InstituteToID"]);
                    var relation = new InstitutionRelation(
                        (TimeSpan)reader["TransportHours"],
                        (double)reader["Cost"],
                        institutionIDs
                        );
                    institutionRelations.Add(relation);
                }
            }
            return (institutionRelations);
        }
        public void Add(InstitutionRelation institutionRelation)
        {
            throw new NotImplementedException();
        }

        public void DeleteByID(int institutionid)
        {
            throw new NotImplementedException();
        }

        public List<InstitutionRelation> GetAll()
        {
            var institutionRelations = new List<InstitutionRelation>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM InstitutionsRelations", connection);
                    connection.Open();
                    institutionRelations = GetInstitutionsRelationsByCommand(command);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in InstitutionRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return institutionRelations;
        }
        public List<InstitutionRelation> GetByInstitutionID(int id)
        {
            var institutionRelation = new InstitutionRelation();
            var institutionRelations = new List<InstitutionRelation>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {

                    var command = new SqlCommand("SELECT * FROM InstitutionsRelations WHERE InstituteFromID=@InstituteFromID", connection);
                    command.Parameters.AddWithValue("@InstituteFromID", id);
                    connection.Open();
                    institutionRelations.Add(GetInstitutionsRelationsByCommand(command)[0]);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in InstitutionRelationRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {

                    var command = new SqlCommand("SELECT * FROM InstitutionsRelations WHERE InstituteFromID=@InstituteToID", connection);
                    command.Parameters.AddWithValue("@InstituteFromID", id);
                    connection.Open();
                    institutionRelations.Add(GetInstitutionsRelationsByCommand(command)[0]);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in InstitutionRelationRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return institutionRelations;
        }
        public InstitutionRelation GetByInstitutionIDs(int id1, int id2)
        {
            var institutionRelation = new InstitutionRelation();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM InstitutionsRelations WHERE InstituteFromID=@InstituteFromID AND InstituteToID=@InstitutetoID", connection);
                    command.Parameters.AddWithValue("@InstituteFromID", id1);
                    command.Parameters.AddWithValue("@InstituteToID", id2);
                    connection.Open();
                    institutionRelation = GetInstitutionsRelationsByCommand(command)[0];
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in InstitutionRelationRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return institutionRelation;
        }

        public void Update(InstitutionRelation institutionRelation)
        {
            throw new NotImplementedException();
        }
    }
}
