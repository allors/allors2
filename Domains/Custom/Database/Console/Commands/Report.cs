namespace Allors.Commands
{
    using System;
    using System.Linq;

    using Allors.Domain;

    public class Report : Command
    {
        private readonly IDatabase database;

        public Report()
        {
            this.database = this.SnapshotDatabase;
        }

        public override void Execute()
        {
            this.Security();
        }
    
        public void Security()
        {
            using (var session = this.database.CreateSession())
            {
                Console.WriteLine("Roles:");
                foreach (var role in new Roles(session).Extent().OrderBy(v=>v.Name))
                {
                    Console.WriteLine("- " + role.Name);
                }

                Console.WriteLine();

                Console.WriteLine("User Groups:");
                foreach (var userGroup in new UserGroups(session).Extent().OrderBy(v => v.Name))
                {
                    Console.WriteLine("- " + userGroup.Name);
                }
            }
        }

        public void Index()
        {
            Console.WriteLine("Indexes:");

            var metaPopulation = this.database.MetaPopulation;
            var relationTypes = metaPopulation.RelationTypes.Where(v => v.RoleType.ObjectType.IsComposite && v.IsIndexed == false).ToList();
            relationTypes.ForEach(Console.WriteLine);
        }
    }
}
