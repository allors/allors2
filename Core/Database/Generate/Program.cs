// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.IO;

    using Allors.Development.Repository.Tasks;

    internal class Program
    {
        private static int Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return Default();

                case 2:
                    return Generate.Execute(args[0], args[1]).ErrorOccured ? 1 : 0;

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

                RemoveDirectory(output);

                var log = Generate.Execute(template, output);
                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            return 0;
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
