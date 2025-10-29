using System.Configuration;

namespace apiservice_consumer_with_bearer.Config;

public static class ConfigurationHelper
{
    public static AppSettings LoadAppSettings()
    {
        var settings = new AppSettings
        {
            BaseUri = ConfigurationManager.AppSettings["BaseUri"] ?? string.Empty,
            Usuario = ConfigurationManager.AppSettings["Usuario"] ?? string.Empty,
            Senha = ConfigurationManager.AppSettings["Senha"] ?? string.Empty
        };

        return settings;
    }
}
