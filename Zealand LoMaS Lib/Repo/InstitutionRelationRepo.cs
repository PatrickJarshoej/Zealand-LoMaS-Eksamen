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
        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionRelationRepo"/> class using the default database
        /// connection settings.
        /// </summary>
        /// <remarks>This constructor configures the repository to connect to the default SQL Server
        /// database. Use this constructor when you want to interact with the standard data source for institution
        /// relations.</remarks>
        public InstitutionRelationRepo() 
        {
            _connectionString = "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
        /// <summary>
        /// Retrieves a list of institution relations by executing the specified SQL command.
        /// </summary>
        /// <remarks>The caller is responsible for ensuring that the provided <paramref name="command"/>
        /// is properly configured and that its connection is open before calling this method.
        /// 
        /// Legacy note: Column names (InstituteFromID/InstituteToID) originate from the transport system
        /// where direction mattered. For institution relationships, these represent unordered pairs
        /// stored in canonical (sorted) order.</remarks>
        /// <param name="command">The <see cref="SqlCommand"/> to execute. The command must be configured to return institution relation data,
        /// including columns for InstituteFromID, InstituteToID, TransportHours, and Cost.</param>
        /// <returns>A list of <see cref="InstitutionRelation"/> objects representing the relations between institutions returned
        /// by the command. The list is empty if no relations are found.</returns>
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
        /// <summary>
        /// Adds a new institution relation to the database.
        /// </summary>
        /// <remarks>
        /// Inserts a new record using InstituteFromID/InstituteToID column names (legacy transport system naming).
        /// The InstitutionIDs are expected to be in canonical (sorted) order as they represent unordered pairs.
        /// </remarks>
        /// <param name="institutionRelation">The <see cref="InstitutionRelation"/> object to add. Must not be <c>null</c>.</param>
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
        /// <summary>
        /// Retrieves all institution relation records from the data source using <see cref="GetInstitutionsRelationsByCommand(SqlCommand)"/>
        /// </summary>
        /// <returns>A list of <see cref="InstitutionRelation"/> objects representing all institution relations. The list is
        /// empty if no records are found.</returns>
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
        /// <summary>
        /// Retrieves all <see cref="InstitutionRelation"/> records associated with the specified institution ID using <see cref="GetInstitutionsRelationsByCommand(SqlCommand)"/>.
        /// </summary>
        /// <remarks>
        /// Searches both InstituteFromID and InstituteToID columns (legacy transport system naming)
        /// since institution relationships are unordered pairs despite the directional column names.
        /// </remarks>
        /// <param name="id">The unique identifier of the institution for which to retrieve related institution relations.</param>
        /// <returns>A list of <see cref="InstitutionRelation"/> objects where the specified institution participates.
        /// Returns an empty list if no relations are found.</returns>
        public List<InstitutionRelation> GetByInstitutionID(int id)
        {
            var institutionRelation = new InstitutionRelation();
            var institutionRelations = new List<InstitutionRelation>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {

                    var command = new SqlCommand("SELECT * FROM InstitutionsRelations " +
                        "WHERE InstituteFromID=@InstituteID " +
                        "OR InstituteToID=@InstituteID", connection);
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
            return institutionRelations;
        }
        /// <summary>
        /// Retrieves the <see cref="InstitutionRelation"/> between two institutions with canonical ordering.
        /// </summary>
        /// <remarks>
        /// Queries using InstituteFromID/InstituteToID column names (legacy transport system naming).
        /// IMPORTANT: This method assumes canonical ID ordering (id1 &lt; id2). 
        /// Callers must ensure IDs are sorted before calling.
        /// </remarks>
        /// <param name="id1">The smaller institution ID (InstituteFromID in canonical order).</param>
        /// <param name="id2">The larger institution ID (InstituteToID in canonical order).</param>
        /// <returns>An <see cref="InstitutionRelation"/> object or default if not found.</returns>
        public InstitutionRelation GetByInstitutionIDs(int id1, int id2)
        {
            var institutionRelation = new InstitutionRelation();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM InstitutionsRelations " +
                        "WHERE InstituteFromID=@InstituteFromID " +
                        "AND InstituteToID=@InstituteToID", connection);
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
        /// <summary>
        /// Updates the cost and transport hours for an existing institution relation in the database.
        /// </summary>
        /// <remarks>
        /// Uses InstituteFromID/InstituteToID column names (legacy transport system naming).
        /// Assumes InstitutionIDs are in canonical order (ensured by the create function in the service layer).
        /// </remarks>
        /// <param name="institutionRelation">The <see cref="InstitutionRelation"/> object containing updated values.</param>
        public void Update(InstitutionRelation institutionRelation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("UPDATE InstitutionsRelations SET Cost=@Cost, TransportHours=@Time " +
                        "WHERE InstituteFromID = @InstitutionFromID AND InstituteToID = @InstitutionToID ", connection);
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
