using System.Net;
using HttpServerLibrary.Core;
using HttpServerLibrary.Configurations;
using HttpServerLibrary.Handlers;
using HttpServerLibrary.Models;


namespace HttpServerLibrary;

internal sealed class StaticFilesHandler : Handler
{
    //TODO брать из конфига поправить
    public string _staticDirectoryPath = AppConfig.GetInstance().Path;
    public override void HandleRequest(HttpRequestContext context)
    {
        bool isGet = context.Request.HttpMethod.Equals("GET", StringComparison.CurrentCultureIgnoreCase);
        string[] arr = context.Request.Url.AbsolutePath.Split(".");
        bool isFile = arr.Length >= 2;
        // некоторая обработка запроса
        var isIndexRequest = false;
        string absolutePath = context.Request.Url.AbsolutePath;
        if (absolutePath.EndsWith("/"))
        {
            absolutePath += "index.html";
            isIndexRequest = true;
        }
        if (isGet && isFile)
        {
            string filePath = Path.Combine(_staticDirectoryPath, context.Request.Url.AbsolutePath.TrimStart('/'));
            if (File.Exists(filePath))
            {
                SendContentResponse(context.Response, filePath);
            }
            else
            {
                SendNotFoundResponse(context.Response);
            }
            
        }
        // передача запроса дальше по цепи при наличии в ней обработчиков
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
    
    private void SendContentResponse(HttpListenerResponse response, string filePath)
    {
        // определение типа контента
        response.ContentType = GetContentType(filePath);
            
        // Формирование ответа отправляемый в ответ код html возвращает
        byte[] buffer = File.ReadAllBytes(filePath);

        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);

        // Закрываем ответ
        response.OutputStream.Close();
    }
    
    private void SendNotFoundResponse(HttpListenerResponse response)
    {
        byte[] buffer = File.ReadAllBytes(_staticDirectoryPath + "/404.html");
        response.StatusCode = 404;
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
    
    private string GetContentType(string pathToFile)
    {
        string extension = Path.GetExtension(pathToFile).ToLower();
        if (extension == ".html")
        {
            return "text/html";
        }
        else if (extension == ".css")
        {
            return "text/css";
        }
        else if (extension == ".png")
        {
            return "image/png";
        }
        else if (extension == ".jpeg")
        {
            return "image/jpeg";
        }
        else if (extension == ".svg")
        {
            return "image/svg+xml";
        }
        else if (extension == ".webp")
        {
            return "image/webp";
        }
        else if (extension == ".js")
        {
            return "application/javascript";
        }
        else
        {
            return "application/octet-stream";
        }
    }
}