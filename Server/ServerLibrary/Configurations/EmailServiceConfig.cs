namespace HttpServerLibrary.Configurations;

/// <summary>
/// Настройки конфигурации для службы электронной почты,
/// </summary>
public class EmailServiceConfig
{
    /// <summary>
    /// Хост для отправки электронной почты
    /// </summary>
    public string Host { get; set; } = "";

    /// <summary>
    /// Порт для подключения к хосту электронной почты.
    /// </summary>
    public int Port { get; set; } = 25;

    /// <summary>
    /// Имя пользователя для аутентификации на почтовом сервере.
    /// </summary>
    public string UserName { get; set; } = "";

    /// <summary>
    /// Пароль для аутентификации на почтовом сервере.
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// Указывает, следует ли использовать SSL для защищенного соединения.
    /// </summary>
    public bool IsSSL { get; set; } = true;
}