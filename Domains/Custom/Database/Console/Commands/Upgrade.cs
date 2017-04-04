namespace Allors.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Allors.Domain;

    public class Upgrade : Command
    {
        private readonly HashSet<Guid> excludedObjectTypes = new HashSet<Guid>
        {
        };

        private readonly HashSet<Guid> excludedRelationTypes = new HashSet<Guid>
        {
        };

        public override void Execute()
        {
            var database = this.RepeatableReadDatabase; // Or Serializable
            
            try
            {
                Console.WriteLine("Upgrading");

                var notLoadedObjectTypeIds = new HashSet<Guid>();
                var notLoadedRelationTypeIds = new HashSet<Guid>();
                using (var reader = XmlReader.Create(this.PopulationFileName))
                {
                    database.ObjectNotLoaded += (sender, args) =>
                    {
                        if (!this.excludedObjectTypes.Contains(args.ObjectTypeId))
                        {
                            notLoadedObjectTypeIds.Add(args.ObjectTypeId);
                        }
                    };

                    database.RelationNotLoaded += (sender, args) =>
                    {
                        if (!this.excludedRelationTypes.Contains(args.RelationTypeId))
                        {
                            notLoadedRelationTypeIds.Add(args.RelationTypeId);
                        }
                    };

                    Console.WriteLine("Loading");
                    database.Load(reader);
                    Console.WriteLine("Loaded");
                }

                if (notLoadedObjectTypeIds.Count > 0)
                {
                    var notLoaded = notLoadedObjectTypeIds
                        .Aggregate("Could not load following ObjectTypeIds: ", (current, objectTypeId) => current + ("- " + objectTypeId));

                    Console.WriteLine(notLoaded);
                    return;
                }

                if (notLoadedRelationTypeIds.Count > 0)
                {
                    var notLoaded = notLoadedRelationTypeIds
                        .Aggregate("Could not load following RelationTypeIds: ", (current, relationTypeId) => current + ("- " + relationTypeId));

                    Console.WriteLine(notLoaded);
                    return;
                }

                using (var session = database.CreateSession())
                {
                    Console.WriteLine("Processing");

                    // Call upgrade methods

                    new Permissions(session).Sync();
                    new Security(session).Apply();
                    session.Commit();
                }

                Console.WriteLine("Upgraded");
            }
            catch
            {
                Console.WriteLine("Please correct errors or restore from backup");
                throw;
            }
        }
    }
}
