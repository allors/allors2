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

            var confirmationKey = Console.ReadKey(true).KeyChar.ToString();
            if (confirmationKey.ToLower().Equals("y"))
            {
                Console.WriteLine("Populating");

                Console.WriteLine("Init database");
                database.Init();

                using (var session = database.CreateSession())
                {
                    Console.WriteLine("Processing");

                    var dataDirectory = new DirectoryInfo(this.DataPath);
                    new Setup(session, dataDirectory).Apply();
                    session.Commit();

                    Console.WriteLine("Populated");
                }
            }
        }
    }
}
