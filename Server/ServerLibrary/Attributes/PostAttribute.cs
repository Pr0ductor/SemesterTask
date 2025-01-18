namespace HttpServerLibrary.Attributes;
// <summary>
/// Указывает, что метод обрабатывает HTTP POST запросы по указанному маршруту.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class PostAttribute : Attribute
{
    /// <summary>
    /// Получает маршрут для POST запроса.
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="PostAttribute"/> с заданным маршрутом.
    /// </summary>
    /// <param name="route">Маршрут, по которому будет доступен метод.</param>
    public PostAttribute(string route)
    {
        Route = route;
    }
}

