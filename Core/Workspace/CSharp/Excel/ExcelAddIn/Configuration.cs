using System.Configuration;

namespace ExcelAddIn
{
    public class Configuration
    {
        public string AllorsDatabaseAddress => ConfigurationManager.AppSettings["allors.database.address"];

        public string Environment => ConfigurationManager.AppSettings["environment"];

        public string AutoLogin => ConfigurationManager.AppSettings["autologin"];
    }
}
