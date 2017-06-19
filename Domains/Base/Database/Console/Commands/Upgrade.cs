namespace Allors.Console
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Domain;

    public class Upgrade : Command
    {
        public Upgrade(string file)
        {
            this.fileInfo = new FileInfo(file);
        }

        private readonly FileInfo fileInfo;

        private readonly HashSet<Guid> excludedObjectTypes = new HashSet<Guid>
        {
        };

        private readonly HashSet<Guid> excludedRelationTypes = new HashSet<Guid>
        {
        };

        private readonly HashSet<Guid> movedRelationTypes = new HashSet<Guid>
        {
        };

        public override void Execute()
        {
            var database = this.CreateDatabase(IsolationLevel.Serializable);
            
            try
            {
                Console.WriteLine("Upgrading Allors");

                var notLoadedObjectTypeIds = new HashSet<Guid>();
                var notLoadedRelationTypeIds = new HashSet<Guid>();

                var notLoadedObjects = new HashSet<long>();

                using (var reader = XmlReader.Create(this.fileInfo.FullName))
                {
                    database.ObjectNotLoaded += (sender, args) =>
                    {
                        if (!this.excludedObjectTypes.Contains(args.ObjectTypeId))
                        {
                            notLoadedObjectTypeIds.Add(args.ObjectTypeId);
                        }
                        else
                        {
                            notLoadedObjects.Add(args.ObjectId);
                        }
                    };

                    database.RelationNotLoaded += (sender, args) =>
                    {
                        if (!this.excludedRelationTypes.Contains(args.RelationTypeId))
                        {
                            if (!notLoadedObjects.Contains(args.AssociationId))
                            {
                                notLoadedRelationTypeIds.Add(args.RelationTypeId);
                            }
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
                    new Allors.Upgrade(session, new DirectoryInfo(this.DataPath)).Execute();
                    session.Commit();

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
