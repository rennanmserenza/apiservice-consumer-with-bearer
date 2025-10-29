    public static class ConfigurationHelper
    {
        public static AppSettings LoadAppSettings()
        {
            var settings = new AppSettings
            {
                BaseUri = ConfigurationManager.AppSettings["BaseUri"],
                Usuario = ConfigurationManager.AppSettings["Usuario"],
                Senha = ConfigurationManager.AppSettings["Senha"]
            };

            return settings;
        }
    }