namespace Allors.Commands
{
    using System;
    using System.Globalization;
    using System.IO;

    public class Populate : Command
    {
        public override void Execute()
        {
            var database = this.RepeatableReadDatabase; // Or Serializable

            Console.WriteLine("Are you sure, all current data will be destroyed? (Y/N)\n");

            var confirmationKey = Console.ReadKey(true).KeyChar.ToString(CultureInfo.InvariantCulture);
            if (confirmationKey.ToLower().Equals("y"))
            {
                this.Logger.Info("Populating");

                this.Logger.Info("Init database");
                database.Init();

                using (var session = database.CreateSession())
                {
                    this.Logger.Info("Processing");

                    var dataDirectory = new DirectoryInfo(this.DataPath);
                    new Setup(session, dataDirectory).Apply();
                    session.Commit();

                    this.Logger.Info("Populated");
                }
            }
        }
    }
}
