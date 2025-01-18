using HttpServerLibrary.Configurations;

namespace Server.Helpers;

public class ResponseHelper : IResponseHelper
{
    /// <summary>
    /// �������� ����� ������ �� ����� �� ���������� ���������� ����.
    /// </summary>
    /// <param name="localPath">��������� ���� � �����.</param>
    /// <returns>����� ������ �� ����� ��� ��������� �� ������, ���� ���� �� ������.</returns>
    public string GetResponseText(string localPath)
    {
        var filePath = AppConfig.GetInstance().Path + localPath;
        var additionalPath = AppConfig.GetInstance().Path + localPath;
        if (File.Exists(filePath))
        {
            var responseText = File.ReadAllText(filePath);
            return responseText;
        }
        else if (File.Exists(additionalPath))
        {
            var responseText = File.ReadAllText(additionalPath);
            return responseText;
        }

        return "error 404 file not found";
    }
}