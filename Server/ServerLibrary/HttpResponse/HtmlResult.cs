using System.Text;

namespace HttpServerLibrary.Core.HttpResponse;

/// <summary>
/// Класс HtmlResult представляет результат HTTP-ответа в формате HTML.
/// </summary>
public class HtmlResult : IHttpResponseResult
{
    private readonly string _html;

    /// <summary>
    /// Инициализирует новый экземпляр класса HtmlResult с заданным HTML-контентом.
    /// </summary>
    /// <param name="html">Строка, содержащая HTML-контент, который будет отправлен в ответе.</param>
    public HtmlResult(string html)
    {
        _html = html;
    }

    /// <summary>
    /// Выполняет обработку HTTP-ответа, отправляя HTML-контент в поток ответа.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса, который содержит информацию о запросе и ответе.</param>
    public void Execute(HttpRequestContext context)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(_html);

        // получаем поток ответа и пишем в него ответ
        context.Response.ContentLength64 = buffer.Length;
        using Stream output = context.Response.OutputStream;

        // отправляем данные
        output.Write(buffer);
        output.Flush();
    }
}