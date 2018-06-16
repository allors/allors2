namespace Commands.Verbs.Scheduler
{
    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using CommandLine;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class Daily 
    {
        private readonly string userName;
        private readonly IDatabase database;
        private readonly ILogger<Daily> logger;

        public Daily(IConfiguration configuration, IDatabaseService databaseService, ILogger<Daily> logger)
        {
            this.userName = configuration["user"];
            this.database = databaseService.Database;
            this.logger = logger;
        }

        public int Execute(Options options)
        {
            this.logger.LogInformation("Begin");

            using (var session = this.database.CreateSession())
            {
                var user = new Users(session).GetUser(this.userName);
                session.SetUser(user);
                //Assignments.Daily(session, logAdapter);
            }

            this.logger.LogInformation("End");

            return 0;
        }

        [Verb("daily", HelpText = "Daily scheduler.")]
        public class Options
        {
        }

        // TODO: Add Polly for snapshot retry
        //catch (SqlException sqlException)
        //{
        //    if (sqlException.ToString().ToLowerInvariant().Contains("snapshot"))
        //    {
        //        logger.Warn("Snapshot isolation conflict.");
        //        return Success;
        //    }

        //    logger.Error(sqlException);
        //    return Failure;
        //}
    }
}
