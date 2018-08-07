namespace Commands.Verbs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using CommandLine;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    
    public class Upgrade
    {
        private readonly string dataPath;
        private readonly IDatabase database;
        private readonly ILogger<Upgrade> logger;

        private readonly HashSet<Guid> excludedObjectTypes = new HashSet<Guid>
        {
        };

        private readonly HashSet<Guid> excludedRelationTypes = new HashSet<Guid>
        {
        };

        private readonly HashSet<Guid> movedRelationTypes = new HashSet<Guid>
        {
        };

        public Upgrade(IConfiguration configuration, IDatabaseService databaseService, ILogger<Upgrade> logger)
        {
            this.dataPath = configuration["datapath"];
            this.database = databaseService.Database;
            this.logger = logger;
        }

        public int Execute(Options opts)
        {
            var fileInfo = new FileInfo(opts.File);
           
            this.logger.LogInformation("Begin");
            
            var notLoadedObjectTypeIds = new HashSet<Guid>();
            var notLoadedRelationTypeIds = new HashSet<Guid>();

            var notLoadedObjects = new HashSet<long>();

            using (var reader = XmlReader.Create(fileInfo.FullName))
            {
                this.database.ObjectNotLoaded += (sender, args) =>
                {
                    if (!this.excludedObjectTypes.Contains(args.ObjectTypeId))
                    {
                        notLoadedObjectTypeIds.Add(args.ObjectTypeId);
                    }
                    else
                    {
                        var id = args.ObjectId;
                        notLoadedObjects.Add(id);
                    }
                };

                this.database.RelationNotLoaded += (sender, args) =>
                {
                    if (!this.excludedRelationTypes.Contains(args.RelationTypeId))
                    {
                        if (!notLoadedObjects.Contains(args.AssociationId))
                        {
                            notLoadedRelationTypeIds.Add(args.RelationTypeId);
                        }
                    }
                };

                this.logger.LogInformation("Loading {file}", fileInfo.FullName);
                this.database.Load(reader);
            }

            if (notLoadedObjectTypeIds.Count > 0)
            {
                var notLoaded = notLoadedObjectTypeIds
                    .Aggregate("Could not load following ObjectTypeIds: ", (current, objectTypeId) => current + ("- " + objectTypeId));

                this.logger.LogError(notLoaded);
                return 1;
            }

            if (notLoadedRelationTypeIds.Count > 0)
            {
                var notLoaded = notLoadedRelationTypeIds
                    .Aggregate("Could not load following RelationTypeIds: ", (current, relationTypeId) => current + ("- " + relationTypeId));

                this.logger.LogError(notLoaded);
                return 1;
            }

            using (var session = this.database.CreateSession())
            {
                new Allors.Upgrade(session, new DirectoryInfo(this.dataPath)).Execute();
                session.Commit();

                new Permissions(session).Sync();
                new Security(session).Apply();

                session.Commit();
            }

            this.logger.LogInformation("End");
            return 0;
        }

        [Verb("upgrade", HelpText = "Upgrade the database.")]
        public class Options
        {
            [Option('f', "file", Required = false, HelpText = "File to load from.")]
            public string File { get; set; }
        }
    }
}
