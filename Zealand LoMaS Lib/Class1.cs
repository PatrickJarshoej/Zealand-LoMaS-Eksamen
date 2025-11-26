//using Microsoft.Data.SqlClient;

//namespace Zealand_LoMaS_Lib
//{
//    public class Class1
//    {
//        var command = new SqlCommand("SELECT * from Administrators WHERE Email = @Email", connection);
//        command.Parameters.AddWithValue("@Email", Email);
//                var command2 = new SqlCommand("SELECT AdministratorID from AdministratorPasswords WHERE Password = @Password and AdministratorID = @AdministratorID", connection);
//        command2.Parameters.AddWithValue("@Password", Password);
//                connection.Open();
//                try
//                {

//                    Console.WriteLine(Email);
//                    Console.WriteLine(Password);
//                    using (var reader = command.ExecuteReader())
//                    {
//                        int AdminID = 0;
                            
//                        if (reader.Read())
//                        {
//                            AdminID = (int) reader["AdministratorID"];
//    }
//}
//}
