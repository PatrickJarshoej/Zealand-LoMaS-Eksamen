using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Zealand_LoMaS_Lib.Model;
using Zealand_LoMaS_Lib.Repo.Interfaces;

namespace Zealand_LoMaS_Lib.Repo
{
    public class AClassRepo : IAClassRepo
    {
        private string _connectionString;
        public AClassRepo()
        {
            _connectionString = "Data Source=mssql8.unoeuro.com;User ID=stackoverflowed_dk;Password=mH629G5hFzaktn34pBEw;Encrypt=False; Database=stackoverflowed_dk_db_zealand_lomas; Command Timeout=30;MultipleActiveResultSets=true;";
        }
        public void Add(AClass aClassObject)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    var command = new SqlCommand("INSERT INTO Classes (StartTime, Duration, Description, Subject) OUTPUT Inserted.ClassID VALUES (@StartTIme, @Duration, @Description, @Subject)", connection);
                    command.Parameters.AddWithValue("@StartTIme", aClassObject.ClassStart);
                    command.Parameters.AddWithValue("@Duration", aClassObject.Duration);
                    command.Parameters.AddWithValue("@Description", aClassObject.ClassDesciption);
                    command.Parameters.AddWithValue("@Subject", aClassObject.ClassSubject);
                    int ClassID = (int)command.ExecuteScalar();

                    var command2 = new SqlCommand("INSERT INTO MapAdministratorsClasses (AdministratorID, ClassID) VALUES (@AdministratorID, @ClassID)", connection);
                    command2.Parameters.AddWithValue("@AdministratorID", aClassObject.AdministratorID);
                    command2.Parameters.AddWithValue("@ClassID", ClassID);
                    command2.ExecuteNonQuery();

                    var command3 = new SqlCommand("INSERT INTO MapInstitutionsClasses (InstitutionID, ClassID) VALUES (@InstitutionID, @ClassID)", connection);
                    command3.Parameters.AddWithValue("@InstitutionID", aClassObject.InstitutionID);
                    command3.Parameters.AddWithValue("@ClassID", ClassID);
                    command3.ExecuteNonQuery();

                    var command4 = new SqlCommand("INSERT INTO MapTeachersClasses (TeacherID, ClassID) VALUES (@TeacherID, @ClassID)", connection);
                    command4.Parameters.AddWithValue("@TeacherID", aClassObject.TeacherID);
                    command4.Parameters.AddWithValue("@ClassID", ClassID);
                    command4.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There is a fault in AClassRepo Create");
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
