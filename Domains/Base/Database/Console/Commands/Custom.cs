namespace Allors.Console
{
    public class Custom : Command
    {
        public override void Execute()
        {
            var database = this.CreateDatabase();
            using (var session = database.CreateSession())
            {
                this.SetIdentity("Administrator");


                session.Derive();
                session.Commit();
            }
        }
    }
}
