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

            var confirmationKey = Console.ReadKey(true).KeyChar.ToString(CultureInfo.InvariantCulture);
            if (confirmationKey.ToLower().Equals("y"))
            {
                this.Logger.Info("Creating Demo");

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
                            this.Logger.Error(error);
                        }

                        this.Logger.Info("Demo not created");
                    }
                    else
                    {
                        session.Commit();

                        this.Logger.Info("Demo created");
                    }
                }
            }
        }
    }
}
