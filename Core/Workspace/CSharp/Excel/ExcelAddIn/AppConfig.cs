namespace ExcelAddIn
{
    using System.Configuration;

    public class AppConfig
    {
        public string AllorsDatabaseAddress => ConfigurationManager.AppSettings["allors.database.address"];

        public string Environment => ConfigurationManager.AppSettings["environment"];

        public string AutoLogin => ConfigurationManager.AppSettings["autologin"];
    }
}
