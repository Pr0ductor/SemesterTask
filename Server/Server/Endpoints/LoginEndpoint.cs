using System.Data.SqlClient;
using System.Net;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using MyORMLibrary;
using MyServer.services;
using Server.Models;

namespace Server.Endpoints;

public class LoginEndpoint : EndpointBase
{

    [Post("login")]
    public IHttpResponseResult Login(string login,  string password)
    {
        var connection = new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]);  
        var dbContext = new ORMContext<User>(connection);
        var check = dbContext.FirstOrDefault(u => u.Login == login && u.Password == password);
        if (check != null)
        {
            var token = Guid.NewGuid().ToString();
            Cookie nameCookie = new Cookie("session-token", token);
            nameCookie.Path = "/";

            Context.Response.Cookies.Add(nameCookie);
            SessionStorage.SaveSession(token, check.Id.ToString());
            return Redirect("movies");
        }
        return Html("error");
    }
}