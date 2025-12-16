using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;
using Microsoft.Data.SqlClient;

namespace Zealand_LoMaS_Lib.Repo
{
    public class TransportRepo : ITransportRepo
    {
        private string _connectionString;
        /// <summary>
        /// Initializes a new instance of the <see cref="TransportRepo"/> class using the default database connection
        /// settings.
        /// </summary>
        /// <remarks>The repository is configured to connect to the default SQL Server database using a
        /// predefined connection string. Use this constructor when you want to interact with the default data
        /// source.</remarks>
        public TransportRepo() 
        {
            _connectionString= "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
        /// <summary>
        /// Adds a new transport record to the database using the specified transport details.
        /// </summary>
        /// <remarks>The method inserts the transport information, including teacher, hours, cost, date,
        /// and institute IDs, into the underlying data store.</remarks>
        /// <param name="transport">The <see cref="Transport"/> object containing the details of the transport to add. Must not be <c>null</c>.</param>
        public void Add(Transport transport)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO " +
                        "Transports(TeacherID, TransportHours, TransportCost , Date, InstituteFromID, InstituteToID) " +
                        "VALUES (@TeacherID, @TransportHours, @TransportCost , @Date, @InstituteFromID, @InstituteToID)", connection);
                    command.Parameters.AddWithValue("@TeacherID", transport.TeacherID);
                    command.Parameters.AddWithValue("@TransportHours", transport.TransportHours);
                    command.Parameters.AddWithValue("@TransportCost", transport.TransportCost);
                    command.Parameters.AddWithValue("@Date", transport.TheDate);
                    command.Parameters.AddWithValue("@InstituteFromID", transport.InstitueFromID);
                    command.Parameters.AddWithValue("@InstituteToID", transport.InstitueToID);
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
        /// Deletes the transport record with the specified identifier from the data store.
        /// </summary>
        /// <remarks>If no record with the specified <paramref name="transportID"/> exists, the method
        /// completes without error and no records are affected.</remarks>
        /// <param name="transportID">The unique identifier of the transport record to delete.</param>
        public void DeleteByID(int transportID)
        {
            var transports = new List<Transport>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Transports", connection);
                    connection.Open();
                    transports = GetTransportsByCommand(command);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in TransportRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// Retrieves all transport records from the data source using <see cref="GetTransportsByCommand(SqlCommand)"/>.
        /// </summary>
        /// <returns>A list of <see cref="Transport"/> objects representing all transports. The list is empty if no transport
        /// records are found.</returns>
        public List<Transport> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("DELETE FROM Transports WHERE TransportID = @ID", connection);
                    command.Parameters.AddWithValue("@ID", transportID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error: in DeleteBy in TransportRepo");
                    Debug.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Retrieves a list of transports by executing the specified SQL command.
        /// </summary>
        /// <remarks>The caller is responsible for ensuring that the provided <paramref name="command"/>
        /// is properly configured and that its connection is open before calling this method.</remarks>
        /// <param name="command">The <see cref="SqlCommand"/> to execute. The command must be configured to return transport data,
        /// including columns for TeacherID, Date, InstituteFromID, InstituteToID, TransportHours, TransportCost and TransportID.</param>
        /// <returns>A list of <see cref="Transport"/> objects representing transports returned
        /// by the command. The list is empty if no transports are found.</returns>
        private List<Transport> GetTransportsByCommand(SqlCommand command) 
        {
            var transports = new List<Transport>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var transport = new Transport
                    (
                        (int)reader["TeacherID"],
                        (DateTime)reader["Date"],
                        (int)reader["InstituteFromID"],
                        (int)reader["InstituteToID"],
                        (TimeSpan)reader["TransportHours"],
                        Decimal.ToDouble((decimal)reader["TransportCost"]),
                        (int)reader["TransportID"]
                    );
                    transports.Add(transport);
                }
            }
            return( transports );
        }
        /// <summary>
        /// Retrieves the <see cref="Transport"/> Object associated with the specified transportID using <see cref="GetTransportsByCommand(SqlCommand)"/>.
        /// </summary>
        /// <param name="transportID">The unique identifier of the Transport for which to retrieve related data, to that transport.</param>
        /// <returns>A <see cref="Transport"/> object. Returns a default object if no transport object is found</returns>
        public Transport GetByID(int transportID)
        {
            Transport transport = new Transport();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Transports WHERE TransportID=@TransportID", connection);
                    command.Parameters.AddWithValue("@TransportID", transportID);
                    connection.Open();
                    transport=GetTransportsByCommand(command)[0];
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetByID() in TransportRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return transport;
        }


        public List<Transport> GetByInstitutionFromID(int institutionID)
        {
            var transports = new List<Transport>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Transports WHERE InstitutionFromID=@institutionID", connection);
                    command.Parameters.AddWithValue("@institutionID", institutionID);
                    connection.Open();
                    transports = GetTransportsByCommand(command);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in TransportRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return transports;
        }

        public List<Transport> GetByInstitutionToID(int institutionID)
        {
            var transports = new List<Transport>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Transports WHERE InstitutionToID=@institutionID", connection);
                    command.Parameters.AddWithValue("@institutionID", institutionID);
                    connection.Open();
                    transports = GetTransportsByCommand(command);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in TransportRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return transports;
        }

        public List<Transport> GetByTeacherID(int teacherID)
        {
            var transports = new List<Transport>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Transports WHERE TeacherID=@TeacherID", connection);
                    command.Parameters.AddWithValue("@TeacherID", teacherID);
                    connection.Open();
                    transports = GetTransportsByCommand(command);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in GetAll() in TransportRepo");
                    Debug.WriteLine($"Error: {ex}");
                }
                finally { connection.Close(); }


            }
            return transports;
        }

        public void Update(Transport transport)
        {
            Debug.WriteLine("This is the ID of the transport you tried to update:"+transport.TransportID);
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("UPDATE Transports SET TransportCost = @cost, Date=@date, TransportHours=@time WHERE TransportID = @TransportID  ", connection);
                    command.Parameters.AddWithValue("@cost", transport.TransportCost);
                    command.Parameters.AddWithValue("@TransportID", transport.TransportID);
                    command.Parameters.AddWithValue("@Date", transport.TheDate);
                    command.Parameters.AddWithValue("@time", transport.TransportHours);
                    connection.Open();
                    command.ExecuteNonQuery();
                    Debug.WriteLine("it runs the try");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in Update() in TransportRepo");
                    Debug.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
