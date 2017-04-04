namespace Allors.Commands
{
    using System;
    using System.Globalization;

    public class Demo : Command
    {
        public override void Execute()
        {
            var database = this.RepeatableReadDatabase; // Or Serializable

            Console.WriteLine("Are you sure, all current data will be destroyed? (Y/N)\n");

            var confirmationKey = Console.ReadKey(true).KeyChar.ToString();
            if (confirmationKey.ToLower().Equals("y"))
            {
                Console.WriteLine("Creating Demo");

                database.Init();

                using (var session = database.CreateSession())
                {
                    var setup = new Setup(session, null);

                    setup.Apply();

                    setup.ApplyDemo();

                    var validation = session.Derive();
                    if (validation.HasErrors)
                    {
                        foreach (var error in validation.Errors)
                        {
                            Console.WriteLine(error);
                        }

                        Console.WriteLine("Demo not created");
                    }
                    else
                    {
                        session.Commit();

                        Console.WriteLine("Demo created");
                    }
                }
            }
        }
    }
}
