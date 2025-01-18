using HttpServerLibrary.Configurations;
using HttpServerLibrary;

namespace Server;

class Program
{
    static void Main(string[] args)
    {
        var config = AppConfig.GetInstance();
        var prefixes = new[] { $"http://{config.Domain}:{config.Port}/" };
        var server = new HttpServer(prefixes);
        server.Start();
    }
}