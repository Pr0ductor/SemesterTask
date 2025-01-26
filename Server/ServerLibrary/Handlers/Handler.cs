using HttpServerLibrary.Core;


namespace HttpServerLibrary.Handlers;

/// <summary>
/// Абстрактный класс Handler определяет общий интерфейс для обработки 
/// запросов в цепочке обработки.
/// </summary>
public abstract class Handler
{
    /// <summary>
    /// Получает или устанавливает следующего обработчика в цепочке.
    /// Если текущий обработчик не может обработать запрос, он передает 
    /// его следующему обработчику.
    /// </summary>
    public Handler Successor { get; set; }

    /// <summary>
    /// Метод для обработки запроса. Это абстрактный метод, 
    /// который должен быть реализован в подклассах.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса, содержащий данные запроса.</param>
    public abstract void HandleRequest(HttpRequestContext context);
}