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
            var relation = new InstitutionRelation();
            var institutionRelations = new List<InstitutionRelation>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    List<int> institutionIDs = new List<int>();
                    institutionIDs.Add((int)reader["InstituteFromID"]);
                    institutionIDs.Add((int)reader["InstituteToID"]);
                    relation = new InstitutionRelation(
                        (TimeSpan)reader["TransportHours"],
                        Decimal.ToDouble((decimal)reader["Cost"]),
                        institutionIDs
                        );
                    institutionRelations.Add(relation);
                }
            }
            return (institutionRelations);
        }
        public void Add(InstitutionRelation institutionRelation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO " +
                        "InstitutionsRelations(InstituteFromID, InstituteToID, Cost , TransportHours) " +
                        "VALUES (@InstituteFromID, @InstituteToID, @Cost , @TransportHours)", connection);
                    command.Parameters.AddWithValue("@InstituteFromID", institutionRelation.InstitutionIDs[0]);
                    command.Parameters.AddWithValue("@InstituteToID", institutionRelation.InstitutionIDs[1]);
                    command.Parameters.AddWithValue("@Cost", institutionRelation.Cost);
                    command.Parameters.AddWithValue("@TransportHours", institutionRelation.Time);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in Add() in TransportRepo");
                    Debug.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
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

                    var command = new SqlCommand("SELECT * FROM InstitutionsRelations WHERE InstituteFromID=@InstituteID or InstituteToID=@InstituteID", connection);
                    command.Parameters.AddWithValue("@InstituteID", id);
                    connection.Open();
                    var newRelations = GetInstitutionsRelationsByCommand(command);
                    foreach (var r in newRelations) { institutionRelations.Add(r); }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in InstitutionRelationRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
          
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
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("UPDATE InstitutionsRelations SET Cost=@Cost, TransportHours=@Time WHERE InstituteFromID = @InstitutionFromID AND InstituteToID = @InstitutionToID ", connection);
                    command.Parameters.AddWithValue("@InstitutionFromID", institutionRelation.InstitutionIDs[0]);
                    command.Parameters.AddWithValue("@InstitutionToID", institutionRelation.InstitutionIDs[1]);
                    command.Parameters.AddWithValue("@Cost", institutionRelation.Cost);
                    command.Parameters.AddWithValue("@Time", institutionRelation.Time);
                    connection.Open();
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error in Update() in InstitutionRelationRepo");
                    Debug.WriteLine("Error: " + ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
