using System.Data.SqlClient;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;
using Server.Models;

namespace MyServer.Endpoints;

public class UserEndpoints : EndpointBase
{
    [Get("users")]
    public IHttpResponseResult GetUsers()
    {
        var users =new List<User>();
        string connectionString = @"Data Source = db;Initial Catalog=PersonDB;User ID=sa;Password=MidnightProductor007;";
 
        string sqlExpression = "SELECT * FROM Users";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
 
            if(reader.HasRows) // если есть данные
            {
                // выводим названия столбцов
                
 
                while (reader.Read()) // построчно считываем данные
                {
                    var user = new User()
                    {
                        Id = reader.GetInt32(0),
                        Login = (string)reader.GetValue(1),
                        Password = (string)reader.GetValue(2),
                        Email = (string)reader.GetValue(3)
                    };

                    users.Add(user);
                }
            }
         
            reader.Close();
        }
        
        return Json(users);
    }
    
}