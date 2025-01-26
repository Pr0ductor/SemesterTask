

using System.Data.SqlClient;
using System.Net;
using HttpServerLibrary.Attributes;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using MyORMLibrary;
using MyServer.services;
using Server.Helpers;
using Server.Models;
using TemlateEngine;

namespace MyHttpServer.Endpoints;

/// <summary>
/// Представляет конечную точку для административных операций.
/// </summary>
public class AdminEndpoint : EndpointBase
{
    /// <summary>
    /// Получает или задает помощника ответа.
    /// </summary>
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();

    /// <summary>
    /// Обрабатывает GET-запрос для страницы входа администратора.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("admlogin")]
    public IHttpResponseResult Get()
    {
        var localPath = "admin_login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        return Html(responseText);
    }

    /// <summary>
    /// Обрабатывает POST-запрос для входа администратора.
    /// </summary>
    /// <param name="login">Логин администратора.</param>
    /// <param name="password">Пароль администратора.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admlogin")]
    public IHttpResponseResult Login(string login, string password)
    {
        var localPath = "admin_login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        var templateEngine = new HtmlTemplateEngine();
        var admin_context = new ORMContext<Admin>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));

        // Логирование входных данных
        Console.WriteLine("----- User on admin-login-page -----");
        Console.WriteLine($"Login attempt with login: {login} and password: {password}");

        var admin = admin_context.FirstOrDefault(x => x.Login == login && x.Password == password);

        // Логирование результата запроса
        if (admin == null)
        {
            Console.WriteLine("admin not found in the database.");
            var result = templateEngine.Render(responseText, "<!--ERROR: ADMIN DOESN'T EXIST-->", "<p class=\"error_message\">Такого администратора не существует</p>");
            return Html(result);
        }

        Console.WriteLine($"admin found: {admin.Login}, {admin.Password}");

        var token = Guid.NewGuid().ToString();
        Cookie nameCookie = new Cookie("admin-session-token", token);
        nameCookie.Path = "/";
        Context.Response.Cookies.Add(nameCookie);
        SessionStorage.SaveSession(token, admin.Id.ToString());

        Console.WriteLine("admin found and redirecting to index");
        return Redirect("/admin");
    }

    /// <summary>
    /// Обрабатывает GET-запрос для административной страницы.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("admin")]
    public IHttpResponseResult GetPage()
    {
        var localPath = "admin.html";
        var responseText = ResponseHelper.GetResponseText(localPath);

        Console.WriteLine("----- admin on admin-page -----");

        var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
        var movies = movie_context.GetAll();

        var user_context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
        var users = user_context.GetAll();

        var admin_context = new ORMContext<Admin>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
        var admins = admin_context.GetAll();

        var templateEngine = new HtmlTemplateEngine();
        var model = new
        {
            movies = movies,
            admins = admins,
            users = users,
        };

        if (!IsAuthorized(Context))
        {
            return Redirect("/admlogin");
        }

        var text = templateEngine.Render(responseText, model);
        return Html(text);
    }

    /// <summary>
    /// Проверяет, авторизован ли пользователь.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>True, если пользователь авторизован; иначе false.</returns>
    public bool IsAuthorized(HttpRequestContext context)
    {
        // Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c => c.Name == "admin-session-token"))
        {
            var cookie = context.Request.Cookies["admin-session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }

        return false;
    }

    /// <summary>
    /// Обрабатывает POST-запрос для добавления пользователя.
    /// </summary>
    /// <param name="addUserLogin">Логин нового пользователя.</param>
    /// <param name="addUserPassword">Пароль нового пользователя.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/user/add")]
    public IHttpResponseResult AddUser(string addUserLogin, string addUserPassword, string addUserEmail)
    {
        try
        {
            var user_context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
            var user = user_context.GetUserByLogin(addUserLogin);
            if (user == null)
            {
                User newUser = new User
                {
                    Login = addUserLogin,
                    Password = addUserPassword,
                    Email = addUserEmail
                };
                user_context.Create(newUser);
                user = user_context.GetUserByLogin(addUserLogin);
                return Json(user);
            }

            return Json(false);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding movie: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для удаления пользователя по идентификатору.
    /// </summary>
    /// <param name="deleteUserId">Идентификатор пользователя для удаления.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/user/delete")]
    public IHttpResponseResult DeleteUserById(string deleteUserId)
    {
        try
        {
            var user_context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
            var user = user_context.GetById(int.Parse(deleteUserId));
            if (deleteUserId != null && user.Login != null)
            {
                user_context.Delete(deleteUserId, "Users");
                return Json(user_context.GetAll());
            }
            return Json(false);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting user: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для добавления фильма.
    /// </summary>
    /// <param name="addMovieName">Название фильма.</param>
    /// <param name="addMovieEnglishName">Английское название фильма.</param>
    /// <param name="addMovieYear">Год выпуска фильма.</param>
    /// <param name="addMovieCountry">Страна производства фильма.</param>
    /// <param name="addMovieGenre">Жанр фильма.</param>
    /// <param name="addMovieDirector">Режиссер фильма.</param>
    /// <param name="addMovieActors">Актеры фильма.</param>
    /// <param name="addMovieDescription">Описание фильма.</param>
    /// <param name="addMovieLink">Ссылка на фильм.</param>
    /// <param name="addMovieImageLink">Ссылка на изображение фильма.</param>
    /// <param name="addMovieLLink">Ссылка на фильм.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/movie/add")]
    public IHttpResponseResult AddMovie(string addMovieTitle, int addMovieYear, string addMovieCountry, string addMovieGenre, string addMovieDirector, string addMovieActors, string addMovieDescription, int addMovieDuration, string addMovieImage, string addMovieVideo)
    {
        try
        {
            var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
            var movie = movie_context.GetByTitle(addMovieTitle);

            if (movie != null)
            {
                return Json(false);
            }
            Movie newMovie = new Movie
            {
                title = addMovieTitle,
                release_year = addMovieYear,
                duration = addMovieDuration,
                description = addMovieDescription,
                poster_url = addMovieImage,
                director = addMovieDirector,
                actors = addMovieActors,
                URL_video = addMovieVideo,
                country = addMovieCountry,
                genre = addMovieGenre,
            };
            movie_context.CreateMovie(newMovie);
            movie = movie_context.GetByTitle(addMovieTitle);
            return Json(movie);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding movie: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для удаления фильма по идентификатору.
    /// </summary>
    /// <param name="deleteMovieId">Идентификатор фильма для удаления.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/movie/delete")]
    public IHttpResponseResult DeleteMovieById(string deleteMovieId)
    {
        try
        {
            Console.WriteLine(deleteMovieId);
            var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
            var movie = movie_context.GetById(int.Parse(deleteMovieId));
            if (deleteMovieId != null && movie.title != null)
            {
                movie_context.DeleteMovie(deleteMovieId, "Movies");
                return Json(movie_context.GetAll());
            }
            return Json(false);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting user: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
    
    [Post("admin/user/update")]
public IHttpResponseResult UpdateUser(int updateUserId, string updateUserLogin, string updateUserPassword, string updateUserEmail)
{
    try
    {
        var user_context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
        var existingUser = user_context.GetById(updateUserId);
        if (existingUser == null)
        {
            return Json(false);
        }

        if (!string.IsNullOrEmpty(updateUserLogin))
        {
            existingUser.Login = updateUserLogin;
        }

        if (!string.IsNullOrEmpty(updateUserPassword))
        {
            existingUser.Password = updateUserPassword;
        }
        
        if (!string.IsNullOrEmpty(updateUserEmail))
        {
            existingUser.Email = updateUserEmail;
        }

        user_context.UpdateUser(existingUser);

        return Json(user_context.GetAll());

    }
    catch (Exception ex)
    {
        // Логирование ошибки
        Console.WriteLine("Error updating user: " + ex.Message);
        return Json(new { error = ex.Message });
    }
}

[Post("admin/movie/update")]
public IHttpResponseResult UpdateMovie(int updateMovieId, string updateMovieTitle, string updateMovieYear, string updateMovieCountry, string updateMovieGenre, string updateMovieDirector, string updateMovieActors, string updateMovieDescription, string updateMovieDuration, string updateMovieImage, string updateMovieVideo)
{
    try
    {
        var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionStrings["DefaultConnection"]));
        var existingMovie = movie_context.GetById(updateMovieId);

        if (existingMovie == null)
        {
            return Json(false);
        }

        if (!string.IsNullOrEmpty(updateMovieTitle))
        {
            existingMovie.title = updateMovieTitle;
        }

        if (!string.IsNullOrEmpty(updateMovieYear))
        {
            existingMovie.release_year = int.Parse(updateMovieYear);
        }
        

        if (!string.IsNullOrEmpty(updateMovieDuration))
        {
            existingMovie.duration = int.Parse(updateMovieDuration);
        }

        if (!string.IsNullOrEmpty(updateMovieDescription))
        {
            existingMovie.description = updateMovieDescription;
        }

        if (!string.IsNullOrEmpty(updateMovieDirector))
        {
            existingMovie.director = updateMovieDirector;
        }

        if (!string.IsNullOrEmpty(updateMovieActors))
        {
            existingMovie.actors = updateMovieActors;
        }

        if (!string.IsNullOrEmpty(updateMovieVideo))
        {
            existingMovie.URL_video = updateMovieVideo;
        }

        if (!string.IsNullOrEmpty(updateMovieCountry))
        {
            existingMovie.country = updateMovieCountry;
        }

        if (!string.IsNullOrEmpty(updateMovieGenre))
        {
            existingMovie.genre = updateMovieGenre;
        }

        movie_context.UpdateMovie(existingMovie);

        return Json(movie_context.GetAll());



    }
    catch (Exception ex)
    {
        // Логирование ошибки
        Console.WriteLine("Error adding movie: " + ex.Message);
        return Json(new { error = ex.Message });
    }
}
}
