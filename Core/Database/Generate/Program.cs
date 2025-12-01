// <copyright file="Program.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Allors.Development.Repository.Tasks;
    using Allors.Meta;
    using Development.Repository;
    using Development.Repository.Generation;
    using Development.Repository.Tagged;

    internal class Program
    {
        private static int Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return Default() | Diagrams() | Mapping();

                case 2:
                    return Generate(args[0], args[1]).ErrorOccured ? 1 : 0;

                default:
                    return 1;
            }
        }

        private static int Default()
        {
            string[,] config =
                {
                    { "Database/Templates/domain.cs.stg", "Database/Domain/generated" },
                    { "Database/Templates/uml.cs.stg", "Database/Domain.Diagrams/generated" },

                    { "Workspace/CSharp/Templates/uml.cs.stg", "Workspace/CSharp/Diagrams/generated" },
                    { "Workspace/CSharp/Templates/meta.cs.stg", "Workspace/CSharp/Meta/generated" },
                    { "Workspace/CSharp/Templates/domain.cs.stg", "Workspace/CSharp/Domain/generated" },

                    { "Workspace/Typescript/Templates/meta.ts.stg", "Workspace/Typescript/libs/meta/generated/src" },
                    { "Workspace/Typescript/Templates/domain.ts.stg", "Workspace/Typescript/libs/domain/generated/src" },

                    { "Workspace/Typescript.legacy/Templates/meta.ts.stg", "Workspace/Typescript.legacy/Site/wwwroot/generated/meta" },
                    { "Workspace/Typescript.legacy/Templates/domain.ts.stg", "Workspace/Typescript.legacy/Site/wwwroot/generated/domain" },
                };

            for (var i = 0; i < config.GetLength(0); i++)
            {
                var template = config[i, 0];
                var output = config[i, 1];

                Console.WriteLine("-> " + output);

                if (output.ToLowerInvariant().EndsWith("generated"))
                {
                    RemoveDirectory(output);
                }

                var log = Generate(template, output);

                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static int Diagrams()
        {
            var metaPopulation = MetaPopulation.Instance;
            var tags = new HashSet<string>(
                metaPopulation.Classes.SelectMany(v => v.Tags)
                    .Union(metaPopulation.RelationTypes.SelectMany(v => v.Tags)
                        .Union(metaPopulation.MethodTypes.SelectMany(v => v.Tags))));

            foreach (var tag in tags)
            {
                var log = new GenerateLog();

                var taggedMetaPopulation = new TaggedMetaPopulation(metaPopulation, new HashSet<string>([tag]));
                var stringTemplate = new TaggedStringTemplate(new FileInfo("Database/Templates/mermaid.stg"));
                var outputDirectoryInfo = new DirectoryInfo("Database/Docs");

                var output = tag + ".mmd";
                Console.WriteLine("-> " + Path.Combine(outputDirectoryInfo.Name, output));

                stringTemplate.Generate(taggedMetaPopulation, outputDirectoryInfo, output, log);

                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static int Mapping()
        {
            var metaPopulation = MetaPopulation.Instance;
            var tags = new HashSet<string>(
                metaPopulation.Classes.SelectMany(v => v.Tags)
                    .Union(metaPopulation.RelationTypes.SelectMany(v => v.Tags)
                        .Union(metaPopulation.MethodTypes.SelectMany(v => v.Tags))));

            var log = new GenerateLog();

            var taggedMetaPopulation = new TaggedMetaPopulation(metaPopulation, tags);
            var stringTemplate = new TaggedStringTemplate(new FileInfo("Database/Templates/mapping.xml.stg"));
            var outputDirectoryInfo = new DirectoryInfo("Database/Docs");

            var output = "mapping.xml";
            Console.WriteLine("-> " + Path.Combine(outputDirectoryInfo.Name, output));

            stringTemplate.Generate(taggedMetaPopulation, outputDirectoryInfo, output, log);

            if (log.ErrorOccured)
            {
                return 1;
            }

            return 0;
        }

        public static Log Generate(string template, string output)
        {
            var log = new GenerateLog();

            var templateFileInfo = new FileInfo(template);
            var stringTemplate = new StringTemplate(templateFileInfo);
            var outputDirectoryInfo = new DirectoryInfo(output);

            stringTemplate.Generate(MetaPopulation.Instance, outputDirectoryInfo, log);

            return log;
        }

        private static void RemoveDirectory(string output)
        {
            var directoryInfo = new DirectoryInfo(output);
            if (directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Delete(true);
                }
                catch
                {
                }
            }
        }
    }
}
