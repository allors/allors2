// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All Rights Reserved.
// Licensed under the LGPL v3 license.
// </copyright>

namespace Allors
{
    using System;
    using System.IO;
    using Allors.Development.Repository.Tasks;
    using Allors.Workspace.Meta;
    using Autotest;

    internal class Program
    {
        private static int Default(Model model)
        {
            try
            {
                string[,] config =
                {
                    {
                        "./Workspace/Scaffold/Templates/sidenav.cs.stg", "./Workspace/Scaffold/Angular.Material.Tests/generated/sidenav",
                    },
                    {
                        "./Workspace/Scaffold/Templates/component.cs.stg", "./Workspace/Scaffold/Angular.Material.Tests/generated/components",
                    },
                };

                for (var i = 0; i < config.GetLength(0); i++)
                {
                    var template = config[i, 0];
                    var output = config[i, 1];

                    Console.WriteLine($"{template} -> {output}");

                    RemoveDirectory(output);

                    var log = Generate.Execute(template, output, model);
                    if (log.ErrorOccured)
                    {
                        return 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + e.StackTrace);
            }

            return 0;
        }

        private static int Main(string[] args)
        {
            try
            {
                var model = new Model
                {
                    MetaPopulation = MetaPopulation.Instance,
                };

                model.LoadMetaExtensions(new FileInfo("./Workspace/Typescript/dist/scaffold/meta.json"));
                model.LoadProject(new FileInfo("./Workspace/Typescript/dist/scaffold/project.json"));
                model.LoadMenu(new FileInfo("./Workspace/Typescript/dist/scaffold/menu.json"));

                switch (args.Length)
                {
                    case 0:
                        return Default(model);

                    case 2:
                        return Generate.Execute(args[0], args[1], model).ErrorOccured ? 1 : 0;

                    default:
                        return 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
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
