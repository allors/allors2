namespace Allors.Console
{
    using Allors.Domain;

    public class Custom : Command
    {
        public override void Execute()
        {
            var database = this.CreateDatabase();
            using (var session = database.CreateSession())
            {
                var administrator = new Users(session).GetUser("administrator");
                session.SetUser(administrator);


                session.Derive();
                session.Commit();
            }
        }
    }
}
