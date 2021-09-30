namespace ExcelDNA
{
    using System.Configuration;

    public class AppConfig
    {
        public string AllorsDatabaseAddress => ConfigurationManager.AppSettings["allors.database.address"];

        public string AllorsAuthenticationTokenUrl => ConfigurationManager.AppSettings["allors.authentication.token.url"];

        public string Environment => ConfigurationManager.AppSettings["environment"];

        public string AutoLogin => ConfigurationManager.AppSettings["autologin"];
    }
}
