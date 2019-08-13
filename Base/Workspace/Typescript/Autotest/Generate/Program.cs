namespace Allors
{
    using Allors.Workspace.Meta;
    using Autotest;
    using System;
    using System.IO;

    using Allors.Development.Repository.Tasks;

    internal class Program
    {
        private static int Default(Model model)
        {
            try
            {
                string[,] config =
                {
                    {
                        "../Core/Workspace/Typescript/Autotest/Templates/sidenav.cs.stg", "./Workspace/Typescript/Intranet.Tests/generated/sidenav"
                    },
                    {
                        "../Core/Workspace/Typescript/Autotest/Templates/component.cs.stg", "./Workspace/Typescript/Intranet.Tests/generated/components"
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
                    MetaPopulation = MetaPopulation.Instance
                };

                model.LoadMetaExtensions(new FileInfo("./Workspace/Typescript/Intranet/dist/autotest/meta.json"));
                model.LoadProject(new FileInfo("./Workspace/Typescript/Autotest/Angular/dist/project.json"));
                model.LoadMenu(new FileInfo("./Workspace/Typescript/Intranet/dist/autotest/menu.json"));
                model.LoadDialogs(new FileInfo("./Workspace/Typescript/Intranet/dist/autotest/dialogs.json"));

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