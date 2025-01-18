namespace HttpServerLibrary.Attributes;
/// <summary>
/// Атрибут GetAttribute используется для обозначения методов, обрабатывающих 
/// HTTP GET-запросы для определенного маршрута.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class GetAttribute : Attribute
{
    /// <summary>
    /// Получает маршрут, связанный с этим GET-методом.
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GetAttribute"/>,
    /// используя указанный маршрут.
    /// </summary>
    /// <param name="route">Маршрут, который должен обрабатывать метод.</param>
    public GetAttribute(string route)
    {
        Route = route;
    }
}