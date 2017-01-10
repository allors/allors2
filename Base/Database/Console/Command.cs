namespace Allors
{
    using System;
    using System.Configuration;

    using NLog;

    public abstract class Command
    {
        protected Logger Logger { get; }

        protected string DataPath => ConfigurationManager.AppSettings["dataPath"];

        protected string PopulationFileName = "population.xml";

        protected IDatabase SnapshotDatabase
        {
            get
            {
                var configuration = new Adapters.Object.SqlClient.Configuration
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["allors"].ConnectionString,
                    ObjectFactory = Config.ObjectFactory,
                    IsolationLevel = System.Data.IsolationLevel.Snapshot,
                    CommandTimeout = 0
                };
                return new Adapters.Object.SqlClient.Database(configuration);
            }
        }

        protected IDatabase RepeatableReadDatabase
        {
            get
            {
                var configuration = new Adapters.Object.SqlClient.Configuration
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["allors"].ConnectionString,
                    ObjectFactory = Config.ObjectFactory,
                    IsolationLevel = System.Data.IsolationLevel.RepeatableRead,
                    CommandTimeout = 0
                };
                return new Adapters.Object.SqlClient.Database(configuration);
            }
        }

        protected IDatabase SerializableDatabase
        {
            get
            {
                var configuration = new Adapters.Object.SqlClient.Configuration
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["allors"].ConnectionString,
                    ObjectFactory = Config.ObjectFactory,
                    IsolationLevel = System.Data.IsolationLevel.Serializable,
                    CommandTimeout = 0
                };
                return new Adapters.Object.SqlClient.Database(configuration);
            }
        }

        public abstract void Execute();

        protected Command()
        {
            this.Logger = LogManager.GetCurrentClassLogger();
        }

        protected void Derive(ISession session, Extent extent)
        {
            var derivation = new Domain.NonLogging.Derivation(session, extent.ToArray());
            var validation = derivation.Derive();
            if (validation.HasErrors)
            {
                foreach (var error in validation.Errors)
                {
                    this.Logger.Error(error.Message);
                }

                throw new Exception("Derivation Error");
            }
        }
    }
}
