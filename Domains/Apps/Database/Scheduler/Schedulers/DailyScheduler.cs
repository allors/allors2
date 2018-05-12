namespace Allors.Scheduler
{
    using Allors.Domain;

    public class DailyScheduler : Scheduler
    {
        public override void Schedule()
        {
            var database = this.CreateDatabase();

            var logAdapter = new LogAdapter(this.Logger);

            this.Logger.Info("Deriving Assignments");
            using (var session = database.CreateSession())
            {
                var user = new Users(session).GetUser(this.Configuration["user"]);
                session.SetUser(user);
                //Assignments.Daily(session, logAdapter);
            }

            this.Logger.Info("Daily Scheduler finished");
        }
    }
}
