using HttpServerLibrary.Attributes;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;

namespace MyServer.Endpoints;

internal class TestEndpoint : EndpointBase
{
    [Get("wow")]
    public IHttpResponseResult GetWow(string hello)
    {
        Console.WriteLine(hello);
        string responseText = "Ура " + hello;
        return Html(responseText);
    }
}
