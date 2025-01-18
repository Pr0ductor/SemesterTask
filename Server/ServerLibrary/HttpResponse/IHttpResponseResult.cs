namespace HttpServerLibrary.Core.HttpResponse;

/// <summary>
/// Интерфейс IHttpResponseResult определяет контракт для результатов HTTP-ответов.
/// Он используется для выполнения обработки результатов, связанных с HTTP-запросами.
/// </summary>
public interface IHttpResponseResult
{
    /// <summary>
    /// Выполняет обработку HTTP-ответа для заданного контекста HTTP-запроса.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса, который содержит информацию о запросе.</param>
    void Execute(HttpRequestContext context);
}
