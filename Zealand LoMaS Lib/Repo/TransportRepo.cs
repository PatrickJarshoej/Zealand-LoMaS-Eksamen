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
            _connectionString= string.Empty;
        }
        public void Add(Transport transport)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO Transport(TeacherID) VALUES (@TeacherID)", connection);
                    command.Parameters.AddWithValue("@TeacherID", transport.TeacherID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in Add() in DomicileRepo");
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
            throw new NotImplementedException();
        }

        public List<Transport> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Transport> GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Transport GetByID(int transportID)
        {
            throw new NotImplementedException();
        }

        public List<Transport> GetByInstitutionFromID(int institutionID)
        {
            throw new NotImplementedException();
        }

        public List<Transport> GetByInstitutionToID(int institutionID)
        {
            throw new NotImplementedException();
        }

        public List<Transport> GetByTeacherID(int teacherID)
        {
            throw new NotImplementedException();
        }

        public void Update(Transport transport)
        {
            throw new NotImplementedException();
        }
    }
}
