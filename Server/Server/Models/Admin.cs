namespace Server.Models;

public class Admin
{
    /// <summary>
    /// Получает или задает уникальный идентификатор администратора.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает логин администратора.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Получает или задает пароль администратора.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Получает или задает имя администратора.
    /// </summary>
    public string Email { get; set; }
}