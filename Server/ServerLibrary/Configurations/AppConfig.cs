using System.Text.Json;
using System.Text.Json.Serialization;


namespace HttpServerLibrary.Configurations;

/// <summary>
/// Конфигурация приложения.
/// </summary>
public sealed class AppConfig
{
    /// <summary>
    /// Имя файла конфигурации.
    /// </summary>
    public static string FILE_NAME = "config.json"; 

    /// <summary>
    /// Домен приложения.
    /// </summary>
    public string Domain { get; set; } = "localhost";

    /// <summary>
    /// Порт, на котором работает приложение.
    /// </summary>
    public uint Port { get; set; } = 8080;

    /// <summary>
    /// Путь к статическим файлам.
    /// </summary>
    public string Path { get; set; } = "public/";

    /// <summary>
    /// Конфигурация для службы электронной почты.
    /// </summary>
    public EmailServiceConfig EmailServiceConfiguration { get; set; } = new ();

    public string ConnectionString { get; set; } =
        @"Data Source = localhost;Initial Catalog=PersonDB;User ID=sa;Password=MidnightProductor007;";

    private static AppConfig _instance;

    /// <summary>
    /// Приватный конструктор для предотвращения создания экземпляров класса извне.
    /// </summary>
    [JsonConstructor]
    private AppConfig() {}

    /// <summary>
    /// Получает единственный экземпляр класса AppConfig.
    /// </summary>
    /// <returns>Экземпляр класса AppConfig.</returns>
    public static AppConfig GetInstance()
    {
        if (_instance is null)
        {
            _instance = new AppConfig();
            _instance.Initialize();
        }

        return _instance;
    }

    /// <summary>
    /// Инициализирует конфигурацию, загружая данные из config.json, если он существует.
    /// </summary>
    private void Initialize()
    {
        if (File.Exists(FILE_NAME))
        {
            var configFile = File.ReadAllText(FILE_NAME);
            _instance = JsonSerializer.Deserialize<AppConfig>(configFile);
        }
        else
        {
            Console.WriteLine($"Файл настроек {AppConfig.FILE_NAME} не найден");
        }
    }
}