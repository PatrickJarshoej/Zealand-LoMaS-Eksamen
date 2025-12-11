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
        public TransportRepo() 
        {
            _connectionString= "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
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

        public void DeleteByID(int transportID)
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

        public List<Transport> GetAll()
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
                finally { connection.Close(); }


            }
            return transports;
        }

        public List<Transport> GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }
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
                    var command = new SqlCommand("UPDATE Transports SET TransportCost = @cost WHERE TransportID = @TransportID  ", connection);
                    command.Parameters.AddWithValue("@cost", transport.TransportCost);
                    command.Parameters.AddWithValue("@TransportID", transport.TransportID);
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
