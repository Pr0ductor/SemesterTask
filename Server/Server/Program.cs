using System.Data.SqlClient;
using HttpServerLibrary.Configurations;
using HttpServerLibrary;
using HttpServerLibrary.Handlers;
using HttpServerLibrary.Models;

namespace Server;

class Program
{
    
    /// <summary>
    /// Точка входа в приложение
    /// </summary>
    /// <param name="args">Аргументы при запуске приложения</param>
    static void Main(string[] args)
    {
        
        var config = AppConfig.GetInstance(); // инициализируем AppConfig
        var connectionString = config.ConnectionStrings["DefaultConnection"];

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Подключение к базе данных успешно!");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
        }
        var prefixes = new[] { $"http://{config.Domain}:{config.Port}/" }; // Собираем префиксы в список
        var server = new HttpServerWithTypes(prefixes, config.Path); // создаем сервер с префиксами
        Console.WriteLine("НЕ ОБРАЩАЙТЕ ВНИМАНИЕ НА АДРЕСС СВЕРХУ.\nВОТ : http://localhost:8888/");
        server.Start(); // запускаем сервер
    }

}