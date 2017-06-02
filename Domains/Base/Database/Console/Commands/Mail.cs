namespace Allors.Console
{
    using Allors.Domain;

    public class Mail : Command
    {
        public override void Execute()
        {
            var database = this.CreateDatabase();
            using (var session = database.CreateSession())
            {
                var emailMessages = new EmailMessages(session);
                emailMessages.Send();

                session.Commit();
            }
        }
    }
}
