using System.Data;
using System.Data.SqlClient;
using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;
using MyORMLibrary;
using MyServer.services;
using Server.Models;
using TemlateEngine;

namespace MyHttpServer.Endpoints;


public class MoviesEndpoint : EndpointBase
{
    [Get("movies")]
    public IHttpResponseResult GetMovies()
    {
        var localpath = "public/index.html";
        var response = File.ReadAllText(localpath);

        var templateEngine = new HtmlTemplateEngine();
        var movieContext = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var movies = movieContext.GetAll();


        var model = new MoviesDataVm
        {
            movies = movies,
        };
        CheckAuthorization(model);
        var text = templateEngine.Render(response, model);

        return Html(text);
    }
    
    private void CheckAuthorization(MoviesDataVm model)
    {
    
        model.IsAuthorized = IsAuthorized(Context); // Используем метод проверки авторизации
        if (model.IsAuthorized){
    
            var userId = Int32.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
            if (userId != 0)
            {
                var connection = new SqlConnection(AppConfig.GetInstance().ConnectionString);  
                var dbContext = new ORMContext<User>(connection);
                model.Login = dbContext.FirstOrDefault(u => u.Id == userId).Login;
            }
        }
    }
    
    public bool IsAuthorized(HttpRequestContext context)
    {
        // Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c=> c.Name == "session-token"))
        {
            var cookie = context.Request.Cookies["session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }
     
        return false;
    }
}
