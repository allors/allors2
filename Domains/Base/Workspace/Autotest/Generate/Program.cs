namespace Allors
{
    using System;
    using System.IO;

    using Allors.Development.Repository.Tasks;

    internal class Program
    {
        private static int Default()
        {
            string[,] config =
                {
                    { "./Workspace/Autotest/Templates/sidenav.cs.stg", "./Workspace/Typescript/modules/Material.Tests/generated/menu" },
                    { "./Workspace/Autotest/Templates/component.cs.stg", "./Workspace/Typescript/modules/Material.Tests/generated/components" },
                };

            for (var i = 0; i < config.GetLength(0); i++)
            {
                var template = config[i, 0];
                var output = config[i, 1];

                Console.WriteLine($"{template} -> {output}");

                RemoveDirectory(output);

                var log = Generate.Execute(template, output);
                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static int Main(string[] args)
        {
            try
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