using System;

namespace Allors
{
    using Allors.Meta;
    using Allors.Domain;

    public abstract class Command
    {
        protected ObjectFactory ObjectFactory = new Allors.ObjectFactory(MetaPopulation.Instance, typeof(IObject), typeof(User));

        protected string ConnectionString = "server=(custom);database=base;Integrated Security=SSPI";

        protected string DataPath => "../../data";

        protected string PopulationFileName = "population.xml";

        protected IDatabase SnapshotDatabase
        {
            get
            {
                var configuration = new Adapters.Object.SqlClient.Configuration
                {
                    ConnectionString = this.ConnectionString,
                    ObjectFactory = this.ObjectFactory,
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
                    ConnectionString = this.ConnectionString,
                    ObjectFactory = this.ObjectFactory,
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
                    ConnectionString = this.ConnectionString,
                    ObjectFactory = this.ObjectFactory,
                    IsolationLevel = System.Data.IsolationLevel.Serializable,
                    CommandTimeout = 0
                };
                return new Adapters.Object.SqlClient.Database(configuration);
            }
        }

        public abstract void Execute();

        protected void Derive(ISession session, Extent extent)
        {
            var derivation = new Domain.NonLogging.Derivation(session, extent.ToArray());
            var validation = derivation.Derive();
            if (validation.HasErrors)
            {
                foreach (var error in validation.Errors)
                {
                    Console.WriteLine(error.Message);
                }

                throw new Exception("Derivation Error");
            }
        }
    }
}
