using System.Data.SqlClient;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using MyORMLibrary;
using MyServer.services;
using Server.Models;
using TemlateEngine;

namespace Server.Endpoints;

public class FilmEndpoint : EndpointBase
{
    [Get("movie")]
    public IHttpResponseResult GetFilmPage(string movieTitle)
    {
        var localPath = "public/film.html";
        var responseText = File.ReadAllText(localPath);
        Console.WriteLine(movieTitle);

        Console.WriteLine("----- User on film-page -----");

        var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));

        // Используем метод GetByColumn
        var movie = movie_context.GetByTitle(movieTitle);

        var templateEngine = new HtmlTemplateEngine();

        // Проверка наличия куки session-token
        if (Context.Request.Cookies["session-token"] == null)
        {
            Console.WriteLine("Cookie 'session-token' not found.");
            
        }
        
        var sessionToken = Context.Request.Cookies["session-token"].Value;
        if (string.IsNullOrEmpty(sessionToken))
        {
            Console.WriteLine("Session token is empty.");
            
        }
        
        var userId = Int32.Parse(SessionStorage.GetUserId(sessionToken));
        var context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
        var user = context.GetById(userId);
        
        var model = new
        {
            movie = movie,
            user = user
        };
        
        if (!IsAuthorized(Context))
        {
            var text = templateEngine.Render(responseText, model);
            return Html(text);
        }
        
        var text_with_name = templateEngine.Render(
            responseText,
            "<button class=\"header__login-btn btn-without-bg js-show-login\">Войти</button>", "<button class=\"header__login-btn btn-without-bg js-show-login\">{{user.Login}}</button>"
        );

        var result = templateEngine.Render(text_with_name, model);
        return Html(result);
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
